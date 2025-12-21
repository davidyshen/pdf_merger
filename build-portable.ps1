# Build and package portable executable

param(
    [string]$Configuration = "Release",
    [string]$Architecture = "ARM64"
)

$publishDir = "bin\$Architecture\$Configuration\net8.0-windows10.0.22621.0\win-$($Architecture.ToLower())\publish"
$zipName = "PDFMerger-portable.zip"

Write-Host "📦 Building Portable PDF Merger..." -ForegroundColor Cyan

# Step 1: Publish Release build
Write-Host "`n1️⃣  Publishing Release build..." -ForegroundColor Yellow
dotnet publish PDFMerger.csproj -c $Configuration -r "win-$($Architecture.ToLower())" --self-contained true -p:PublishReadyToRun=true
if ($LASTEXITCODE -ne 0) {
    Write-Host "   ❌ Publish failed" -ForegroundColor Red
    exit 1
}
Write-Host "   ✅ Published to: $publishDir"

# Step 2: Create ZIP archive
Write-Host "`n2️⃣  Creating portable ZIP..." -ForegroundColor Yellow
if (Test-Path $zipName) {
    Remove-Item $zipName -Force
}
Compress-Archive -Path "$publishDir\*" -DestinationPath $zipName -CompressionLevel Optimal
Write-Host "   ✅ Created: $zipName"

# Step 3: Show info
Write-Host "`n✨ Portable package ready!" -ForegroundColor Green
Get-Item $zipName | Select-Object -Property FullName, @{Name="Size(MB)";Expression={[math]::Round($_.Length/1MB,2)}}

Write-Host "`n📋 Usage:`n"
Write-Host "   1. Extract $zipName to any location"
Write-Host "   2. Run PDFMerger.exe"
Write-Host "   3. Context menu will register automatically"
Write-Host ""
