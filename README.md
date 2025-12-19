# PDF Merger for Windows

A powerful Windows application for merging PDF files with an intuitive graphical interface and native File Explorer integration.

## Features

- **File Explorer Integration**: Right-click context menu to merge PDFs directly from File Explorer
- **Thumbnail Preview**: Visual thumbnails of the first page of each PDF
- **Reordering**: Use up/down buttons to reorder PDFs before merging
- **Custom Output Names**: Specify custom output filename or use automatic naming
- **Safe Merging**: Preserves original files, creates a new merged PDF
- **Automatic Naming**: Uses `merged_pdf_DATETIME.pdf` format if no name provided

## System Requirements

- Windows 10 (Build 22621) or later
- .NET 8.0 Runtime or later
- Visual Studio 2022 **OR** VS Code (for development)

## Installation

### Build from Source

**Option 1: Visual Studio 2022**

1. Clone the repository
2. Open `PDFMerger.sln` in Visual Studio 2022
3. Press Ctrl+Shift+B to build
4. Press F5 to run

**Option 2: VS Code** (recommended for lightweight setup)

1. Clone the repository
2. Open folder in VS Code: `code .`
3. Run in terminal: `dotnet build PDFMerger.csproj`
4. Run: `dotnet run --project PDFMerger.csproj`

**Option 3: Command Line Only**

1. Clone the repository
2. Run: `dotnet build PDFMerger.csproj`
3. Run: `dotnet run --project PDFMerger.csproj`

See [VSCODE_SETUP.md](VSCODE_SETUP.md) for complete VS Code instructions.

### Register Context Menu

After installation, register the context menu integration:

```powershell
.\setup-context-menu.ps1 -Action register
```

This will add a "Merge PDFs" option to the File Explorer context menu (under "Show more options" on Windows 11).

### Unregister Context Menu

To remove the context menu integration:

```powershell
.\setup-context-menu.ps1 -Action unregister
```

## Usage

### Via File Explorer

1. Select multiple PDF files in File Explorer
2. Right-click and select "Merge PDFs"
3. The PDF Merger window opens with all selected files
4. Reorder files using the up/down buttons if needed
5. Enter an output filename (optional) or leave blank for automatic naming
6. Click "Merge PDFs" to combine all files
7. The merged PDF is saved in the same directory as the first selected file

### Direct Launch

Double-click the application to open an empty merger window where you can add PDFs manually.

## Project Structure

```
pdf_merger/
├── Views/
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── ViewModels/
│   └── MainWindowViewModel.cs
├── Services/
│   ├── PDFService.cs
│   ├── ContextMenuService.cs
├── App.xaml
├── App.xaml.cs
├── Program.cs
├── NativeMethods.cs
└── PDFMerger.csproj
```

## Dependencies

- **Microsoft.WindowsAppSDK**: WinUI 3 framework for modern Windows UI
- **itext7**: PDF manipulation library
- **System.Drawing.Common**: Thumbnail generation

## Architecture

### Key Components

1. **MainWindow (XAML)**: UI for displaying PDF thumbnails, reordering, and merge controls
2. **PDFService**: Handles PDF operations (merging, thumbnail generation, validation)
3. **ContextMenuService**: Manages Windows registry entries for context menu integration
4. **MainWindowViewModel**: Data binding and state management

### Data Flow

1. Application receives file paths from File Explorer context menu
2. MainWindow loads PDF files and generates thumbnails
3. User reorders files and enters output filename
4. On merge, PDFService combines all files using iText7
5. Output file is saved to the source directory

## Technical Details

### PDF Merging

- Uses iText7 library for PDF manipulation
- Preserves all content, metadata, and formatting
- Supports files up to maximum PDF specification limits

### Thumbnail Generation

- Extracts first page of each PDF
- Renders at 200x280 pixels for consistent display
- Falls back gracefully if preview unavailable

### Context Menu Registration

- Stores registry entries in `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`
- Integrates seamlessly with Windows File Explorer
- Can be toggled on/off via command-line switches

## Future Enhancements

- Drag-and-drop file reordering
- Batch processing from command line
- PDF page selection (merge specific pages only)
- Compression options
- Metadata editing
- Auto-save recent merge settings

## License

MIT License

## Contributing

Contributions are welcome! Please follow the existing code style and structure.

## Support

For issues or feature requests, please open an issue on the repository.
