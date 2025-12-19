# PDF Merger - Testing & QA Guide

## Testing Overview

This document provides comprehensive testing guidelines for the PDF Merger application.

## Pre-Testing Checklist

- [ ] .NET 8.0 runtime installed
- [ ] Visual Studio 2022 with Windows App SDK workload
- [ ] Test PDF files available (various sizes and formats)
- [ ] Administrator command prompt available
- [ ] Registry editor accessible for verification

## Unit Testing

### PDFService Tests

#### Test 1: PDF Validation

```csharp
[TestMethod]
public void IsValidPDF_WithValidFile_ReturnsTrue()
{
    // Arrange
    var testFile = "test_valid.pdf";
    
    // Act
    var result = PDFService.IsValidPDF(testFile);
    
    // Assert
    Assert.IsTrue(result);
}

[TestMethod]
public void IsValidPDF_WithInvalidFile_ReturnsFalse()
{
    // Arrange
    var testFile = "test_corrupted.pdf";
    
    // Act
    var result = PDFService.IsValidPDF(testFile);
    
    // Assert
    Assert.IsFalse(result);
}
```

#### Test 2: Default Filename Generation

```csharp
[TestMethod]
public void GenerateDefaultFileName_ReturnsCorrectFormat()
{
    // Arrange
    var pattern = @"merged_pdf_\d{4}-\d{2}-\d{2}_\d{2}-\d{2}-\d{2}\.pdf";
    
    // Act
    var filename = PDFService.GenerateDefaultFileName();
    
    // Assert
    Assert.IsTrue(Regex.IsMatch(filename, pattern));
}
```

#### Test 3: Thumbnail Generation

```csharp
[TestMethod]
public async Task GenerateThumbnailAsync_ReturnsImageData()
{
    // Arrange
    var testFile = "test_single_page.pdf";
    
    // Act
    var thumbnail = await PDFService.GenerateThumbnailAsync(testFile);
    
    // Assert
    Assert.IsNotNull(thumbnail);
    Assert.IsTrue(thumbnail.Length > 0);
}

[TestMethod]
public async Task GenerateThumbnailAsync_WithInvalidFile_ReturnsNull()
{
    // Arrange
    var testFile = "test_not_a_pdf.txt";
    
    // Act
    var thumbnail = await PDFService.GenerateThumbnailAsync(testFile);
    
    // Assert
    Assert.IsNull(thumbnail);
}
```

#### Test 4: PDF Merging

```csharp
[TestMethod]
public async Task MergePDFsAsync_CombinesMultipleFiles()
{
    // Arrange
    var files = new List<string> { "test1.pdf", "test2.pdf", "test3.pdf" };
    var output = "test_merged.pdf";
    
    // Act
    var result = await PDFService.MergePDFsAsync(files, output);
    
    // Assert
    Assert.IsTrue(result);
    Assert.IsTrue(File.Exists(output));
    Assert.IsTrue(PDFService.IsValidPDF(output));
}

[TestMethod]
public async Task MergePDFsAsync_PreservesPageOrder()
{
    // Arrange
    var files = new List<string> { "test1.pdf", "test2.pdf" };
    var output = "test_merged_order.pdf";
    
    // Act
    await PDFService.MergePDFsAsync(files, output);
    var mergedDoc = new PdfDocument(new PdfReader(output));
    
    // Assert
    Assert.IsTrue(mergedDoc.GetNumberOfPages() > 0);
    // Verify page count matches sum of inputs
}
```

### ContextMenuService Tests

#### Test 5: Context Menu Registration

