# PDF Merger - Portable Distribution

## Overview

PDF Merger is distributed as a self-contained portable executable that requires no installation or dependencies beyond Windows itself.

## Files

- **PDFMerger-portable-arm64.zip** - For ARM64 Windows (newer Surface devices, Apple Silicon running Windows)
- **PDFMerger-portable-x64.zip** - For x64 Windows (standard Intel/AMD processors)

## Installation

1. Download the ZIP file appropriate for your system architecture
2. Extract the ZIP file to any location
3. Click the `PDFMerger.lnk` shortcut to launch the application

## First Run

On the first run, PDF Merger automatically registers itself with Windows File Explorer:

- Right-click any PDF file or folder of PDFs
- Select "Merge PDFs" from the context menu

## Uninstallation

To completely remove PDF Merger:

1. Run the included `uninstall.ps1` script to remove the Windows context menu entry
2. Delete the extracted folder

**Important:** Always run the uninstall script before deleting the folder to clean up the registry entries.

## Structure

```
PDFMerger-portable-[arch].zip/
├── PDFMerger.lnk              # Shortcut to launch the application
├── uninstall.ps1              # Script to clean up and uninstall
└── [win-arch64|win-x64]/      # Contains the actual executable and all dependencies
    ├── PDFMerger.exe
    ├── PDFMerger.dll
    ├── PDFMerger.deps.json
    ├── PDFMerger.pdb
    ├── PDFMerger.runtimeconfig.json
    └── [all .NET runtime and library DLLs]
```

## Building

To build the portable executable yourself:

```powershell
# Build both architectures
.\build.ps1 -Runtime win-arm64
.\build.ps1 -Runtime win-x64

# Or specify a different configuration
.\build.ps1 -Runtime win-x64 -Configuration Release
```

The generated ZIP files will be named:

- `PDFMerger-portable-arm64.zip`
- `PDFMerger-portable-x64.zip`

## Requirements

- Windows 10 (build 17763) or later
- No additional software installation needed (all runtime files included)

## Troubleshooting

### "Run uninstall.ps1 failed" error

If you get permission errors running the uninstall script:

1. Right-click PowerShell
2. Select "Run as Administrator"
3. Navigate to the extracted folder
4. Run: `Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process`
5. Run: `.\uninstall.ps1`

### Context menu not appearing

The context menu registration happens automatically on first run. If it doesn't appear:

1. Launch the application using the shortcut
2. Close it
3. Right-click a PDF file to verify the context menu is registered
