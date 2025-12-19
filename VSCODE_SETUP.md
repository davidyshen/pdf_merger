# Building PDF Merger in VS Code

Yes! You can absolutely build and develop the PDF Merger project in VS Code. This guide shows you how.

---

## ✅ Prerequisites

Before starting, ensure you have:

- [ ] Windows 10 Build 22621 or Windows 11
- [ ] .NET 8.0 SDK (download from <https://dotnet.microsoft.com/download>)
- [ ] VS Code (download from <https://code.visualstudio.com>)
- [ ] C# extension for VS Code (ms-dotnettools.csharp)
- [ ] Optional: REST Client extension (for debugging)

### Verify Prerequisites

```powershell
# Check .NET SDK installed
dotnet --version

# Should show 8.0.x or later

# Check VS Code is installed
code --version
```

---

## 🚀 Quick Start (5 Minutes)

### 1. Open Project in VS Code

```powershell
# Navigate to project folder
cd "C:\Users\dyshe\Git projects\pdf_merger"

# Open in VS Code
code .
```

### 2. Install C# Extension (if not already installed)

1. Click Extensions icon (Ctrl+Shift+X)
2. Search for "C#"
3. Install "C# Dev Kit" by Microsoft
4. Wait for installation to complete

### 3. Build the Project

Press **Ctrl+Shift+B** or use Terminal:

```powershell
dotnet build PDFMerger.csproj
```

### 4. Run the Application

```powershell
dotnet run --project PDFMerger.csproj
```

**Done!** The PDF Merger window should launch.

---

## 📋 Complete Setup Guide

### Step 1: Install C# Extensions

**Option A: VS Code Extensions GUI**

1. Open VS Code
2. Press `Ctrl+Shift+X` (Extensions)
3. Search and install:
   - **C# Dev Kit** (Microsoft)
   - **Pylance** (for better IntelliSense)
   - **Code Runner** (optional, for quick execution)

**Option B: Command Line**

```powershell
code --install-extension ms-dotnettools.csharp
code --install-extension ms-vscode.csharp
```

### Step 2: Verify .NET Installation

```powershell
# Check if .NET 8.0 is installed
dotnet --list-sdks

# Expected output includes: 8.0.x

# If not installed, download from:
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### Step 3: Open Project

```powershell
# Navigate to project directory
cd "C:\Users\dyshe\Git projects\pdf_merger"

# Open in VS Code
code .

# Or open VS Code first, then File > Open Folder
```

### Step 4: Restore NuGet Packages

VS Code should do this automatically, but you can manually run:

```powershell
dotnet restore PDFMerger.csproj
```

---

## 🛠️ Building in VS Code

### Method 1: Using Terminal (Recommended)

```powershell
# Navigate to project
cd "C:\Users\dyshe\Git projects\pdf_merger"

# Build
dotnet build PDFMerger.csproj

# Build Release version
dotnet build PDFMerger.csproj -c Release

# Build for specific platform
dotnet build PDFMerger.csproj -p:Platform=x64
```

### Method 2: Using VS Code Build Task (Ctrl+Shift+B)

The project includes a build task. Press **Ctrl+Shift+B** and select:

- `dotnet: build` - Debug build
- `dotnet: build (release)` - Release build

### Method 3: Using Run Button

1. Open `Program.cs` in editor
2. Click the **Run** button above the Main method (if visible)
3. Or press **F5** to debug

---

## ▶️ Running the Application

### Method 1: Direct Execution (Recommended)

```powershell
# Run in debug mode
dotnet run --project PDFMerger.csproj

# Run in release mode
dotnet run --project PDFMerger.csproj -c Release
```

### Method 2: Run Compiled Executable

After building, the executable is located at:

```
bin/Debug/net8.0-windows10.0.22621.0/PDFMerger.exe
bin/Release/net8.0-windows10.0.22621.0/PDFMerger.exe
```

Run directly:

```powershell
# Debug version
.\bin\Debug\net8.0-windows10.0.22621.0\PDFMerger.exe

# Release version
.\bin\Release\net8.0-windows10.0.22621.0\PDFMerger.exe
```

### Method 3: Using VS Code Launch Configuration

Create `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (console)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/bin/Debug/net8.0-windows10.0.22621.0/PDFMerger.exe",
      "args": [],
      "cwd": "${workspaceFolder}",
      "stopAtEntry": false,
      "console": "integratedTerminal"
    }
  ]
}
```

Then press **F5** to launch with debugger.

---

## 🐛 Debugging in VS Code

### Enable Debugger

1. Open `Program.cs` or any C# file
2. Set breakpoints by clicking on line numbers
3. Press **F5** to start debugging
4. The debugger will stop at breakpoints

### Debugging Features

- **Step Over**: F10
- **Step Into**: F11
- **Step Out**: Shift+F11
- **Continue**: F5
- **Stop**: Shift+F5

### View Variables

While debugging, use the Debug panel on the left to:

- View variables
- Watch expressions
- View the call stack
- Inspect objects

---

## 📊 Terminal Commands Reference

### Build Commands

```powershell
# Full rebuild
dotnet clean PDFMerger.csproj
dotnet build PDFMerger.csproj

