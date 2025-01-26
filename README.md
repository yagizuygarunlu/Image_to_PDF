# Image to PDF Converter

A modern Windows Forms application that converts images and HTML content to PDF files. Built with .NET 9 and PdfSharpCore, this application provides a user-friendly interface for converting both images and HTML content to PDF format.

## Features

- 🖼️ Convert image files to PDF
- 📋 Paste images directly from clipboard
- 🔍 Live image preview
- 📐 Automatic image scaling
- 💫 Modern UI with visual feedback
- 🎨 Supports common image formats (JPEG, PNG, GIF, BMP)
- 🌐 Convert HTML content to PDF using browser's print function
- 📱 Responsive layout

## Project Structure

```
Image_to_PDF/
├── src/                    # Source code
│   ├── Program.cs         # Application entry point
│   ├── MainForm.cs        # Main application window
│   └── Image_to_PDF.csproj # Project file
├── LICENSE                # MIT License
└── README.md             # This file
```

## Requirements

- .NET 9.0 SDK or later
- Windows operating system
- Visual Studio 2022 or compatible IDE
- A modern web browser (for HTML to PDF conversion)

## Installation

1. Clone the repository:
```bash
git clone https://github.com/yagizuygarunlu/Image_to_PDF.git
```

2. Navigate to the project directory:
```bash
cd Image_to_PDF/src
```

3. Build the project:
```bash
dotnet build
```

## Usage

1. Run the application:
```bash
dotnet run
```

Or from the root directory:
```bash
dotnet run --project src/Image_to_PDF.csproj
```

2. For Image to PDF conversion:
   - Click "Select Image" to choose an image file
   - Or click "Paste from Clipboard" to use an image from your clipboard
   - Preview your image in the application window
   - (Optional) Click "Browse" to choose a custom output location
   - Click "Convert to PDF" to create your PDF file

3. For HTML to PDF conversion:
   - Enter your HTML content in the text box at the bottom
   - Click "Convert HTML to PDF"
   - The HTML content will open in your default browser
   - Use the browser's print function (Ctrl+P) to save as PDF
   - Select "Save as PDF" in the print dialog

## Dependencies

All dependencies used in this project are free and open-source:

- PdfSharpCore (1.3.62) - PDF creation and manipulation (MIT License)
- System.Drawing.Common (8.0.1) - Image processing (MIT License)
- System.Text.Encoding.CodePages (8.0.0) - Text encoding support (MIT License)
- Microsoft.NETCore.App - .NET Core runtime and base libraries (MIT License)
- Microsoft.WindowsDesktop.App.WindowsForms - Windows Forms UI framework (MIT License)

## Notes

For HTML to PDF conversion, the application uses your system's default browser's built-in PDF export functionality. This approach ensures:
- No dependency on commercial PDF libraries
- High-quality PDF output
- Consistent rendering across different systems
- Support for modern HTML/CSS features

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 