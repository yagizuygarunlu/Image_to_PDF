using System.Drawing;
using System.Drawing.Imaging;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using DrawingImage = System.Drawing.Image;

namespace Image_to_PDF;

public partial class MainForm : Form
{
    private string? _selectedImagePath;
    private readonly Label _previewLabel;
    private readonly PictureBox _previewBox;
    private readonly Button _selectButton;
    private readonly Button _pasteButton;
    private readonly Button _convertButton;
    private readonly TextBox _outputPathBox;
    private readonly Button _browseOutputButton;
    private readonly Label _statusLabel;

    public MainForm()
    {
        // Enable Windows GDI+ support for PDFsharp
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Form settings
        Text = "Image to PDF Converter";
        Size = new Size(800, 600);
        MinimumSize = new Size(600, 400);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;

        // Create controls
        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            ColumnCount = 3,
            RowCount = 4
        };

        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        
        // Preview section
        _previewLabel = new Label
        {
            Text = "Image Preview",
            Font = new Font(Font.FontFamily, 10, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Fill
        };

        _previewBox = new PictureBox
        {
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.WhiteSmoke
        };

        // Buttons
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        _selectButton = CreateStyledButton("Select Image");
        _selectButton.Click += SelectButton_Click;

        _pasteButton = CreateStyledButton("Paste from Clipboard");
        _pasteButton.Click += PasteButton_Click;

        buttonPanel.Controls.AddRange(new Control[] { _selectButton, _pasteButton });

        // Output path section
        var outputPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1
        };
        outputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
        outputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        outputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

        var outputLabel = new Label
        {
            Text = "Output Path:",
            TextAlign = ContentAlignment.MiddleRight,
            Dock = DockStyle.Fill
        };

