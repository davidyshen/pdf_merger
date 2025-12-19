# Project Structure & File Reference

## Overview

PDF Merger is a complete Windows PDF merging application built with C# and WinUI3. This document describes the project structure and all created files.

## Directory Structure

```
pdf_merger/
├── Views/
│   ├── MainWindow.xaml              # Main UI definition
│   └── MainWindow.xaml.cs           # UI code-behind and event handlers
├── ViewModels/
│   └── MainWindowViewModel.cs       # MVVM data models
├── Services/
│   ├── PDFService.cs                # PDF operations (merge, thumbnail, validation)
│   └── ContextMenuService.cs        # Windows registry integration
├── App.xaml                         # Application resources
├── App.xaml.cs                      # Application entry point
├── Program.cs                       # Main program entry
├── NativeMethods.cs                 # Windows API declarations
├── PDFMerger.csproj                 # Project configuration
├── PDFMerger.sln                    # Visual Studio solution
├── .gitignore                       # Git configuration
└── Documentation/
    ├── README.md                    # Project overview
    ├── QUICKSTART.md                # Quick start guide
    ├── INSTALLATION.md              # Installation instructions
    ├── DEVELOPMENT.md               # Developer documentation
    ├── SETUP.md                     # This file
    ├── setup-context-menu.bat       # Batch registration script
    ├── setup-context-menu.ps1       # PowerShell registration script
    └── PDFMergerContextMenu.reg     # Registry file for manual registration
```

## File Descriptions

### Core Application Files

#### App.xaml / App.xaml.cs

- **Purpose**: Application-level configuration and startup
- **Contains**: Application resources, window creation
- **Key Methods**: `OnLaunched()` - creates main window

#### Program.cs

- **Purpose**: Program entry point and command-line argument handling
- **Contains**: Main() method, context menu registration logic
- **Supports**:
  - `--register-context-menu` flag
  - `--unregister-context-menu` flag
  - PDF file path arguments from File Explorer

#### NativeMethods.cs

- **Purpose**: P/Invoke declarations for Windows APIs
- **Contains**: Native Windows method signatures for window management

### UI Layer

#### MainWindow.xaml

- **Purpose**: XAML user interface definition
- **Contains**:
  - Header section with title
  - GridView for PDF thumbnails
  - Reorder buttons (up, down, remove)
  - Output filename text box
  - Merge and Cancel buttons

#### MainWindow.xaml.cs

- **Purpose**: Code-behind for main window
- **Key Methods**:
  - `LoadPDFFiles()` - loads PDFs and generates thumbnails
  - `MoveUpButton_Click()` - moves selected PDF up
  - `MoveDownButton_Click()` - moves selected PDF down
  - `RemoveButton_Click()` - removes selected PDF
  - `MergeButton_Click()` - initiates merge operation
  - `CancelButton_Click()` - closes window

### ViewModel Layer

#### MainWindowViewModel.cs

- **Purpose**: MVVM data binding and state management
- **Classes**:
  - `PDFItem`: Represents a single PDF with path, filename, and thumbnail
  - `MainWindowViewModel`: Manages PDF collection and output filename

### Service Layer

#### PDFService.cs

- **Purpose**: Core PDF operations
- **Key Methods**:
  - `GenerateThumbnailAsync()` - creates 200x280px thumbnail from first page
  - `MergePDFsAsync()` - combines multiple PDFs using iText7
  - `IsValidPDF()` - validates PDF file integrity
  - `GenerateDefaultFileName()` - creates timestamped filename

#### ContextMenuService.cs

- **Purpose**: Windows registry integration for File Explorer context menu
- **Key Methods**:
  - `RegisterContextMenu()` - adds registry entries for context menu
  - `UnregisterContextMenu()` - removes registry entries
  - `IsContextMenuRegistered()` - checks registration status

### Project Configuration

#### PDFMerger.csproj

- **Purpose**: Project file with build configuration
- **Includes**:
  - Target framework: .NET 8.0
  - Platform targets: x86, x64, ARM64
  - NuGet dependencies: WindowsAppSDK, itext7, System.Drawing.Common

#### PDFMerger.sln

- **Purpose**: Visual Studio solution file
- **Contains**: Project references and build configurations

#### .gitignore

- **Purpose**: Git ignore patterns
- **Excludes**: Build artifacts, VS cache, NuGet files

### Documentation Files

#### README.md

- Overview of application features and capabilities
- System requirements and installation overview
- Project structure and architecture explanation
- Usage instructions and future enhancements

