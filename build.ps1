#!/usr/bin/env pwsh
<#
.SYNOPSIS
Build PDF Merger portable executable (ARM64 and x64, en-GB only)

.DESCRIPTION
Publishes a self-contained, portable executable:
- Self-contained (no .NET runtime required)
- Supports ARM64 and x64 architectures
- English (GB) language only
- Organized structure: runtime subdirectory contains exe and all DLLs
- Shortcut to exe at top level for easy access
- Creates PDFMerger-portable-[arch].zip ready for distribution

.EXAMPLE
./build.ps1
#>

param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    
    [ValidateSet('win-arm64', 'win-x64')]
    [string]$Runtime = 'win-arm64'
)

$ErrorActionPreference = 'Stop'

# Colors for output
$Green = @{ ForegroundColor = 'Green' }
$Yellow = @{ ForegroundColor = 'Yellow' }
$Red = @{ ForegroundColor = 'Red' }

Write-Host "`n=== PDF Merger Build Script ===" @Green

# Step 1: Publish self-contained executable
Write-Host "`n[1/3] Publishing $Configuration build for $Runtime..." @Yellow
$publishCmd = @(
    "dotnet publish PDFMerger.csproj",
    "-c $Configuration",
    "-r $Runtime",
    "--self-contained true",
    "-p:PublishReadyToRun=true"
)

Invoke-Expression ($publishCmd -join ' ')

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" @Red
    exit 1
}

# Step 2: Determine output paths
$outputPath = "bin\ARM64\$Configuration\net8.0-windows10.0.22621.0\$Runtime"
$publishPath = Join-Path $outputPath "publish"

if (-not (Test-Path $publishPath)) {
    Write-Host "Publish directory not found: $publishPath" @Red
    exit 1
}

Write-Host "Published to: $publishPath" @Green

# Step 3: Clean up non-English language files from publish directory
Write-Host "`n[2/3] Removing non-English language files..." @Yellow
Get-ChildItem -Path $publishPath -Directory | Where-Object {$_.Name -notmatch "^en-GB$|^en$"} | 
    Remove-Item -Recurse -Force

Write-Host "Kept only: en-GB language files" @Green

# Step 4: Create shortcut to exe (for ZIP root)
Write-Host "`n[3/3] Preparing distribution package..." @Yellow

# Create a temporary staging directory for ZIP contents
$stagingPath = "bin\PDFMerger_staging"
if (Test-Path $stagingPath) {
    Remove-Item -Path $stagingPath -Recurse -Force
}
New-Item -ItemType Directory -Path $stagingPath | Out-Null

# Copy the runtime directory to staging
Copy-Item -Path $publishPath -Destination "$stagingPath\$Runtime" -Recurse -Force

# Create .lnk shortcut to the exe
$shell = New-Object -ComObject WScript.Shell
$shortcutPath = "$stagingPath\PDFMerger.lnk"
$targetExePath = ".\$Runtime\PDFMerger.exe"
$shortcut = $shell.CreateShortcut($shortcutPath)
$shortcut.TargetPath = (Resolve-Path "$stagingPath\$Runtime\PDFMerger.exe").Path
$shortcut.WorkingDirectory = (Get-Item $stagingPath).FullName
$shortcut.Description = "PDF Merger - Click to launch"
$shortcut.IconLocation = (Resolve-Path "$stagingPath\$Runtime\PDFMerger.exe").Path
$shortcut.Save()
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($shell) | Out-Null

Write-Host "Created shortcut to PDFMerger.exe" @Green

# Copy uninstall script
if (Test-Path "uninstall.ps1") {
    Copy-Item -Path "uninstall.ps1" -Destination "$stagingPath\uninstall.ps1" -Force
}

# Step 5: Create ZIP package
Write-Host "`nCreating portable ZIP package..." @Yellow

# Map runtime to architecture name for ZIP filename
$archName = switch ($Runtime) {
    'win-arm64' { 'arm64' }
    'win-x64' { 'x64' }
    'win-x86' { 'x86' }
    default { $Runtime }
}

$zipName = "PDFMerger-portable-$archName.zip"

if (Test-Path $zipName) {
    Remove-Item $zipName -Force
}

# Create ZIP with staging directory contents
Compress-Archive -Path "$stagingPath\*" -DestinationPath $zipName -Force

if (-not (Test-Path $zipName)) {
    Write-Host "ZIP creation failed!" @Red
    exit 1
}

# Clean up staging directory
Remove-Item -Path $stagingPath -Recurse -Force

# Step 6: Show results
Write-Host "`n" @Green

$zipFile = Get-Item $zipName
$publishSize = [Math]::Round((Get-ChildItem -Path "$publishPath" -Recurse -File | Measure-Object -Property Length -Sum).Sum / 1MB, 2)
$zipSize = [Math]::Round($zipFile.Length / 1MB, 2)

Write-Host "=== Build Results ===" @Green
Write-Host "Configuration: $Configuration"
Write-Host "Runtime:       $Runtime ($archName)"
Write-Host "Uncompressed:  $publishSize MB"
Write-Host "ZIP Size:      $zipSize MB"
Write-Host "Output:        $zipName"
Write-Host "Structure:     PDFMerger.lnk (shortcut) + $Runtime/* (exe + all DLLs)"
Write-Host "`n=== Ready to distribute ===" @Green
