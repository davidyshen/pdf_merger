using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace PDFMerger.Services;

public class PDFService
{
    /// <summary>
    /// Generates a thumbnail of the first page of a PDF file
    /// </summary>
    public static async Task<byte[]?> GenerateThumbnailAsync(string filePath, uint width = 200)
    {
        try
        {
            if (!File.Exists(filePath)) return null;

            // Use FileStream to avoid StorageFile permission issues in unpackaged apps
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var randomAccessStream = fileStream.AsRandomAccessStream();

            var pdfDoc = await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(randomAccessStream);

            if (pdfDoc.PageCount == 0) return null;

            using var page = pdfDoc.GetPage(0);

            using var outputStream = new InMemoryRandomAccessStream();

            var options = new PdfPageRenderOptions
            {
                DestinationWidth = width
            };

            await page.RenderToStreamAsync(outputStream, options);

            if (outputStream.Size == 0) return null;

            var buffer = new byte[outputStream.Size];
            outputStream.Seek(0);
            await outputStream.ReadAsync(buffer.AsBuffer(), (uint)outputStream.Size, InputStreamOptions.None);

            return buffer;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Thumbnail error for {filePath}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Merges multiple PDF files into a single file
    /// </summary>
    public static async Task<(bool Success, string ErrorMessage)> MergePDFsAsync(List<string> filePaths, string outputPath)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Ensure the output directory exists
                var directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var writer = new PdfWriter(outputPath))
                {
                    writer.SetSmartMode(true);
                    using (var outputDocument = new iText.Kernel.Pdf.PdfDocument(writer))
                    {
                        var merger = new PdfMerger(outputDocument);

                        foreach (var filePath in filePaths)
                        {
                            if (!File.Exists(filePath))
                            {
                                throw new FileNotFoundException($"Source file not found: {filePath}");
                            }

                            try
                            {
                                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    using (var reader = new PdfReader(fs))
                                    {
                                        using (var sourceDocument = new iText.Kernel.Pdf.PdfDocument(reader))
                                        {
                                            merger.Merge(sourceDocument, 1, sourceDocument.GetNumberOfPages());
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Error in '{Path.GetFileName(filePath)}': {ex.Message}", ex);
                            }
                        }

                        merger.Close();
                        outputDocument.Close();
                    }
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null && !message.Contains(ex.InnerException.Message))
                {
                    message += $" ({ex.InnerException.Message})";
                }

                System.Diagnostics.Debug.WriteLine($"Merge Error: {ex}");
                return (false, message);
            }
        });
    }

    /// <summary>
    /// Generates a default filename if none is provided
    /// </summary>
    public static string GenerateDefaultFileName()
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        return $"merged_pdf_{timestamp}.pdf";
    }

    /// <summary>
    /// Validates that a file is a valid PDF
    /// </summary>
    public static bool IsValidPDF(string filePath)
    {
        try
        {
            using var pdf = new iText.Kernel.Pdf.PdfDocument(new PdfReader(filePath));
            return pdf.GetNumberOfPages() > 0;
        }
        catch
        {
            return false;
        }
    }
}
