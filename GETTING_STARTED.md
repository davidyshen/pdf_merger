# Getting Started Checklist

## 🎯 Quick Start (15 minutes)

### Prerequisites Check

- [ ] Windows 10 Build 22621 or Windows 11
- [ ] .NET 8.0 SDK installed (`dotnet --version`)
- [ ] **EITHER** Visual Studio 2022 **OR** VS Code installed
- [ ] Administrator access for testing context menu

### First Build - Choose Your IDE

#### Option A: Visual Studio 2022

- [ ] Open `PDFMerger.sln` in Visual Studio 2022
- [ ] Right-click solution → "Restore NuGet Packages"
- [ ] Press `Ctrl+Shift+B` to build solution
- [ ] Wait for build to complete (should be 1-3 minutes)
- [ ] Check Output window for "Build succeeded"

#### Option B: VS Code (Lightweight)

- [ ] Open folder in VS Code: `code .`
- [ ] Open terminal (Ctrl+`)
- [ ] Run: `dotnet build PDFMerger.csproj`
- [ ] Wait for build to complete
- [ ] See [VSCODE_SETUP.md](VSCODE_SETUP.md) for more details

### First Run

**Visual Studio:**

- [ ] Press `F5` to start debugging

**VS Code or Command Line:**

- [ ] Run: `dotnet run --project PDFMerger.csproj`

- [ ] Window titled "PDF Merger" should appear
- [ ] Close the window (application works!)

---

## 📝 Initial Setup (30 minutes)

### Prepare Test Files

- [ ] Gather 3-5 PDF files of varying sizes
- [ ] Create folder: `C:\TestPDFs\`
- [ ] Copy test PDFs to this folder

### Test Basic Functionality

- [ ] Launch PDF Merger
- [ ] Drag PDF files onto window (if drag-drop implemented)
- [ ] Or manually select via File menu (if implemented)
- [ ] Verify thumbnails appear
- [ ] Test move up/down buttons
- [ ] Test remove button
- [ ] Enter output filename
- [ ] Click Merge PDFs
- [ ] Verify merged file appears in source directory

### Test File Explorer Integration

- [ ] Build in Release mode: `Ctrl+Shift+B`
- [ ] Locate output: `bin\Release\net8.0-windows10.0.22621.0\PDFMerger.exe`
- [ ] Open Command Prompt as Administrator
- [ ] Navigate to PDFMerger directory
- [ ] Register context menu:

  ```batch
  PDFMerger.exe --register-context-menu
  ```

- [ ] Open File Explorer
- [ ] Select 2+ PDF files
- [ ] Right-click and look for "Merge PDFs"
- [ ] Click "Merge PDFs" - window should open with files

---

## 🧪 Validation Testing (1 hour)

### Unit Testing

- [ ] Review TESTING.md for test cases
- [ ] Run unit tests (if test project created)
- [ ] Verify PDFService functions work correctly
- [ ] Verify ContextMenuService registry operations

### Integration Testing

- [ ] Test merge with 2 PDFs
- [ ] Test merge with 5+ PDFs
- [ ] Test with mixed file sizes
- [ ] Test reordering before merge
- [ ] Test custom filename
- [ ] Test default filename generation
- [ ] Test cancel button
- [ ] Test with problematic PDFs (corrupted, encrypted)

### User Experience Testing

- [ ] Verify window is resizable
- [ ] Verify UI elements are properly aligned
- [ ] Verify error messages are clear
- [ ] Verify success message shows correct path
- [ ] Test keyboard navigation (Tab, arrows)

---

## 📦 Deployment Preparation (2 hours)

### Code Review

- [ ] Review all C# files for code quality
- [ ] Check for any TODO or FIXME comments
- [ ] Verify error handling is comprehensive
- [ ] Check for memory leaks or resource leaks

### Documentation Review

- [ ] Read through README.md
- [ ] Verify INSTALLATION.md is accurate
- [ ] Check DEVELOPMENT.md for completeness
- [ ] Review code comments for clarity

### Build for Release

- [ ] Set configuration to Release (not Debug)
- [ ] Select x64 platform (or x86, ARM64 as needed)
- [ ] Clean solution: `Build → Clean Solution`
- [ ] Build solution: `Ctrl+Shift+B`
- [ ] Verify no build errors or warnings
- [ ] Check output directory for artifacts

### Testing Release Build

- [ ] Unregister context menu (previous registration)

  ```batch
  PDFMerger.exe --unregister-context-menu
  ```

- [ ] Copy Release build PDFMerger.exe to test location
- [ ] Test application launch from new location
- [ ] Register context menu from new location
- [ ] Test context menu integration

### Create Installation Package

- [ ] Create folder: `C:\Program Files\PDFMerger\`
- [ ] Copy PDFMerger.exe to this folder
- [ ] Copy dependent DLLs if needed
- [ ] Create shortcut on Desktop
- [ ] Test launching from folder
- [ ] Test registering context menu

---

## 📋 Pre-Release Checklist

### Functionality Verification

- [ ] All buttons functional
- [ ] Merge produces valid PDF
- [ ] Output filename works (both custom and auto)
- [ ] Context menu appears on right-click
- [ ] Up/down buttons reorder correctly
- [ ] Remove button works
- [ ] Cancel button closes window without merging
- [ ] Error dialogs display appropriately

### File Handling

- [ ] Handles long filenames
- [ ] Handles special characters in filenames
- [ ] Handles Unicode filenames
- [ ] Preserves file permissions
- [ ] Doesn't create temp files in wrong location
- [ ] Cleans up properly on error

### Performance

- [ ] Thumbnails generate within 500ms
- [ ] Merge completes without freezing UI
- [ ] Memory usage reasonable
- [ ] No memory leaks after repeated operations

### Registry/System Integration

- [ ] Registry entries created in correct location
- [ ] Entries removed completely on unregister
- [ ] Doesn't require elevation for normal operation
- [ ] Works with user's current permissions

### Documentation

- [ ] README.md is current
- [ ] Installation instructions work
- [ ] Troubleshooting section addresses known issues
- [ ] All code files have appropriate comments

---

## 🚀 Post-Release Tasks

### Distribution

- [ ] Create installer/package
- [ ] Sign executable (if distributing)
- [ ] Create release notes
- [ ] Upload to website/repository
- [ ] Test download and installation

### Support

- [ ] Monitor for user issues
- [ ] Track feature requests
- [ ] Create FAQ if needed
- [ ] Prepare support documentation

### Future Versions

- [ ] Plan next features (see DEVELOPMENT.md)
- [ ] Gather user feedback
- [ ] Prioritize enhancements
- [ ] Plan v1.1 release

---

## 📊 Metrics to Track

### Build Metrics

- Build time: ___ seconds
- Release binary size: ___ MB
- Total dependencies: ___

### Performance Metrics

- Thumbnail generation time: ___ ms
- Merge time for 10MB files: ___ seconds
- Memory usage (idle): ___ MB
- Memory usage (merging): ___ MB

### Quality Metrics

- Lines of code: ___
- Code files: ___
- Documentation pages: ___
- Test cases: ___

---

## 🔗 Important Files Reference

| File | Purpose | Status |
|------|---------|--------|
| PDFMerger.sln | Visual Studio solution | ✓ Created |
| App.xaml(.cs) | Application entry point | ✓ Created |
| MainWindow.xaml(.cs) | Main UI and logic | ✓ Created |
| PDFService.cs | PDF operations | ✓ Created |
| ContextMenuService.cs | Windows integration | ✓ Created |
| README.md | User documentation | ✓ Created |
| INSTALLATION.md | Setup guide | ✓ Created |
| DEVELOPMENT.md | Developer guide | ✓ Created |

---

## 💡 Tips

**Tip 1: Quick Context Menu Testing**

```batch
REM Register for testing
PDFMerger.exe --register-context-menu

REM Unregister when done testing
PDFMerger.exe --unregister-context-menu
```

**Tip 2: Creating Test PDFs**

- Use free online PDF creator tools
- Convert Word documents to PDF
- Export from PowerPoint
- Scan documents to PDF

**Tip 3: Debugging**

- Use Visual Studio debugger (F5)
- Set breakpoints with F9
- View variables in Debug window
- Check registry with regedit during testing

**Tip 4: Performance Profiling**

- Use Visual Studio Profiler (Debug → Performance Profiler)
- Monitor memory usage during merge
- Test with progressively larger files

---

## ❓ Troubleshooting During Setup

**Problem**: Build fails with "Windows App SDK not found"  
**Solution**: Install Windows App SDK workload in Visual Studio Installer

**Problem**: "PDFMerger.exe not found" when running scripts  
**Solution**: Ensure script is in same directory as .exe, or use full path

**Problem**: Context menu doesn't appear after registration  
**Solution**:

1. Run registration command as Administrator
2. Restart File Explorer (Task Manager → Kill explorer.exe → Start explorer.exe)
3. Verify registry entry exists in regedit

**Problem**: Application crashes on launch  
**Solution**:

1. Check .NET 8.0 is installed: `dotnet --version`
2. Check all dependencies are present in bin folder
3. Run as Administrator
4. Check Windows Event Viewer for error details

**Problem**: Merge button doesn't work  
**Solution**:

1. Verify at least 2 PDFs are selected
2. Check output directory is writable
3. Verify PDFs are not corrupted
4. Check event log for specific errors

---

## ✅ Success Criteria

Your setup is complete when:

✅ Visual Studio opens PDFMerger.sln without errors  
✅ Solution builds successfully (Release + x64)  
✅ Application launches and displays UI  
✅ Merge operation produces valid PDF  
✅ Context menu appears in File Explorer after registration  
✅ All documentation files are present and readable  
✅ No warnings in Build Output  
✅ Application doesn't crash during normal usage  

---

**Version**: 1.0  
**Last Updated**: December 2025  
**Estimated Completion Time**: 4-5 hours total (with thorough testing)

---

**You're all set! Start with opening PDFMerger.sln in Visual Studio 2022** 🚀