        _outputPathBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(5)
        };

        _browseOutputButton = CreateStyledButton("Browse");
        _browseOutputButton.Click += BrowseOutputButton_Click;

        outputPanel.Controls.Add(outputLabel, 0, 0);
        outputPanel.Controls.Add(_outputPathBox, 1, 0);
        outputPanel.Controls.Add(_browseOutputButton, 2, 0);

        // Convert button
        _convertButton = new Button
        {
            Text = "Convert to PDF",
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(22, 122, 28),  // Darker green for better contrast
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = 
            { 
                BorderSize = 1,
                BorderColor = Color.FromArgb(16, 124, 16),
                MouseOverBackColor = Color.FromArgb(28, 145, 35),
                MouseDownBackColor = Color.FromArgb(15, 100, 20)
            },
            Padding = new Padding(15, 8, 15, 8),
            Cursor = Cursors.Hand,
            Enabled = false
        };
        _convertButton.Click += ConvertButton_Click;

        // Status label
        _statusLabel = new Label
        {
            Text = "Ready",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.Gray
        };

        // Add controls to main panel
        mainPanel.Controls.Add(_previewLabel, 0, 0);
        mainPanel.SetColumnSpan(_previewLabel, 3);

        mainPanel.Controls.Add(_previewBox, 0, 1);
        mainPanel.SetColumnSpan(_previewBox, 3);

        mainPanel.Controls.Add(buttonPanel, 0, 2);
        mainPanel.SetColumnSpan(buttonPanel, 3);

        mainPanel.Controls.Add(outputPanel, 0, 3);
        mainPanel.SetColumnSpan(outputPanel, 2);

        mainPanel.Controls.Add(_convertButton, 2, 3);

        // Add main panel to form
        Controls.Add(mainPanel);

        // Add status label at the bottom
        var statusStrip = new StatusStrip();
        statusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });
        statusStrip.Items.Add(new ToolStripStatusLabel { Text = "Ready" });
        Controls.Add(statusStrip);
    }

    private Button CreateStyledButton(string text)
    {
        var button = new Button
        {
            Text = text,
            Padding = new Padding(15, 8, 15, 8),  // Increased padding
            BackColor = Color.FromArgb(14, 99, 156),  // Darker blue for better contrast
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),  // Modern font
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = 
            { 
                BorderSize = 1,  // Thin border
                BorderColor = Color.FromArgb(0, 120, 212),  // Light blue border
                MouseOverBackColor = Color.FromArgb(0, 140, 245),  // Lighter blue on hover
                MouseDownBackColor = Color.FromArgb(10, 80, 120)  // Darker blue when clicked
            },
            Margin = new Padding(5),
            Cursor = Cursors.Hand,
            AutoSize = true,  // Allow button to size based on text
            MinimumSize = new Size(100, 35)  // Minimum size for consistency
        };

        // Add hover effect handlers
        button.MouseEnter += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(0, 160, 255);
        button.MouseLeave += (s, e) => button.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 212);

        return button;
    }

    private void SelectButton_Click(object sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All files|*.*",
            Title = "Select an Image File"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            LoadImage(openFileDialog.FileName);
        }
    }

    private void PasteButton_Click(object sender, EventArgs e)
    {
        if (!Clipboard.ContainsImage())
        {
            MessageBox.Show("No image found in clipboard!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var image = Clipboard.GetImage();
            if (image == null) return;

            var tempPath = Path.Combine(Path.GetTempPath(), $"clipboard_image_{Guid.NewGuid()}.png");
            image.Save(tempPath, ImageFormat.Png);
            LoadImage(tempPath, true);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing clipboard image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BrowseOutputButton_Click(object sender, EventArgs e)
    {
        using var saveFileDialog = new SaveFileDialog
        {
            Filter = "PDF files|*.pdf",
            Title = "Save PDF As",
            DefaultExt = "pdf"
        };

        if (_selectedImagePath != null)
        {
            saveFileDialog.FileName = Path.ChangeExtension(Path.GetFileName(_selectedImagePath), ".pdf");
        }

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            _outputPathBox.Text = saveFileDialog.FileName;
        }
    }

    private void LoadImage(string path, bool isTemporary = false)
    {
        try
        {
            _selectedImagePath = path;
            _previewBox.Image?.Dispose();
            _previewBox.Image = DrawingImage.FromFile(path);
            _previewLabel.Text = $"Image Preview - {Path.GetFileName(path)}";
            _convertButton.Enabled = true;

            if (string.IsNullOrEmpty(_outputPathBox.Text))
            {
                _outputPathBox.Text = Path.ChangeExtension(path, ".pdf");
            }

            if (isTemporary)
            {
                _statusLabel.Text = "Temporary image loaded from clipboard";
            }
            else
            {
                _statusLabel.Text = "Image loaded successfully";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void ConvertButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedImagePath) || !File.Exists(_selectedImagePath))
        {
            MessageBox.Show("Please select an image first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrEmpty(_outputPathBox.Text))
        {
            MessageBox.Show("Please specify the output PDF path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _convertButton.Enabled = false;
            _statusLabel.Text = "Converting...";
            Cursor = Cursors.WaitCursor;

            await Task.Run(() => ConvertImageToPdf(_selectedImagePath, _outputPathBox.Text));

            MessageBox.Show("PDF created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _statusLabel.Text = "Conversion completed successfully";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error converting to PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _statusLabel.Text = "Conversion failed";
        }
        finally
        {
            _convertButton.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void ConvertImageToPdf(string imagePath, string pdfPath)
    {
        try
        {
            // Load the image
            using var sourceImage = DrawingImage.FromFile(imagePath);

            // Create new PDF document
            using var document = new PdfDocument();
            
            // Add a new page
            var page = document.AddPage();

            // Set page size to match image size
            page.Width = XUnit.FromPoint(sourceImage.Width);
            page.Height = XUnit.FromPoint(sourceImage.Height);

            // Create PDF graphics for drawing
            using var gfx = XGraphics.FromPdfPage(page);

            // Load and draw the image
            using var xImage = XImage.FromFile(imagePath);
            gfx.DrawImage(xImage, 0, 0, page.Width.Point, page.Height.Point);

            // Save the PDF
            document.Save(pdfPath);
        }
        catch (IOException ex)
        {
            throw new Exception($"Error accessing files: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating PDF: {ex.Message}", ex);
        }
    }
} 