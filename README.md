# PDF Merger for Windows

A simple Windows application for merging PDF files with an native-looking graphical interface and native File Explorer integration, similar to the built-in merge PDF tool on macOS.

Warning: This code is entirely AI generated and may contain errors because I don't know how to write C#. Use at your own risk (or better yet, if you like the app, consider adding some human-written code yourself!).

## Table of Contents

- [Features](#features)
- [System Requirements](#system-requirements)
- [Installation](#installation)
  - [WiX Installer (Recommended)](#wix-installer-recommended)
  - [Portable Version](#portable-version)
  - [Build from Source](#build-from-source)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Architecture](#architecture)
- [Development](#development)
- [Troubleshooting](#troubleshooting)
- [Uninstallation](#uninstallation)

## Features

- **File Explorer Integration**: Right-click context menu to merge PDFs directly from File Explorer
- **Thumbnail Preview**: Visual thumbnails of the first page of each PDF
- **Reordering**: Use up/down buttons to reorder PDFs before merging
- **Custom Output Names**: Specify custom output filename or use automatic naming
- **Safe Merging**: Preserves original files, creates a new merged PDF
- **Automatic Naming**: Uses `merged_pdf_DATETIME.pdf` format if no name provided
- **Self-Contained**: No additional runtimes required for the installer version
- **ARM64 Support**: Optimized for modern Windows devices

## System Requirements

| Component | Requirement |
|-----------|-------------|
| **OS** | Windows 10 (Build 22621) or Windows 11 |
| **.NET Runtime** | 8.0 or later (for portable versions) |
| **Disk Space** | ~200 MB for installation |
| **RAM** | 2 GB minimum, 4 GB recommended |
| **Architecture** | x86, x64, or ARM64 |

## Installation

### WiX Installer (Recommended)

The WiX installer is the easiest way to install PDF Merger. It handles all setup automatically without requiring certificates.

#### Download and Install

1. Download `PDFMerger.msi` from the releases page
2. Double-click the `.msi` file
3. Follow the on-screen installer prompts
4. The app will be installed to `C:\Program Files\PDFMerger\`
5. A "Merge PDFs" context menu option will be automatically registered

#### Build the Installer Yourself

If you prefer to build the installer from source:

1. **Install WiX Toolset** (one-time setup):

   ```powershell
   dotnet tool install --global wix
   wix extension add WixToolset.UI.wixext
   ```

2. **Publish the Application**:

   ```powershell
   dotnet publish -c Release -r win-arm64 --self-contained true -p:PublishReadyToRun=true
   ```

3. **Build the MSI**:

   ```powershell
   wix build .\installer.wxs -o PDFMerger.msi -ext WixToolset.UI.wixext
   ```

4. The `PDFMerger.msi` file is now ready to share and install on any Windows machine.

### Portable Version

For a standalone folder without installer:

1. **Publish the Application**:

   ```powershell
   dotnet publish -c Release -r win-arm64 --self-contained true -p:PublishReadyToRun=true
   ```

2. Navigate to: `bin\ARM64\Release\net8.0-windows10.0.22621.0\win-arm64\publish\`

3. **First Run**: Simply run `PDFMerger.exe` once. The app will automatically register the "Merge PDFs" context menu.

4. **Optional**: Manually register/unregister using:

   ```powershell
   # Register context menu
   .\setup-context-menu.ps1 -Action register

   # Unregister context menu
   .\setup-context-menu.ps1 -Action unregister
   ```

### Build from Source

#### Using Visual Studio 2022

1. Clone the repository
2. Open `PDFMerger.sln` in Visual Studio 2022
3. Press Ctrl+Shift+B to build
4. Press F5 to run and test

#### Using VS Code (Recommended)

1. Clone the repository
2. Open folder in VS Code: `code .`
3. Run in terminal:

   ```powershell
   dotnet build PDFMerger.csproj
   dotnet run --project PDFMerger.csproj
   ```

## Usage

### Via File Explorer

1. Select one or more PDF files in File Explorer
2. Right-click and select "Merge PDFs"
3. The PDF Merger window opens with all selected files
4. **Reorder** (optional): Select a file and use "↑ Move Up" or "↓ Move Down" buttons
5. **Remove** (optional): Select a file and click "✕ Remove" to exclude it
6. **Output Name** (optional): Enter a custom filename or leave blank for automatic naming
7. Click "Merge PDFs" to combine all files
8. The merged PDF is saved in the same directory as the first selected file

### Direct Launch

Double-click `PDFMerger.exe` to open an empty window where you can:

- Add PDFs via drag-and-drop or file selection
- Preview thumbnails
- Reorder files before merging

### Command Line Usage

```powershell
PDFMerger.exe "C:\path\to\file1.pdf"
PDFMerger.exe "C:\path\to\file1.pdf" "C:\path\to\file2.pdf"
```

## Project Structure

```
pdf_merger/
├── Views/
│   ├── MainWindow.xaml          # Main UI layout
│   └── MainWindow.xaml.cs       # UI code-behind
├── ViewModels/
│   └── MainWindowViewModel.cs   # Data binding and state management
├── Services/
│   ├── PDFService.cs            # PDF merging and thumbnail generation
│   ├── ShellService.cs          # Context menu registration
│   └── WindowsInterop.cs        # Windows native API interop
├── App.xaml                      # Application resources
├── App.xaml.cs                   # Application entry point
├── Program.cs                    # Single-instance pipe server and main entry point
├── app.manifest                  # Windows app manifest
├── installer.wxs                 # WiX installer source
└── PDFMerger.csproj             # Project configuration
```

## Architecture

### Components

**MainWindow (XAML/C#)**

- **Thumbnail Grid**: ListView displaying 80x112px PDF preview images
- **Mica Backdrop**: Modern Windows 11 material design (gracefully falls back on older Windows 10)
- **Reordering**: Up/Down buttons trigger `MoveUp()` and `MoveDown()` in ViewModel
- **File Management**: Remove button excludes individual PDFs before merging
- **Output Naming**: TextBox for custom filename, defaults to `merged_pdf_DATETIME.pdf` if blank
- **Folder Picker**: Browse button allows changing output directory via WinRT FolderPicker

**PDFService**

- **Merging**: Uses iText7's `PdfMerger` class to combine multiple PDFs with SmartMode optimization
- **Thumbnails**: Uses Windows.Data.Pdf (WinRT API) to render the first page at specified width (default 200px)
- **FileStream Handling**: Opens PDFs via FileStream instead of StorageFile to avoid permission issues in unpackaged apps
- **Async Operations**: Thumbnail generation runs asynchronously to prevent UI blocking
- **Error Handling**: Gracefully handles corrupted PDFs and missing files with debug output

**ShellService**

- **Registry-Based**: Manages context menu via `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs` (user-level, no admin needed)
- **Packaged Detection**: `IsPackaged()` checks for MSIX/packaged installations (which skip registration as manifest handles it)
- **Self-Registration**: `RegisterContextMenu()` automatically called at startup in `Program.cs` for portable installations
- **Dynamic Path**: Registers current executable path, allowing app to work from any location
- **Methods**: `RegisterContextMenu()`, `UnregisterContextMenu()`, `IsRegistered()` for complete control

**WindowsInterop**

- **Dispatcher Queue**: `EnsureWindowsSystemDispatcherQueueServer()` manages WinUI dispatcher on UI thread
- **Window Management**: Helper methods for window foreground, finding, and process ID operations
- **Consolidated P/Invoke**: Combines native API declarations for cleaner codebase

**Program.cs**

- **Single-Instance Pattern**: Uses `AppInstance.FindOrRegisterForKey()` to ensure only one instance runs
- **Named Pipe IPC**: Implements `NamedPipeServerStream("PDFMergerSingleInstancePipe")` for inter-process communication
- **Path Forwarding**: Secondary launches send file paths to running instance via Named Pipe, then exit
- **Buffer System**: Maintains `_pathBuffer` to queue paths that arrive before UI is ready
- **Async Pipe Server**: Runs in background Task while main dispatcher initializes

### Data Flow

1. File Explorer context menu or direct launch triggers `PDFMerger.exe`
2. Single-instance check: If already running, sends file paths to main instance via Named Pipe
3. MainWindow loads PDF files and generates thumbnails
4. User reorders files (optional) and enters output filename (optional)
5. On merge: PDFService combines files using iText7
6. Merged PDF is saved to the source directory
7. Context menu is automatically registered on first run (portable version)

## Development

### Prerequisites

- Visual Studio 2022 or VS Code
- .NET 8.0 SDK
- Git

### Building

```powershell
# Restore dependencies
dotnet restore PDFMerger.sln

# Build for development
dotnet build PDFMerger.csproj

# Build for release (ARM64)
dotnet build PDFMerger.csproj -c Release -r win-arm64

# Run
dotnet run --project PDFMerger.csproj
```

### Key Libraries

- **Microsoft.WindowsAppSDK 1.5**: WinUI 3 framework for modern Windows UI
- **itext7 8.0.0**: PDF manipulation and merging
- **itext7.bouncy-castle-adapter 8.0.0**: Cryptographic support for iText7
- **System.IO.Pipes**: Named Pipe communication for single-instance support

### Architecture Decisions

**WinUI 3 (Unpackaged)**

- Modern, responsive UI framework
- Better control over installation and distribution
- Avoids Microsoft Store complexity

**iText7**

- Mature, reliable PDF handling
- Supports all PDF versions and features
- Open-source friendly license option

**Windows.Data.Pdf for Thumbnails**

- Native WinRT API (built-in to Windows)
- Better performance than iText7 rendering
- Proper DPI scaling support
- Graceful handling of problematic PDFs

**Named Pipes for Single-Instance**

- Lightweight IPC mechanism
- Works across user sessions
- Allows passing file paths to running instance

**Registry-Based Context Menu**

- No administrator privileges required (uses HKCU)
- Easy to register/unregister
- Survives app reinstallation
- Works for unpackaged and portable installations

**WiX for Distribution**

- No certificate requirements for installation
- Standard Windows Installer (.msi) format
- Automatic context menu registration during setup
- Professional deployment experience

### Implementation Details

**PDF Thumbnail Generation**

- Uses WinRT `Windows.Data.Pdf.PdfDocument` for native Windows rendering
- Creates `InMemoryRandomAccessStream` to avoid file permission issues
- Returns `byte[]` buffer for easy conversion to XAML BitmapImage
- Handles missing/corrupted PDFs gracefully with null return value

**PDF Merging**

- Uses iText7's `PdfMerger` class with `SmartMode(true)` for optimized output
- Each source PDF opened in separate FileStream with shared read access
- Merges entire page range (1 to total) to preserve all content
- Creates output directory if it doesn't exist
- Properly disposes readers/writers to prevent file locks

**Context Menu Registration**

- Command format: `"%exePath%" "%1"` - Windows passes first selected file as `%1`
- Only registers for unpackaged installations (WiX installer/MSIX handles manifest)
- Uses HKCU (current user) instead of HKLM to avoid admin requirements
- Silent failure on registration errors (logged to debug output only)

**Single-Instance Communication**

- App key: "PDFMergerSingleInstanceKey" - unique identifier across Windows
- Pipe name: "PDFMergerSingleInstancePipe" - fixed for inter-process communication
- Path buffering prevents loss of file paths while UI initializes
- Async pipe server allows main thread to initialize dispatcher queue

**WinUI 3 Configuration**

- Uses `DisableXamlGeneratedMain` - custom Program.cs Main() instead of auto-generated
- `[STAThread]` attribute required for COM interop (Windows APIs)
- Dispatcher queue needed for async operations on UI thread
- FrameworkElement.ActualThemeChanged monitors theme for Mica backdrop updates

## Troubleshooting

### "Merge PDFs" context menu not appearing

**Check 1: Verify Registry**

1. Press `Win + R` and type `regedit`
2. Navigate to: `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`
3. Verify the entry exists with the correct path to `PDFMerger.exe`

**Check 2: Reinstall from Portable**

- Run `PDFMerger.exe` once to trigger automatic registration

**Check 3: Manual Re-registration**

```powershell
.\setup-context-menu.ps1 -Action register
```

### Application won't start

**Check 1: .NET Runtime**

```powershell
dotnet --version
```

Ensure version is 8.0 or later. Download from: <https://dotnet.microsoft.com/download>

**Check 2: File Permissions**

- Right-click `PDFMerger.exe` → Properties
- Ensure "Read" and "Execute" permissions are granted

**Check 3: Antivirus**

- Some antivirus software may block unsigned executables
- Whitelist the application or disable scanning temporarily

### PDFs not merging

**Check 1: File Format**

- Ensure all files are valid PDF files
- Try opening them in Adobe Reader first

**Check 2: File Permissions**

- Ensure PDFs are not read-only
- Ensure you have write permission to the directory

**Check 3: Output Location**

- Output is saved to the same directory as the first file
- Ensure you have write permissions there

## Uninstallation

### From Windows (MSI Installation)

1. Open **Settings** → **Apps** → **Installed apps**
2. Search for "PDF Merger"
3. Click the three dots → **Uninstall**
4. Confirm the uninstall
5. The context menu will be automatically removed

### Manual Context Menu Removal

```powershell
# Via PowerShell
.\setup-context-menu.ps1 -Action unregister

# Or manually delete the registry key:
# HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs
```

### Manual File Removal

1. Delete the installation folder (default: `C:\Program Files\PDFMerger\`)
2. Manually remove the context menu registry entry (see above)

## Future Enhancements

- Drag-and-drop file reordering within the UI
- Batch processing from command line
- PDF page selection (merge specific pages only)
- Compression options
- Metadata editing and preservation
- Auto-save recent merge settings
- Dark mode support
- Multi-language support

## License

PDF Merger is released under the MIT License. See LICENSE file for details.

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Follow the existing code style
4. Submit a pull request with a clear description of changes

## Security

- **Local Processing**: All PDFs are processed locally on your computer
- **No Internet**: The application does not send any data over the network
- **Original Files**: Source PDFs are never modified; only a new merged file is created
- **Code Transparency**: Source code is available for security review

## Support

For issues, questions, or feature requests:

- Check existing documentation in this repository
- Review the troubleshooting section above
- Open an issue on the GitHub repository
- Provide details about your environment and steps to reproduce

## Changelog

### Version 1.0.0 (Current)

- Initial release
- PDF merging with custom output names
- Thumbnail preview generation
- File reordering UI
- File Explorer context menu integration
- Single-instance application with Named Pipes
- WiX-based installer (no certificate required)
- Portable version with self-registration
- ARM64 support

---

## Quick Reference

### Common Commands

```powershell
# Build for development
dotnet build PDFMerger.csproj

# Run the application
dotnet run --project PDFMerger.csproj

# Publish for distribution
dotnet publish -c Release -r win-arm64 --self-contained true -p:PublishReadyToRun=true

# Build the MSI installer
dotnet publish -c Release -r win-arm64 --self-contained true -p:PublishReadyToRun=true
wix build .\installer.wxs -o PDFMerger.msi -ext WixToolset.UI.wixext
```

### Registry Locations

- **Context Menu**: `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`
- **Application Settings**: `HKEY_CURRENT_USER\Software\Developer\PDFMerger`

### File Locations

- **Installation**: `C:\Program Files\PDFMerger\`
- **Portable**: `bin\ARM64\Release\net8.0-windows10.0.22621.0\win-arm64\publish\`
- **Build Output**: `bin\ARM64\{Debug|Release}\`

---

## Testing

### Manual Testing Checklist

- [ ] Context menu appears for PDF files
- [ ] Single PDF selection opens merger window
- [ ] Multiple PDF selection loads all files with thumbnails
- [ ] File reordering works correctly (up/down buttons)
- [ ] File removal works (remove button)
- [ ] Merge produces valid PDF with all pages in correct order
- [ ] Output filename generation works
- [ ] Custom output filename is respected
- [ ] Second instance redirection works (launching while app is open)
- [ ] Uninstallation cleans up context menu

### PDF Test Cases

1. **Basic Merge**: 2-3 simple PDFs → Merged PDF contains all pages
2. **Large Files**: PDFs >50MB → Handles without memory issues
3. **Mixed Content**: PDFs with images, text, forms → Content preserved
4. **Edge Cases**:
   - Empty PDFs (0 pages)
   - Single-page PDFs
   - Multi-page PDFs (100+ pages)
   - PDFs with special characters in filename

---

## Known Issues & Limitations

- WiX installer requires UI extension installation (one-time setup)
- SmartScreen may warn on first download of MSI (normal for unsigned installers)
- Context menu registration requires write access to user registry
- PDF rendering uses WinRT API which may have limitations on older Windows 10 builds

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | 2025-12-21 | Initial stable release with WiX installer |
| Beta | 2025-12-20 | MSIX testing and consolidation |
| Alpha | 2025-12-19 | Core functionality and PDF service implementation |

## Support

For issues or feature requests, please open an issue on the repository.
