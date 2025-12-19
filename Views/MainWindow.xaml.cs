using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PDFMerger.Services;
using PDFMerger.ViewModels;
using WinRT;

namespace PDFMerger.Views;

public sealed partial class MainWindow : Window
{
    private MainWindowViewModel _viewModel;
    private List<string> _selectedFiles = new();
    private string? _baseDirectory;
    private WindowsSystemDispatcherQueueHelper? _wsdqHelper;
    private Microsoft.UI.Composition.SystemBackdrops.MicaController? _micaController;
    private Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration? _configurationSource;

    public MainWindow()
    {
        this.InitializeComponent();
        _viewModel = new MainWindowViewModel();

        TrySetMicaBackdrop();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
    }

    private void TrySetMicaBackdrop()
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueServer();

            _configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            this.Activated += Window_Activated;
            this.Closed += Window_Closed;
            ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

            _configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            _micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();
            _micaController.Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.Base;

            _micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            _micaController.SetSystemBackdropConfiguration(_configurationSource);
        }
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (_configurationSource != null)
        {
            _configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        if (_micaController != null)
        {
            _micaController.Dispose();
            _micaController = null;
        }
        this.Activated -= Window_Activated;
        _configurationSource = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (_configurationSource != null)
        {
            SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        if (_configurationSource != null)
        {
            switch (((FrameworkElement)this.Content).ActualTheme)
            {
                case ElementTheme.Dark: _configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: _configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case ElementTheme.Default: _configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }
    }

    // Public method for App.xaml.cs to call with command-line arguments
    public void LoadPDFFilesPublic(List<string> filePaths)
    {
        LoadPDFFiles(filePaths);
    }

    private async void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new Windows.Storage.Pickers.FolderPicker();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        picker.FileTypeFilter.Add("*");

        var folder = await picker.PickSingleFolderAsync();
        if (folder != null)
        {
            _baseDirectory = folder.Path;
        }
    }

    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is PDFItem item)
        {
            _viewModel.MoveUp(item);
        }
    }

    private void MoveDown_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is PDFItem item)
        {
            _viewModel.MoveDown(item);
        }
    }

    private void RemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is PDFItem item)
        {
            _selectedFiles.Remove(item.FilePath);
            _viewModel.PDFFiles.Remove(item);
        }
    }

    private void LoadPDFFiles(List<string> filePaths)
    {
        foreach (var rawPath in filePaths)
        {
            if (string.IsNullOrWhiteSpace(rawPath)) continue;

            // Clean up the path (remove quotes and trim)
            var filePath = rawPath.Trim().Trim('\"');

            if (filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && File.Exists(filePath))
            {
                if (_baseDirectory == null)
                {
                    _baseDirectory = Path.GetDirectoryName(filePath);
                }

                AddPdfItem(filePath);
            }
        }
    }

    private void AddPdfItem(string filePath)
    {
        if (_selectedFiles.Contains(filePath)) return;

        _selectedFiles.Add(filePath);
        var item = new PDFItem
        {
            FilePath = filePath,
            FileName = Path.GetFileName(filePath)
        };
        _viewModel.PDFFiles.Add(item);

        // Generate thumbnail in background without blocking the UI thread
        Task.Run(async () =>
        {
            try
            {
                var thumbnail = await PDFService.GenerateThumbnailAsync(filePath);
                if (thumbnail != null)
                {
                    // Update the item on the UI thread
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        item.ThumbnailData = thumbnail;
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to generate thumbnail for {filePath}: {ex.Message}");
            }
        });
    }

    private async void AddFilesButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new Windows.Storage.Pickers.FileOpenPicker();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeFilter.Add(".pdf");
        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

        var files = await picker.PickMultipleFilesAsync();
        foreach (var file in files)
        {
            if (_baseDirectory == null)
            {
                _baseDirectory = Path.GetDirectoryName(file.Path);
            }

            AddPdfItem(file.Path);
        }
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
        _selectedFiles.Clear();
        _viewModel.PDFFiles.Clear();
    }

    private async void MergeButton_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.PDFFiles.Count < 2)
        {
            await ShowDialog("Error", "Please select at least 2 PDF files to merge.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_baseDirectory))
        {
            await ShowDialog("Error", "Could not determine the output directory.");
            return;
        }

        string fileName = _viewModel.OutputFileName.Trim();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = "merged";
        }

        if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            fileName += ".pdf";
        }

        string outputPath = Path.Combine(_baseDirectory, fileName);

        try
        {
            var filePaths = _viewModel.PDFFiles.Select(p => p.FilePath).ToList();
            var result = await PDFService.MergePDFsAsync(filePaths, outputPath);

            if (result.Success)
            {
                await ShowDialog("Success", $"PDFs merged successfully!\n\nSaved to: {outputPath}");
                this.Close();
            }
            else
            {
                await ShowDialog("Error", $"Failed to merge PDFs.\n\nDetails: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            await ShowDialog("Error", $"Failed to merge: {ex.Message}");
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private async Task ShowDialog(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.Content.XamlRoot
        };

        await dialog.ShowAsync();
    }
}
