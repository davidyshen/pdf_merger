# PDF Merger - Quick Start Guide

## What is PDF Merger?

PDF Merger is a lightweight Windows application that allows you to combine multiple PDF files into a single document. It integrates directly into Windows File Explorer for easy access.

## Key Features

✓ **File Explorer Integration** - Right-click context menu  
✓ **Visual Thumbnails** - Preview first page of each PDF  
✓ **Drag & Drop Ready** - Reorder PDFs before merging  
✓ **Smart Naming** - Auto-generate or custom filenames  
✓ **Safe Merging** - Original files always preserved  

## Building the Application

### Option 1: Visual Studio 2022 (Easiest)

```
1. Open PDFMerger.sln
2. Press Ctrl+Shift+B to build
3. Press F5 to run
```

### Option 2: VS Code (Recommended - Lightweight)

```powershell
# Open in VS Code
code "C:\Users\dyshe\Git projects\pdf_merger"

# In terminal, build
dotnet build PDFMerger.csproj

# Run
dotnet run --project PDFMerger.csproj
```

See [VSCODE_SETUP.md](VSCODE_SETUP.md) for complete VS Code guide.

### Option 3: Command Line Only

```powershell
cd "C:\Users\dyshe\Git projects\pdf_merger"
dotnet build PDFMerger.csproj
dotnet run --project PDFMerger.csproj
```

## Installation (Quick Steps)

1. **Build or download** `PDFMerger.exe`
2. **Place** in `C:\Program Files\PDFMerger\` (or custom location)
3. **Register context menu** (run as Administrator):

   ```batch
   PDFMerger.exe --register-context-menu
   ```

## Basic Usage

### Method 1: File Explorer (Recommended)

1. Open File Explorer
2. Select 2 or more PDF files
3. Right-click → **Merge PDFs**
4. Reorder files if needed using buttons
5. Enter output filename (optional)
6. Click **Merge PDFs**

### Method 2: Command Line

```batch
PDFMerger.exe "file1.pdf" "file2.pdf" "file3.pdf"
```

### Method 3: Open Application

Double-click `PDFMerger.exe` to open an empty window and add PDFs manually.

## Interface Overview

```
┌─────────────────────────────────────────────┐
│  PDF Merger                                 │
│  Select PDFs to merge, reorder them...      │
├─────────────────────────────────────────────┤
│                                             │
│  [Thumbnail] [Thumbnail] [Thumbnail]  ↑    │
│  file1.pdf   file2.pdf   file3.pdf   ↓    │
│                                       ✕    │
│                                             │
├─────────────────────────────────────────────┤
│  Output Filename (optional):                │
│  [________________________]                 │
│                   [Merge] [Cancel]         │
└─────────────────────────────────────────────┘
```

## Common Tasks

### Reorder PDFs

1. Click a PDF thumbnail to select it
2. Click **↑ Move Up** to move it earlier
3. Click **↓ Move Down** to move it later

### Set Output Filename

1. In the "Output Filename" field, enter desired name
2. Don't include `.pdf` extension (added automatically)
3. If left blank, uses: `merged_pdf_YYYY-MM-DD_HH-mm-ss.pdf`

### Merge PDFs

1. Arrange PDFs in desired order
2. (Optional) Enter output filename
3. Click **Merge PDFs**
4. Merged PDF appears in same directory as first file

### Cancel Operation

Click **Cancel** to close the window without merging.

## Output Location

The merged PDF is always saved in the **same directory as the first selected PDF file**.

Example:

- Input files: `C:\Documents\report1.pdf`, `C:\Documents\report2.pdf`
- Output: `C:\Documents\merged_pdf_2025-01-15_10-30-45.pdf`

## Troubleshooting

| Problem | Solution |
|---------|----------|
| "Merge PDFs" not in context menu | Run `PDFMerger.exe --register-context-menu` as Administrator |
| Application won't open | Ensure .NET 8.0 runtime is installed |
| Merge fails | Verify PDFs aren't corrupted; try smaller files first |
| Thumbnails not showing | PDF may be encrypted or unsupported format |

## Uninstall

Remove context menu:

```batch
PDFMerger.exe --unregister-context-menu
```

Delete application folder:

```batch
rmdir /s "C:\Program Files\PDFMerger"
```

## Tips & Tricks

💡 **Tip 1**: Select PDFs in File Explorer in the desired merge order before opening PDF Merger

💡 **Tip 2**: You can remove unwanted PDFs using the **✕ Remove** button before merging

💡 **Tip 3**: For long filenames, the output field shows the current text in the title bar

💡 **Tip 4**: The application works offline - no internet connection required

## System Requirements

- **OS**: Windows 10 (Build 22621) or Windows 11
- **Runtime**: .NET 8.0 or later
- **Disk Space**: ~100 MB
- **RAM**: 2 GB minimum

## Keyboard Shortcuts

- **Alt+F4** - Close application
- **Tab** - Navigate between elements
- **Enter** - Click focused button

## Getting More Help

- See **README.md** for full feature list
- See **INSTALLATION.md** for detailed setup instructions
- See **DEVELOPMENT.md** for technical information

---

**PDF Merger v1.0** - Merge PDFs with ease!