```csharp
[TestMethod]
public void RegisterContextMenu_CreatesRegistryEntries()
{
    // Arrange
    var appPath = "C:\\Program Files\\PDFMerger\\PDFMerger.exe";
    
    // Act
    var result = ContextMenuService.RegisterContextMenu(appPath);
    
    // Assert
    Assert.IsTrue(result);
    Assert.IsTrue(ContextMenuService.IsContextMenuRegistered());
}

[TestMethod]
public void IsContextMenuRegistered_AfterRegistration_ReturnsTrue()
{
    // Arrange
    var appPath = "C:\\Program Files\\PDFMerger\\PDFMerger.exe";
    ContextMenuService.RegisterContextMenu(appPath);
    
    // Act
    var isRegistered = ContextMenuService.IsContextMenuRegistered();
    
    // Assert
    Assert.IsTrue(isRegistered);
}

[TestMethod]
public void UnregisterContextMenu_RemovesRegistryEntries()
{
    // Arrange
    var appPath = "C:\\Program Files\\PDFMerger\\PDFMerger.exe";
    ContextMenuService.RegisterContextMenu(appPath);
    
    // Act
    var result = ContextMenuService.UnregisterContextMenu();
    
    // Assert
    Assert.IsTrue(result);
    Assert.IsFalse(ContextMenuService.IsContextMenuRegistered());
}
```

## Integration Testing

### Test Scenario 1: Complete Merge Workflow

**Objective**: Verify end-to-end merge operation

**Steps**:

1. Launch application with 3 test PDF files as arguments
2. Verify thumbnails appear in grid
3. Reorder files using up/down buttons
4. Enter custom output filename
5. Click Merge button
6. Verify success dialog appears
7. Verify output file exists and is valid PDF

**Expected Result**:

- Output file contains all pages in correct order
- File named as specified
- Located in source directory

### Test Scenario 2: Context Menu Integration

**Objective**: Verify File Explorer context menu works correctly

**Steps**:

1. Register context menu via command line
2. Open File Explorer
3. Select 2-3 PDF files
4. Right-click and look for "Merge PDFs" option
5. Click "Merge PDFs"
6. Application window opens with correct files

**Expected Result**:

- Context menu appears consistently
- Correct files are loaded
- All files are valid PDFs

### Test Scenario 3: File Reordering

**Objective**: Verify reordering preserves merge order

**Steps**:

1. Load 3 PDFs: a.pdf (10 pages), b.pdf (5 pages), c.pdf (3 pages)
2. Reorder to: c → b → a
3. Merge without custom name
4. Open merged PDF

**Expected Result**:

- Merged PDF has 18 total pages (3+5+10)
- First 3 pages from c.pdf
- Next 5 pages from b.pdf
- Last 10 pages from a.pdf

### Test Scenario 4: Error Handling

**Objective**: Verify proper error handling

**Steps**:

1. Select 1 PDF and try to merge (should show error)
2. Load PDFs but provide invalid filename (e.g., with invalid characters)
3. Load PDFs from read-only directory
4. Merge corrupted PDF file

**Expected Result**:

- Error dialogs appear with clear messages
- Application doesn't crash
- Users can correct issues and retry

## Manual Testing Checklist

### Basic Functionality

- [ ] Application launches without arguments
- [ ] Application launches with PDF file arguments
- [ ] Thumbnails display correctly
- [ ] Thumbnails match PDF content
- [ ] Files appear in correct order in grid

### Reordering

- [ ] Up button works for all but first item
- [ ] Down button works for all but last item
- [ ] Remove button removes selected item
- [ ] Grid updates after each operation
- [ ] Selection persists during reordering

### Merging

- [ ] Custom filename is accepted
- [ ] Default filename uses correct format
- [ ] Output file is created in correct directory
- [ ] Merge succeeds with 2 PDFs minimum
- [ ] Merge fails gracefully with 1 PDF

### UI/UX

- [ ] Window is resizable
- [ ] Controls remain responsive during operations
- [ ] Scrolling works for large PDF lists
- [ ] Dialog messages are clear
- [ ] Cancel button closes window
- [ ] Close button (X) closes window

### Edge Cases

