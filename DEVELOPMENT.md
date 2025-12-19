# PDF Merger Development Guide

## Building the Project

### Prerequisites

- Visual Studio 2022 with Windows App SDK workload
- .NET 8.0 SDK or later
- Windows 10 Build 22621 or later

### Steps

1. Open `PDFMerger.sln` in Visual Studio 2022
2. Right-click the solution and select "Restore NuGet Packages"
3. Build the solution (Ctrl+Shift+B)
4. Run with F5 (Debug) or Ctrl+F5 (Release)

## Architecture Overview

### View Layer (XAML)

- **MainWindow.xaml**: Main UI with thumbnail grid, reordering controls, and merge settings
- Uses data binding to connect with ViewModel

### ViewModel Layer (MVVM)

- **MainWindowViewModel**: Manages PDF list and merge operation state
- **PDFItem**: Represents individual PDF in the list with metadata

### Service Layer

- **PDFService**: Core PDF operations
  - `GenerateThumbnailAsync()`: Creates thumbnail from first page
  - `MergePDFsAsync()`: Combines PDFs in specified order
  - `IsValidPDF()`: Validates PDF file integrity
  - `GenerateDefaultFileName()`: Creates timestamped filename

- **ContextMenuService**: Registry management
  - `RegisterContextMenu()`: Adds context menu to File Explorer
  - `UnregisterContextMenu()`: Removes context menu integration
  - `IsContextMenuRegistered()`: Checks registration status

## Key Implementation Details

### Thumbnail Generation

Uses iText7 to read PDF pages and System.Drawing to create preview images:

```csharp
var pdfDocument = new PdfDocument(new PdfReader(filePath));
var firstPage = pdfDocument.GetPage(1);
// Render to bitmap for preview
```

### PDF Merging

Copies pages from source PDFs to output document:

```csharp
var outputDocument = new PdfDocument(new PdfWriter(outputPath));
foreach (var filePath in filePaths)
{
    var sourceDocument = new PdfDocument(new PdfReader(filePath));
    for (int i = 1; i <= sourceDocument.GetNumberOfPages(); i++)
    {
        var page = sourceDocument.GetPage(i);
        outputDocument.AddPage(page.CopyTo(outputDocument));
    }
}
```

### Context Menu Integration

Registry entries are created in `HKEY_CURRENT_USER\Software\Classes\*\shell\`:

```
HKEY_CURRENT_USER
  └─ Software
      └─ Classes
          └─ *
              └─ shell
                  └─ MergePDFs
                      ├─ (Default) = "Merge PDFs"
                      ├─ Icon = "path\to\PDFMerger.exe,0"
                      └─ command
                          └─ (Default) = "path\to\PDFMerger.exe" "%1"
```

### Command Line Arguments

- No arguments: Show empty merger window
- PDF file paths: Load files and show merger window
- `--register-context-menu`: Register File Explorer integration
- `--unregister-context-menu`: Remove File Explorer integration

## Extension Points

### Adding Compression Options

Modify `PDFService.MergePDFsAsync()` to accept compression settings:

```csharp
public static async Task<bool> MergePDFsAsync(
    List<string> filePaths, 
    string outputPath,
    CompressionLevel compression = CompressionLevel.Default)
```

### Drag-and-Drop Support

Add `Drop` event handler to `ThumbnailGrid`:

```xaml
<GridView AllowDrop="True" Drop="ThumbnailGrid_Drop" />
```

### Batch Processing

Create `BatchMergeService` to handle multiple merge operations from command line.

## Testing

### Unit Tests

Create tests for:

- `PDFService.IsValidPDF()` - with valid and invalid files
- `PDFService.GenerateDefaultFileName()` - format validation
- `ContextMenuService.RegisterContextMenu()` - registry access

### Integration Tests

- End-to-end merge operation
- File creation and validation
- Context menu functionality

## Debugging Tips

1. **Add Debug Output**:

   ```csharp
   System.Diagnostics.Debug.WriteLine($"Loading PDF: {filePath}");
   ```

2. **Check Registry** (for context menu issues):
   - Open Registry Editor (regedit)
   - Navigate to `HKEY_CURRENT_USER\Software\Classes\*\shell\MergePDFs`

3. **File Permissions**:
   - Ensure write access to output directory
   - Check PDF file permissions

4. **PDF Validation**:
   - Verify input PDFs are not corrupted
   - Test with sample PDFs first

## Performance Considerations

- **Thumbnail Generation**: Run asynchronously to prevent UI freezing
- **Large PDFs**: Consider streaming approach for large files
- **Memory**: iText7 loads entire PDF in memory; manage for large files

## Distribution

### Creating Installer

Use WiX Toolset or MSIX packaging:

```xml
<!-- Package.appxmanifest for MSIX -->
<Package>
  <Identity Name="PDFMerger" Publisher="CN=PDFMerger" />
  <Applications>
    <Application>
      <uap:VisualElements DisplayName="PDF Merger" />
    </Application>
  </Applications>
</Package>
```

### Code Signing

Sign executable and installer with certificate for distribution.

## Troubleshooting

| Issue | Solution |
|-------|----------|
| "Windows App SDK not found" | Install Windows App SDK workload in VS |
| Thumbnails not showing | Check image data generation in PDFService |
| Context menu not appearing | Run --register-context-menu with admin rights |
| Merge operation fails | Validate PDFs with external tool first |
