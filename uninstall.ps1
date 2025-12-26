#!/usr/bin/env pwsh
<#
.SYNOPSIS
Remove "Merge PDFs" context menu from Windows

.DESCRIPTION
Safely removes the "Merge PDFs" option from the File Explorer right-click context menu.

RUN THIS BEFORE DELETING THE PDFMERGER FOLDER!

After running this script, it is safe to delete the entire PDF Merger folder.
To reinstall the context menu later, simply extract the ZIP and run PDFMerger.exe again.

.EXAMPLE
./uninstall.ps1
#>

$ErrorActionPreference = 'Stop'

# Colors for output
$Green = @{ ForegroundColor = 'Green' }
$Yellow = @{ ForegroundColor = 'Yellow' }
$Red = @{ ForegroundColor = 'Red' }
$Cyan = @{ ForegroundColor = 'Cyan' }

Write-Host "`n" @Cyan
Write-Host "╔════════════════════════════════════════╗" @Cyan
Write-Host "║    PDF Merger - Context Menu Removal   ║" @Cyan
Write-Host "╚════════════════════════════════════════╝" @Cyan

$registryPath = "HKCU:\Software\Classes\*\shell\MergePDFs"

try {
    if (Test-Path $registryPath) {
        Write-Host "`nRemoving 'Merge PDFs' context menu entry..." @Yellow
        Remove-Item -Path $registryPath -Force -ErrorAction Stop
        Write-Host "✓ Context menu entry removed successfully" @Green
        Write-Host "`nYou can now safely delete the PDF Merger folder." @Green
    } else {
        Write-Host "`n⚠ Context menu entry not found" @Yellow
        Write-Host "  (already removed or never registered)" @Yellow
    }
}
catch {
    Write-Host "✗ Error removing registry entry:" @Red
    Write-Host "  $_" @Red
    exit 1
}

Write-Host "`n" @Green
Write-Host "If you want to use PDF Merger again in the future:" @Cyan
Write-Host "  1. Extract the PDF Merger ZIP file" @Cyan
Write-Host "  2. Click the PDFMerger.lnk shortcut to launch" @Cyan
Write-Host "  3. The context menu will be registered automatically" @Cyan
Write-Host "`n" @Green
