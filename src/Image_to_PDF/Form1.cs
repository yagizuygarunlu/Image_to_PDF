using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Text;
using System.Diagnostics;

namespace Image_to_PDF;

public partial class Form1 : Form
{
    private PictureBox previewBox = null!;
    private Button selectButton = null!;
    private Button pasteButton = null!;
    private Button convertButton = null!;
    private Button browseButton = null!;
    private Button htmlButton = null!;
    private TextBox outputPathBox = null!;
    private TextBox htmlTextBox = null!;
    private Image? currentImage;

    public Form1()
    {
        InitializeComponent();
        InitializeCustomComponents();
        SetupEventHandlers();
    }

    private void InitializeCustomComponents()
    {
        this.Text = "Image/HTML to PDF Converter";
        this.Size = new Size(800, 800);

        previewBox = new PictureBox
        {
            Size = new Size(600, 400),
            Location = new Point(100, 20),
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.FixedSingle
        };

        selectButton = new Button
        {
            Text = "Select Image",
            Size = new Size(120, 30),
            Location = new Point(100, 440)
        };

        pasteButton = new Button
        {
            Text = "Paste from Clipboard",
            Size = new Size(120, 30),
            Location = new Point(230, 440)
        };

        convertButton = new Button
        {
            Text = "Convert to PDF",
            Size = new Size(120, 30),
            Location = new Point(360, 440),
            Enabled = false
        };

        htmlTextBox = new TextBox
        {
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Size = new Size(600, 150),
            Location = new Point(100, 530),
            PlaceholderText = "Enter HTML content here..."
        };

        htmlButton = new Button
        {
            Text = "Convert HTML to PDF",
            Size = new Size(120, 30),
            Location = new Point(100, 690)
        };

        outputPathBox = new TextBox
        {
            Size = new Size(450, 30),
            Location = new Point(100, 480)
        };

        browseButton = new Button
        {
            Text = "Browse",
            Size = new Size(120, 30),
            Location = new Point(580, 480)
        };

        this.Controls.AddRange(new Control[] { 
            previewBox, selectButton, pasteButton, 
            convertButton, outputPathBox, browseButton,
            htmlTextBox, htmlButton
        });
    }

    private void SetupEventHandlers()
    {
        selectButton.Click += SelectButton_Click;
        pasteButton.Click += PasteButton_Click;
        convertButton.Click += ConvertButton_Click;
        browseButton.Click += BrowseButton_Click;
        htmlButton.Click += HtmlButton_Click;
    }

    private void SelectButton_Click(object? sender, EventArgs e)
    {
        using (OpenFileDialog dialog = new OpenFileDialog())
        {
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoadImage(dialog.FileName);
            }
        }
    }

    private void PasteButton_Click(object? sender, EventArgs e)
    {
        if (Clipboard.ContainsImage())
        {
            currentImage = Clipboard.GetImage();
            previewBox.Image = currentImage;
            convertButton.Enabled = true;
        }
        else
        {
            MessageBox.Show("No image in clipboard!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        using (SaveFileDialog dialog = new SaveFileDialog())
        {
            dialog.Filter = "PDF files (*.pdf)|*.pdf";
            dialog.DefaultExt = "pdf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                outputPathBox.Text = dialog.FileName;
            }
        }
    }

    private void LoadImage(string path)
    {
        try
        {
            currentImage = Image.FromFile(path);
            previewBox.Image = currentImage;
            convertButton.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void HtmlButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(htmlTextBox.Text))
        {
            MessageBox.Show("Please enter HTML content!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string outputPath = outputPathBox.Text;
        if (string.IsNullOrEmpty(outputPath))
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF files (*.pdf)|*.pdf";
                dialog.DefaultExt = "pdf";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputPath = dialog.FileName;
                }
                else return;
            }
        }

        try
        {
            // Create a temporary HTML file
            string tempHtmlPath = Path.Combine(Path.GetTempPath(), "temp.html");
            File.WriteAllText(tempHtmlPath, htmlTextBox.Text);

            // Open the HTML file in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = tempHtmlPath,
                UseShellExecute = true
            });

            MessageBox.Show(
                "The HTML content has been opened in your default browser.\n" +
                "Please use the browser's print function (Ctrl+P) to save as PDF.",
                "Next Steps",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ConvertButton_Click(object? sender, EventArgs e)
    {
        if (currentImage == null)
        {
            MessageBox.Show("Please select or paste an image first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        string outputPath = outputPathBox.Text;
        if (string.IsNullOrEmpty(outputPath))
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF files (*.pdf)|*.pdf";
                dialog.DefaultExt = "pdf";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputPath = dialog.FileName;
                }
                else return;
            }
        }

        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var document = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                using (var imageStream = new MemoryStream())
                {
                    currentImage.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                    imageStream.Position = 0;
                    var xImage = XImage.FromStream(() => imageStream);

                    // Calculate scaling to fit the page while maintaining aspect ratio
                    double pageWidth = page.Width.Point;
                    double pageHeight = page.Height.Point;
                    double imageWidth = xImage.PixelWidth;
                    double imageHeight = xImage.PixelHeight;

                    double scale = Math.Min(pageWidth / imageWidth, pageHeight / imageHeight);
                    double width = imageWidth * scale;
                    double height = imageHeight * scale;

                    // Center the image on the page
                    double x = (pageWidth - width) / 2;
                    double y = (pageHeight - height) / 2;

                    gfx.DrawImage(xImage, x, y, width, height);
                }

                document.Save(outputPath);
            }

            MessageBox.Show("PDF created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