- [ ] Large PDF files (100+ MB)
- [ ] Large PDF page counts (1000+ pages)
- [ ] Multiple instances running simultaneously
- [ ] Merging PDFs with different page sizes
- [ ] Merging encrypted PDFs (should fail gracefully)
- [ ] Filename with special characters
- [ ] Very long filenames
- [ ] PDF files with special names (unicode, etc.)

### Context Menu

- [ ] Appears when selecting PDFs
- [ ] Doesn't appear for non-PDF files
- [ ] Works with single PDF selected
- [ ] Works with multiple PDFs selected
- [ ] Correctly passes file paths to application
- [ ] Works after registration/unregistration

### Performance

- [ ] Thumbnail generation doesn't freeze UI
- [ ] Merge completes in reasonable time (< 10 sec for 50MB)
- [ ] No memory leaks during repeated operations
- [ ] Application responds to cancellation requests

## Test Data Requirements

### Sample PDF Files Needed

```
test_files/
├── single_page.pdf      (1 page, simple)
├── multi_page.pdf       (10 pages, mixed content)
├── large_file.pdf       (50+ MB)
├── small_file.pdf       (< 1 MB)
├── unicode_名前.pdf     (unicode filename)
├── corrupted.pdf        (intentionally corrupted)
├── encrypted.pdf        (password protected)
├── image_only.pdf       (scanned images)
└── text_only.pdf        (text content)
```

## Performance Benchmarks

### Target Performance

| Operation | Target Time | Tolerance |
|-----------|------------|-----------|
| Load 3 thumbnails | < 500ms | ±100ms |
| Merge 3 x 10MB PDFs | < 5 sec | ±2 sec |
| Merge 10 PDFs | < 15 sec | ±5 sec |
| UI response time | < 100ms | ±50ms |

### Memory Usage

| Scenario | Target | Max |
|----------|--------|-----|
| Idle | 50 MB | 75 MB |
| 3 PDFs loaded | 100 MB | 150 MB |
| During merge | 200 MB | 300 MB |

## Regression Testing

Before each release, verify:

- [ ] All previously found issues are fixed
- [ ] No new UI glitches
- [ ] Context menu still registers/unregisters
- [ ] Merge produces valid PDF output
- [ ] File paths with spaces work correctly
- [ ] Special characters in filenames handled
- [ ] Previous test scenarios still pass

## Accessibility Testing

- [ ] Keyboard navigation works (Tab, Enter, Arrow keys)
- [ ] Screen reader compatible (basic ARIA labels)
- [ ] High contrast mode support
- [ ] Font sizes are readable
- [ ] Colors not sole information method

## Security Testing

- [ ] No arbitrary code execution from file paths
- [ ] No path traversal vulnerabilities
- [ ] Registry operations safe from injection
- [ ] File permissions respected
- [ ] No temporary files with sensitive data

## Browser Compatibility (N/A)

*Not applicable - desktop application*

## Deployment Testing

Before release:

- [ ] Build in Release mode compiles without warnings
- [ ] All dependencies included in build output
- [ ] Registry scripts work correctly
- [ ] Installation instructions accurate
- [ ] Uninstallation leaves no orphaned files
- [ ] Context menu removal is complete

## Test Report Template

```
Test Date: [DATE]
Tester: [NAME]
Build Version: [VERSION]
OS: Windows [VERSION]
.NET Runtime: [VERSION]

| Test ID | Description | Status | Notes |
|---------|-------------|--------|-------|
| TC001   | Basic launch | PASS/FAIL | |
| TC002   | PDF loading | PASS/FAIL | |
| ...     | ...         | ...    | |

Overall Result: PASS/FAIL
Known Issues: 
Recommendations:
```

## Continuous Integration Recommendations

Implement automated testing:

```yaml
# Example GitHub Actions workflow
name: Tests
on: [push, pull_request]
jobs:
  test:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '8.0.x'
      - run: dotnet test
```

---

**Document Version**: 1.0  
**Last Updated**: December 2025