# Release build
dotnet build PDFMerger.csproj -c Release

# Build specific platform
dotnet build PDFMerger.csproj -p:Platform=x64

# Verbose output
dotnet build PDFMerger.csproj -v detailed
```

### Run Commands

```powershell
# Run debug version
dotnet run --project PDFMerger.csproj

# Run with arguments
dotnet run --project PDFMerger.csproj -- "path\to\file.pdf"

# Run release version
dotnet run --project PDFMerger.csproj -c Release
```

### Testing Commands

```powershell
# Restore packages
dotnet restore PDFMerger.csproj

# Clean build artifacts
dotnet clean PDFMerger.csproj

# List SDKs
dotnet --list-sdks

# List global tools
dotnet tool list -g
```

---

## 🎯 VS Code Shortcuts for .NET Development

| Shortcut | Action |
|----------|--------|
| Ctrl+Shift+B | Open build task menu |
| F5 | Start debugging |
| Shift+F5 | Stop debugging |
| F9 | Toggle breakpoint |
| F10 | Step over |
| F11 | Step into |
| Shift+F11 | Step out |
| Ctrl+Shift+D | Open Debug view |
| Ctrl+Shift+X | Open Extensions |
| Ctrl+` | Open integrated terminal |
| Ctrl+K Ctrl+I | Show hover information |

---

## 🗂️ Project Structure in VS Code

When you open the project, you'll see:

```
pdf_merger/
├── Views/
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── ViewModels/
│   └── MainWindowViewModel.cs
├── Services/
│   ├── PDFService.cs
│   └── ContextMenuService.cs
├── App.xaml
├── App.xaml.cs
├── Program.cs
├── NativeMethods.cs
├── PDFMerger.csproj
├── PDFMerger.sln
├── .vscode/
│   ├── launch.json (if created)
│   └── tasks.json (if created)
└── [documentation files]
```

---

## ⚙️ Recommended VS Code Extensions for this Project

| Extension | Purpose | Author |
|-----------|---------|--------|
| C# Dev Kit | C# development | Microsoft |
| C# | Language support | Microsoft |
| Pylance | IntelliSense & code analysis | Microsoft |
| Code Runner | Quick code execution | formulahendry |
| REST Client | API testing (optional) | Huachao Mao |
| Markdown All in One | Markdown editing | Yu Zhang |
| Git Graph | Git visualization (optional) | mhutchie |

Install all at once:

```powershell
code --install-extension ms-dotnettools.csharp
code --install-extension ms-vscode.csharp
code --install-extension ms-python.python
code --install-extension formulahendry.code-runner
code --install-extension eamodio.gitlens
```

---

## 🔧 Creating VS Code Tasks

Create `.vscode/tasks.json` in your project root:

```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/PDFMerger.csproj",
        "/property:GenerateFullPaths=true",
        "/consoleloggerparameters:NoSummary"
      ],
      "problemMatcher": "$msCompile",
      "group": {
        "kind": "build",
        "isDefault": true
      }
    },
    {
      "label": "run",
      "command": "dotnet",
      "type": "process",
      "args": [
        "run",
        "--project",
        "${workspaceFolder}/PDFMerger.csproj"
      ]
    },
    {
      "label": "publish",
      "command": "dotnet",
      "type": "process",
      "args": [
        "publish",
        "${workspaceFolder}/PDFMerger.csproj",
        "-c",
        "Release"
      ]
    }
  ]
}
```

