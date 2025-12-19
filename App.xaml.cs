using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using PDFMerger.Views;

namespace PDFMerger;

public partial class App : Application
{
    private List<string> _pendingFiles = new List<string>();
    private Window? m_window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var mainWindow = new MainWindow();
        m_window = mainWindow;

        // Handle any files that were redirected or the initial ones before the window was ready
        lock (_pendingFiles)
        {
            if (_pendingFiles.Count > 0)
            {
                mainWindow.LoadPDFFilesPublic(_pendingFiles);
                _pendingFiles.Clear();
            }
        }

        mainWindow.Activate();
    }

    public void HandleSecondaryActivation(AppActivationArguments args)
    {
        var paths = ExtractPaths(args);
        HandlePaths(paths);
    }

    public void HandlePaths(IEnumerable<string> paths)
    {
        var validPaths = paths.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
        if (validPaths.Count == 0) return;

        if (m_window is MainWindow mainWindow)
        {
            mainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                mainWindow.Activate();
                mainWindow.LoadPDFFilesPublic(validPaths);
            });
        }
        else
        {
            // Window not ready yet, store for OnLaunched
            lock (_pendingFiles)
            {
                _pendingFiles.AddRange(validPaths);
            }
        }
    }

    private List<string> ExtractPaths(AppActivationArguments args)
    {
        var paths = new List<string>();

        if (args.Kind == ExtendedActivationKind.Launch)
        {
            string? arguments = null;

            if (args.Data is Windows.ApplicationModel.Activation.ILaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;
            }
            else if (args.Data is Microsoft.UI.Xaml.LaunchActivatedEventArgs winuiLaunchArgs)
            {
                arguments = winuiLaunchArgs.Arguments;
            }

            if (!string.IsNullOrEmpty(arguments))
            {
                // The arguments might contain multiple paths if passed as a single string
                // or just one path. We'll try to split by quotes or spaces.
                // For now, let's handle the common case where it's a single quoted path.
                string arg = arguments.Trim().Trim('\"');
                if (!string.IsNullOrEmpty(arg))
                {
                    paths.Add(arg);
                }
            }
        }
        else if (args.Kind == ExtendedActivationKind.File)
        {
            var fileArgs = args.Data as Windows.ApplicationModel.Activation.IFileActivatedEventArgs;
            if (fileArgs != null && fileArgs.Files != null)
            {
                foreach (var file in fileArgs.Files)
                {
                    if (!string.IsNullOrEmpty(file.Path))
                    {
                        paths.Add(file.Path);
                    }
                }
            }
        }

        return paths;
    }
}

