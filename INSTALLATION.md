# PDF Merger Installation Guide

## Prerequisites

Before installing PDF Merger, ensure you have:

- Windows 10 (Build 22621) or Windows 11
- .NET 8.0 Runtime (download from: <https://dotnet.microsoft.com/download>)
- Administrator access for context menu registration

## Installation Methods

### Method 1: Direct Installation from Source

#### Step 1: Build the Application

1. Open **Visual Studio 2022**
2. Open the `PDFMerger.sln` solution file
3. Select **Build** > **Build Solution** (or press Ctrl+Shift+B)
4. The executable will be created in `bin/Release/` or `bin/Debug/` directory

#### Step 2: Copy Application Files

1. Navigate to the build output directory
2. Copy the `PDFMerger.exe` file to your desired installation location, e.g.:

   ```
   C:\Program Files\PDFMerger\
   ```

#### Step 3: Register Context Menu

**Option A: Using the Batch Script**

1. Open Command Prompt as Administrator
2. Navigate to the PDF Merger directory
3. Run:

   ```batch
   setup-context-menu.bat register
   ```

**Option B: Using PowerShell**

1. Open PowerShell as Administrator
2. Navigate to the PDF Merger directory
3. Run:

   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
   .\setup-context-menu.ps1 -Action register
   ```

### Method 2: WiX Installer (.msi) - Recommended

1. Download the `PDFMerger.msi` (if provided) or build it using the `installer.wxs` script with the **WiX Toolset**:

   ```powershell
   wix extension add WixToolset.UI.wixext
   wix build installer.wxs -o PDFMerger.msi -ext WixToolset.UI.wixext
   ```

2. Run the installer.
3. The installer will automatically:
   - Install the app to `C:\Program Files\PDFMerger\`
   - Create a Start menu shortcut.
   - Register the "Merge PDFs" context menu.

### Method 3: Portable / Manual Installation

1. Copy the application folder to your desired location.
2. **Automatic Registration**: Simply run `PDFMerger.exe` once. The app will automatically detect if the context menu is missing and register it for you.
3. Alternatively, you can use the PowerShell script:

   ```powershell
   .\setup-context-menu.ps1 -Action register
   ```

## Verification

To verify the installation was successful:

1. Open File Explorer
2. Select multiple PDF files
3. Right-click and look for "Merge PDFs" option in the context menu (on Windows 11, this may be under "Show more options").
4. Click to launch the merger window.

### Troubleshooting Context Menu

If "Merge PDFs" doesn't appear in the context menu:

#### Check 1: Verify Registry Entry

1. Press `Win + R` and type `regedit`
2. Navigate to:
   `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`
3. Ensure the entry exists with the correct path to `PDFMerger.exe`.

#### Check 2: Run the App Once

The app is designed to self-register on startup. If the menu is missing, simply double-click `PDFMerger.exe` to trigger the registration.

1. Open Command Prompt as Administrator
2. Run the registration command again:

   ```batch
   "C:\Program Files\PDFMerger\PDFMerger.exe" --register-context-menu
   ```

#### Check 3: Verify .NET Runtime

1. Open Command Prompt
2. Run:

   ```batch
   dotnet --version
   ```

3. Ensure version is 8.0 or later

#### Check 4: Check File Permissions

1. Right-click `PDFMerger.exe`
2. Select "Properties"
3. Ensure "Read" and "Execute" permissions are granted

## First Use

### Starting PDF Merger

**From File Explorer:**

1. Select 2 or more PDF files
2. Right-click and select "Merge PDFs"

**From Command Line:**

```batch
PDFMerger.exe "path\to\file1.pdf" "path\to\file2.pdf"
```

**By Double-clicking:**

- Opens an empty window where you can add PDFs manually

### Using the Application

1. **View Files**: Thumbnails of the first page appear in the grid
2. **Reorder**: Select a PDF and click "↑ Move Up" or "↓ Move Down"
3. **Remove**: Select a PDF and click "✕ Remove"
4. **Output Name**: Enter a filename (optional) or leave blank for automatic naming
5. **Merge**: Click "Merge PDFs" to combine files
6. **Output Location**: The merged PDF is saved in the same directory as the first file

## Uninstallation

### Remove Context Menu Integration

**Option A: Using Batch Script**

1. Open Command Prompt as Administrator
2. Navigate to PDF Merger directory
3. Run:

   ```batch
   setup-context-menu.bat unregister
   ```

**Option B: Using PowerShell**

1. Open PowerShell as Administrator
2. Run:

   ```powershell
   .\setup-context-menu.ps1 -Action unregister
   ```

**Option C: Using Application**

1. Open Command Prompt as Administrator
2. Run:

   ```batch
   "C:\Program Files\PDFMerger\PDFMerger.exe" --unregister-context-menu
   ```

**Option D: Manual Registry Deletion**

1. Press `Win + R` and type `regedit`
2. Navigate to:

   ```
   HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs
   ```

3. Right-click and select "Delete"
4. Confirm deletion

### Remove Application Files

1. Delete the PDF Merger installation directory
2. Optionally clear Windows temporary files:

   ```batch
   Disk Cleanup (cleanmgr.exe)
   ```

## System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| OS | Windows 10 Build 22621 | Windows 11 |
| RAM | 2 GB | 4 GB |
| Disk Space | 100 MB | 500 MB |
| .NET Runtime | 8.0 | 8.0+ |
| Processor | x86, x64, ARM64 | x64 |

## Updating PDF Merger

1. Download the latest version
2. Replace the existing `PDFMerger.exe` file
3. Context menu registration is preserved across updates

## Advanced Configuration

### Custom Installation Path

Edit the registry to use a custom installation path:

1. Open `regedit`
2. Navigate to `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs\command`
3. Update the default value to your custom path
4. Save and close

### Silent Installation

For scripted/automated installation:

```batch
REM Copy application
xcopy "PDFMerger.exe" "C:\Program Files\PDFMerger\" /Y

REM Register context menu silently
"C:\Program Files\PDFMerger\PDFMerger.exe" --register-context-menu
```

## Security Considerations

- PDF Merger does not send data over the internet
- All files are processed locally on your computer
- Original PDF files are never modified
- Ensure you trust the source before installation

## Getting Help

- Check the README.md for feature overview
- Review DEVELOPMENT.md for technical details
- Check the troubleshooting section above
- Open an issue on the project repository

## License

PDF Merger is released under the MIT License. See LICENSE file for details.
