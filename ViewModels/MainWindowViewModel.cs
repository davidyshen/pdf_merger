using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PDFMerger.ViewModels;

public class PDFItem : INotifyPropertyChanged
{
    private string _filePath = string.Empty;
    private byte[]? _thumbnailData;
    private string _fileName = string.Empty;

    public string FilePath
    {
        get => _filePath;
        set => SetProperty(ref _filePath, value);
    }

    public byte[]? ThumbnailData
    {
        get => _thumbnailData;
        set => SetProperty(ref _thumbnailData, value);
    }

    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

public class MainWindowViewModel : INotifyPropertyChanged
{
    private string _outputFileName = "merged";
    private bool _isMerging = false;
    private ObservableCollection<PDFItem> _pdfFiles = new();

    public ObservableCollection<PDFItem> PDFFiles
    {
        get => _pdfFiles;
        set => SetProperty(ref _pdfFiles, value);
    }

    public void MoveUp(PDFItem item)
    {
        int index = _pdfFiles.IndexOf(item);
        if (index > 0)
        {
            _pdfFiles.Move(index, index - 1);
        }
    }

    public void MoveDown(PDFItem item)
    {
        int index = _pdfFiles.IndexOf(item);
        if (index >= 0 && index < _pdfFiles.Count - 1)
        {
            _pdfFiles.Move(index, index + 1);
        }
    }

    public string OutputFileName
    {
        get => _outputFileName;
        set => SetProperty(ref _outputFileName, value);
    }

    public bool IsMerging
    {
        get => _isMerging;
        set => SetProperty(ref _isMerging, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