Then use: **Terminal → Run Task** → select task

---

## 🚀 Workflow in VS Code

### Daily Development Workflow

1. **Open Project**

   ```powershell
   code "C:\Users\dyshe\Git projects\pdf_merger"
   ```

2. **Make Code Changes**
   - Edit files in editor
   - IntelliSense helps with suggestions

3. **Build**

   ```powershell
   # Press Ctrl+Shift+B or run in terminal:
   dotnet build PDFMerger.csproj
   ```

4. **Run & Test**

   ```powershell
   dotnet run --project PDFMerger.csproj
   ```

5. **Debug (if needed)**
   - Press F9 to set breakpoints
   - Press F5 to debug
   - Use Debug panel to inspect variables

6. **Commit Changes**
   - Use Git integration (Ctrl+Shift+G)
   - Or command line: `git commit -am "message"`

---

## 📊 Common VS Code Issues & Solutions

### Issue: "C# extension not found"

**Solution**: Install C# Dev Kit from Extensions (Ctrl+Shift+X)

### Issue: "dotnet: command not found"

**Solution**: Install .NET 8.0 SDK from <https://dotnet.microsoft.com/download>

### Issue: IntelliSense not working

**Solution**:

1. Install Pylance extension
2. Reload VS Code (Ctrl+Shift+P → Developer: Reload Window)

### Issue: Build fails with "Windows SDK not found"

**Solution**: This is expected - just run the executable from bin folder

### Issue: "WinUI3 not found" error

**Solution**: Run `dotnet restore` to install NuGet packages

### Issue: NuGet packages won't download

**Solution**:

```powershell
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore PDFMerger.csproj
```

---

## 🎯 Building Release Version

For production deployment:

```powershell
# Clean previous builds
dotnet clean PDFMerger.csproj

# Build release version
dotnet build PDFMerger.csproj -c Release

# Output will be at:
# bin/Release/net8.0-windows10.0.22621.0/PDFMerger.exe

# You can also publish it:
dotnet publish PDFMerger.csproj -c Release -o ./publish
```

---

## 📝 Useful VS Code Commands (Ctrl+Shift+P)

```
C# Generate Assets for Build and Debug    - Create launch.json/tasks.json
C# Show Output Window                      - View build output
Developer: Reload Window                   - Reload VS Code
Terminal: Create New Terminal              - Open new terminal
View: Toggle Integrated Terminal           - Show/hide terminal
Debug: Run Without Debugging               - Run without debugger
```

---

## ✅ Checklist for VS Code Setup

- [ ] VS Code installed
- [ ] .NET 8.0 SDK installed and verified
- [ ] C# extension installed
- [ ] Project opened in VS Code
- [ ] NuGet packages restored
- [ ] Project builds successfully (Ctrl+Shift+B)
- [ ] Application runs successfully (dotnet run)
- [ ] Breakpoints work (F5 to debug)
- [ ] Git integration works (optional)

---

## 🎬 Quick Start (Copy & Paste)

```powershell
# 1. Open in VS Code
cd "C:\Users\dyshe\Git projects\pdf_merger"
code .

# 2. In VS Code terminal, build
dotnet build PDFMerger.csproj

# 3. Run
dotnet run --project PDFMerger.csproj

# 4. Done! Application launches
```

---

## 🔗 Useful Resources

- [.NET CLI Documentation](https://learn.microsoft.com/dotnet/core/tools/)
- [VS Code C# Guide](https://code.visualstudio.com/docs/languages/csharp)
- [WinUI 3 Documentation](https://learn.microsoft.com/windows/apps/winui/winui3/)
- [Debug in VS Code](https://code.visualstudio.com/docs/editor/debugging)

---

## Summary

✅ **Yes, you can build in VS Code!**

**Quick Commands:**

- Build: `dotnet build PDFMerger.csproj`
- Run: `dotnet run --project PDFMerger.csproj`
- Debug: Press F5 (after setting breakpoints with F9)

**Extensions Needed:**

- C# Dev Kit (Microsoft)
- C# (Microsoft)

**That's it!** VS Code works great for .NET projects. It's lightweight, fast, and has excellent C# support.

---

**Start now:**

```powershell
cd "C:\Users\dyshe\Git projects\pdf_merger"
code .
```

Then press **Ctrl+Shift+B** to build!
