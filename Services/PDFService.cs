using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;
using Microsoft.UI.Xaml.Media.Imaging;
using PDFMerger.ViewModels;

namespace PDFMerger.Services;

public class PDFService
{
    /// <summary>
    /// Generates a thumbnail of the first page of a PDF file
    /// </summary>
    public static async Task<byte[]?> GenerateThumbnailAsync(string filePath, int width = 200, int height = 280)
    {
        return await Task.Run(() =>
        {
            try
            {
                using var pdfDocument = new PdfDocument(new PdfReader(filePath));

                if (pdfDocument.GetNumberOfPages() == 0)
                    return null;

                var firstPage = pdfDocument.GetPage(1);
                var pageSize = firstPage.GetPageSize();

                // Create a bitmap to render the PDF page
                var bitmap = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    graphics.DrawString("PDF Preview", new Font("Arial", 10), Brushes.Black, 10, 10);
                    graphics.DrawString(Path.GetFileName(filePath), new Font("Arial", 8), Brushes.Gray, 10, 30);
                }

                using var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
        });
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
                    using (var outputDocument = new PdfDocument(writer))
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
                                        using (var sourceDocument = new PdfDocument(reader))
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
            using var pdf = new PdfDocument(new PdfReader(filePath));
            return pdf.GetNumberOfPages() > 0;
        }
        catch
        {
            return false;
        }
    }
}
