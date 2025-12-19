#!/bin/bash
# PDF Merger Context Menu Setup Script (PowerShell)
# This script registers or unregisters the "Merge PDFs" context menu option

param(
    [Parameter(Mandatory=$false)]
    [string]$Action
)

# Find the application executable
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

# Try multiple locations for the executable
$possiblePaths = @(
    (Join-Path $scriptDir "PDFMerger.exe"),
    (Join-Path $scriptDir "bin\ARM64\Debug\net8.0-windows10.0.22621.0\win-arm64\PDFMerger.exe"),
    (Join-Path $scriptDir "bin\x64\Debug\net8.0-windows10.0.22621.0\win-x64\PDFMerger.exe"),
    (Join-Path $scriptDir "bin\Debug\net8.0-windows10.0.22621.0\PDFMerger.exe"),
    (Join-Path $scriptDir "bin\Release\net8.0-windows10.0.22621.0\PDFMerger.exe")
)

$appPath = $null
foreach ($path in $possiblePaths) {
    if (Test-Path $path) {
        $appPath = (Resolve-Path $path).Path
        break
    }
}

if ($null -eq $appPath) {
    Write-Host "Error: PDFMerger.exe not found. Please build the project first."
    exit 1
}

Write-Host "Found PDFMerger.exe at: $appPath"

# Check for administrator privileges
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if ([string]::IsNullOrEmpty($Action)) {
    Write-Host ""
    Write-Host "PDF Merger Context Menu Setup"
    Write-Host "=============================="
    Write-Host ""
    Write-Host "Usage: .\setup-context-menu.ps1 -Action <action>"
    Write-Host ""
    Write-Host "Actions:"
    Write-Host "  register          - Register in legacy context menu (Show more options)"
    Write-Host "  unregister        - Remove from legacy context menu"
    Write-Host "  check             - Check if context menu is registered"
    Write-Host ""
    exit 0
}

if ($Action -eq "register") {
    if (-not $isAdmin) {
        Write-Host "Registration requires administrator privileges."
        exit 1
    }

    Write-Host "Registering legacy context menu..."
    try {
        & reg add "HKCU\Software\Classes\*\shell\MergePDFs" /ve /d "Merge PDFs" /f | Out-Null
        & reg add "HKCU\Software\Classes\*\shell\MergePDFs" /v "Icon" /d "$appPath" /f | Out-Null
        & reg add "HKCU\Software\Classes\*\shell\MergePDFs\command" /ve /d "`"$appPath`" `"%1`"" /f | Out-Null
        
        & reg add "HKCU\Software\Classes\.pdf\shell\MergePDFs" /ve /d "Merge PDFs" /f | Out-Null
        & reg add "HKCU\Software\Classes\.pdf\shell\MergePDFs" /v "Icon" /d "$appPath" /f | Out-Null
        & reg add "HKCU\Software\Classes\.pdf\shell\MergePDFs\command" /ve /d "`"$appPath`" `"%1`"" /f | Out-Null

        & reg add "HKCU\Software\Classes\Directory\shell\MergePDFs" /ve /d "Merge PDFs in Folder" /f | Out-Null
        & reg add "HKCU\Software\Classes\Directory\shell\MergePDFs" /v "Icon" /d "$appPath" /f | Out-Null
        & reg add "HKCU\Software\Classes\Directory\shell\MergePDFs\command" /ve /d "`"$appPath`" `"%1`"" /f | Out-Null
        
        Write-Host "✓ Legacy context menu registered successfully!"
    } catch {
        Write-Host "✗ Failed to register legacy context menu: $_"
        exit 1
    }
    exit 0
}

if ($Action -eq "unregister") {
    if (-not $isAdmin) {
        Write-Host "Unregistration requires administrator privileges."
        exit 1
    }

    Write-Host "Unregistering legacy context menu..."
    try {
        & reg delete "HKCU\Software\Classes\*\shell\MergePDFs" /f | Out-Null
        & reg delete "HKCU\Software\Classes\.pdf\shell\MergePDFs" /f | Out-Null
        & reg delete "HKCU\Software\Classes\Directory\shell\MergePDFs" /f | Out-Null
        Write-Host "✓ Legacy context menu unregistered successfully."
    } catch {
        Write-Host "✗ Failed to unregister: $_"
        exit 1
    }
    exit 0
}

if ($Action -eq "check") {
    $legacy = Test-Path "HKCU:\Software\Classes\*\shell\MergePDFs"
    if ($legacy) { Write-Host "✓ Context menu: Registered" } else { Write-Host "✗ Context menu: Not registered" }
    exit 0
}