#### QUICKSTART.md

- Quick installation steps
- Basic usage instructions
- Common tasks
- Troubleshooting guide
- Tips and tricks

#### INSTALLATION.md

- Detailed installation instructions (4 methods)
- Verification steps
- Troubleshooting guide
- Uninstallation instructions
- Advanced configuration options

#### DEVELOPMENT.md

- Developer-focused documentation
- Architecture overview
- Implementation details with code examples
- Extension points for customization
- Testing strategies
- Debugging tips
- Distribution and signing

### Setup Scripts

#### setup-context-menu.bat

- Batch script for Windows command prompt
- Register/unregister context menu
- Check registration status
- Requires administrator privileges

#### setup-context-menu.ps1

- PowerShell script for modern Windows
- Register/unregister context menu
- Check registration status
- Requires administrator privileges

#### PDFMergerContextMenu.reg

- Registry file for manual context menu setup
- Can be imported directly into registry
- Requires manual path configuration

## Key Technologies & Libraries

### Framework

- **Microsoft.WindowsAppSDK**: WinUI 3 for modern Windows UI
- **.NET 8.0**: Latest .NET runtime

### Libraries

- **iText7**: Professional PDF manipulation library
  - Used for: merging PDFs, reading PDF structure
  - License: AGPL with commercial options

- **System.Drawing.Common**: Image processing
  - Used for: thumbnail generation and rendering

### Windows APIs

- **Windows Registry**: For context menu integration
- **P/Invoke**: For native Windows interoperability

## Build Targets

The project supports multiple architectures:

- **x86**: 32-bit Intel/AMD processors
- **x64**: 64-bit Intel/AMD processors
- **ARM64**: ARM 64-bit processors (Windows 11 on ARM)

## Dependencies & NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.WindowsAppSDK | 1.5.0 | WinUI 3 framework |
| Microsoft.Windows.SDK.BuildTools | 10.0.22621.756 | Windows SDK tools |
| itext7 | 8.0.0 | PDF manipulation |
| System.Drawing.Common | 8.0.0 | Image processing |

## Deployment Artifacts

After building, the following files are generated:

```
bin/Release/net8.0-windows10.0.22621.0/
├── PDFMerger.exe           # Main application
├── PDFMerger.dll           # Application library
├── itext.*.dll             # iText7 libraries
├── System.*.dll            # .NET runtime dependencies
└── [other .dll files]      # Runtime dependencies
```

## Configuration Points

### Thumbnail Settings

- **Dimensions**: 200x280 pixels (customizable in PDFService.cs)
- **Format**: PNG
- **Fallback**: Gray placeholder with filename if generation fails

### Output Filename

- **Default Format**: `merged_pdf_YYYY-MM-DD_HH-mm-ss.pdf`
- **Custom Format**: Any string + .pdf extension
- **Location**: Same directory as first selected PDF

### Registry Paths

- **Registry Key**: `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`
- **Scope**: Per-user (not system-wide)
- **Fallback**: Gracefully handles missing registry entries

## Future Enhancement Areas

1. **Drag & Drop**: Implement drag-and-drop reordering
2. **Batch Processing**: Process multiple merge operations
3. **Page Selection**: Merge specific pages from each PDF
4. **Compression Options**: Reduce output file size
5. **Progress Bar**: Show merge progress for large files
6. **Metadata Editing**: Edit PDF properties
7. **Auto-save Settings**: Remember last used options
8. **PDF Preview**: Full page preview instead of basic thumbnail
9. **Undo/Redo**: Revert operations
10. **Installer Package**: MSI or MSIX distribution

## Testing Considerations

### Unit Test Areas

- PDF validation logic
- Filename generation
- Registry operations

### Integration Test Areas

- End-to-end merge operations
- Context menu triggering
- File I/O operations

### Manual Test Cases

- Merge 2-10 PDFs
- Custom output naming
- Large file handling
- Corrupted PDF handling
- Registry permission scenarios

## Performance Notes

- **Thumbnail Generation**: Async to prevent UI freezing
- **Large Files**: Memory usage scales with PDF size
- **Merge Operation**: Sequential processing maintains order
- **UI Responsiveness**: Async/await pattern maintains responsiveness

## Security Considerations

- All processing done locally (no cloud transmission)
- Original files never modified
- Output file created with appropriate permissions
- Registry operations respect user context
- No elevated privileges required for normal operation

---

**Generated**: December 2025
**Version**: 1.0.0
**Status**: Complete implementation ready for build and deployment
