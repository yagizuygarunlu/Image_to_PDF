# Image to PDF Converter

A modern Windows Forms application that converts images to PDF files. Built with .NET 9 and PdfSharpCore, this application provides a user-friendly interface for converting images to PDF format.

## Features

- 🖼️ Convert image files to PDF
- 📋 Paste images directly from clipboard
- 🔍 Live image preview
- 📐 Automatic image scaling
- 💫 Modern UI with visual feedback
- 🎨 Supports common image formats (JPEG, PNG, GIF, BMP)

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

## Installation

1. Clone the repository:
```bash
git clone https://github.com/yagizuygarunlu/Image_to_PDF.git
```

2. Navigate to the project directory:
```bash
cd Image_to_PDF
```

3. Build the project:
```bash
dotnet build
```

## Usage

1. Run the application:
```bash
dotnet run --project src/Image_to_PDF.csproj
```

2. Choose one of the following options:
   - Click "Select Image" to choose an image file
   - Click "Paste from Clipboard" to use an image from your clipboard

3. Preview your image in the application window

4. (Optional) Click "Browse" to choose a custom output location

5. Click "Convert to PDF" to create your PDF file

## Dependencies

- PdfSharpCore (1.3.62) - PDF creation and manipulation
- System.Drawing.Common (8.0.1) - Image processing
- System.Text.Encoding.CodePages (8.0.0) - Text encoding support

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 