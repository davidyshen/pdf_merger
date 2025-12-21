# PDF Merger for Windows

A simple Windows application for merging PDF files with a native-looking graphical interface and Windows File Explorer integration.

## Features

- **File Explorer Integration**: Right-click context menu to merge PDFs directly from File Explorer
- **Thumbnail Preview**: Visual thumbnails of the first page of each PDF
- **Reordering**: Use up/down buttons to reorder PDFs before merging
- **Custom Output Names**: Specify custom output filename or use automatic naming with timestamp
- **Safe Merging**: Preserves original files, creates a new merged PDF
- **Portable**: No installation required - extract and run
- **Self-Contained**: All dependencies bundled (no .NET installation needed)
- **ARM64 Support**: Optimized for modern Windows devices

## System Requirements

| Component | Requirement |
|-----------|-------------|
| **OS** | Windows 10 (Build 22621) or Windows 11 |
| **Disk Space** | ~200 MB |
| **RAM** | 2 GB minimum |
| **Architecture** | ARM64, x64, or x86 (portable versions available) |

## Installation & Usage

### Quick Start

1. **Download** `PDFMerger-portable.zip` from [Releases](https://github.com/davidyshen/pdf_merger/releases)
2. **Extract** the ZIP file to any location (Desktop, Documents, etc.)
3. **Run** `PDFMerger.exe`
4. **Context menu registers automatically** on first run

### That's it!

The app will automatically register the "Merge PDFs" option in your File Explorer right-click menu. No installation wizard, no administrator prompts, no dependencies to install.

### Using the Context Menu

1. Open File Explorer
2. Select one or more PDF files
3. Right-click any selected PDF
4. Click "Merge PDFs"
5. The app will open with your selected files ready to merge

## Usage

### Via File Explorer

1. Select one or more PDF files in File Explorer
2. Right-click and select "Merge PDFs"
3. The PDF Merger window opens with your selected files
4. **Reorder** (optional): Select a file and use "↑ Move Up" or "↓ Move Down" buttons
5. **Remove** (optional): Select a file and click "✕ Remove" to exclude it
6. **Output Name** (optional): Enter a custom filename or leave blank for automatic `merged_pdf_[TIMESTAMP].pdf`
7. Click "Merge PDFs" to combine all files
8. The merged PDF is saved in the same directory as the first selected file

### Direct Launch

Double-click `PDFMerger.exe` to open an empty window where you can:
- Add PDFs via drag-and-drop
- Preview thumbnails
- Reorder before merging

### Command Line

```powershell
PDFMerger.exe "C:\path\to\file1.pdf" "C:\path\to\file2.pdf"
```

## Building from Source

### Prerequisites

- .NET 8.0 SDK or later
- Git

### Build Steps

```powershell
# Clone the repository
git clone https://github.com/davidyshen/pdf_merger.git
cd pdf_merger

# Restore dependencies
dotnet restore

# Build (Debug)
dotnet build -c Debug -r win-arm64

# Publish (Release - self-contained)
dotnet publish -c Release -r win-arm64 --self-contained true -p:PublishReadyToRun=true
```

The published app will be in: `bin\ARM64\Release\net8.0-windows10.0.22621.0\win-arm64\publish\`

Just copy that folder anywhere and run `PDFMerger.exe`.

## Project Structure

```
pdf_merger/
├── Views/
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── ViewModels/
│   └── MainWindowViewModel.cs
├── Services/
│   ├── PDFService.cs            # PDF merging & thumbnail generation
│   ├── ShellService.cs          # Context menu registration
│   └── WindowsInterop.cs        # Windows native API interop
├── Program.cs                    # Entry point, single-instance enforcement
├── App.xaml & App.xaml.cs
├── .github/workflows/
│   └── build-msi.yml            # GitHub Actions: Build & Release
└── PDFMerger.csproj
```

## Architecture

### Key Components

**ShellService.cs**
- Auto-registers "Merge PDFs" context menu on startup
- Uses HKCU registry (no admin rights needed)
- Checks if already registered to avoid redundant registration

**PDFService.cs**
- Merges PDFs using iText7
- Generates thumbnails (first page preview)
- Auto-generates timestamp-based filenames

**Program.cs**
- Single-instance enforcement via AppInstance
- IPC with Named Pipes for secondary launches
- Auto-calls ShellService.RegisterContextMenu()

**WindowsInterop.cs**
- Consolidated Windows native API declarations
- Dispatcher queue management for WinUI

### Workflow

1. User right-clicks PDF(s) in File Explorer or launches `PDFMerger.exe`
2. Single-instance check routes to existing instance if already running
3. ShellService auto-registers context menu on first run
4. User selects files, reorders (optional), enters output name (optional)
5. PDFService merges files using iText7
6. Merged PDF saved to source directory

## Development

To build and run locally:

```powershell
# Restore & Build
dotnet restore
dotnet build -c Debug -r win-arm64

# Run
dotnet run --project PDFMerger.csproj --configuration Debug --runtimeIdentifier win-arm64

# Or directly run the published version
.\bin\ARM64\Debug\net8.0-windows10.0.22621.0\win-arm64\PDFMerger.exe
```

## Releases & Deployment

GitHub Actions automatically:
1. Builds Release version on tags (e.g., `v1.0.2`)
2. Creates portable ZIP archive
3. Uploads to GitHub Releases

**To create a release:**

```powershell
git tag v1.0.2
git push origin main --tags
```

GitHub Actions will build and upload `PDFMerger-portable.zip` automatically.

## Troubleshooting

### "Merge PDFs" context menu not appearing

1. **Verify registry** (press `Win + R`, type `regedit`):
   - Navigate to: `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`
   - Should exist with correct path to `PDFMerger.exe`

2. **Re-register**: Launch `PDFMerger.exe` again - it auto-registers on startup

3. **Manually check registry**:
   ```powershell
   reg query HKCU\Software\Classes\*\shell\MergePDFs
   ```

### App won't start

- Ensure Windows 10 Build 22621+ or Windows 11
- Try re-extracting the ZIP file
- Check if antivirus is blocking the executable

### Merge fails

- Ensure PDFs are not corrupted
- Check disk space for output file
- Try merging just 2 PDFs to isolate the issue

## License

MIT License - See LICENSE file for details
