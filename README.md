# Image to PDF Converter

A .NET 9 console application that converts images to PDF files. The application supports both file selection and clipboard images.

## Features

- Convert image files to PDF
- Convert clipboard images to PDF
- Supports common image formats (JPEG, PNG, GIF, BMP)
- Simple command-line interface
- Automatic image scaling to fit PDF page

## Requirements

- .NET 9.0 SDK or later
- Windows operating system (for Windows Forms functionality)

## Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/Image_to_PDF.git
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
dotnet run
```

2. Choose one of the following options:
   - Option 1: Select an image file using the file dialog
   - Option 2: Convert an image from clipboard (copy an image before selecting this option)

3. Optionally specify an output PDF path, or press Enter to use the default path (same name as the image but with .pdf extension)

## Dependencies

- itext7 (8.0.2)
- System.Drawing.Common (8.0.1)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 