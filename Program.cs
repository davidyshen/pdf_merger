using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace PDFMerger;

public class Program
{
    private static List<string> _pathBuffer = new List<string>();
    private static App? _app;
    private const string PipeName = "PDFMergerSingleInstancePipe";

    [STAThread]
    static void Main(string[] args)
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();

        AppInstance keyInstance = AppInstance.FindOrRegisterForKey("PDFMergerSingleInstanceKey");

        if (keyInstance.IsCurrent)
        {
            // Start the pipe server to listen for paths from other instances
            Task.Run(StartPipeServer);

            Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                _app = new App();

                // 1. Process the initial activation for the main instance
                _app.HandlePaths(Environment.GetCommandLineArgs().Skip(1));

                // 2. Process any paths that arrived via pipe while we were starting up
                lock (_pathBuffer)
                {
                    if (_pathBuffer.Count > 0)
                    {
                        _app.HandlePaths(_pathBuffer);
                        _pathBuffer.Clear();
                    }
                }
            });
        }
        else
        {
            // Send our command line arguments to the main instance via pipe
            SendPathsToMainInstance(Environment.GetCommandLineArgs().Skip(1));

            // Also trigger the built-in redirection just in case (though it might be empty)
            AppActivationArguments activationArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
            keyInstance.RedirectActivationToAsync(activationArgs).GetAwaiter().GetResult();
        }
    }

    private static void StartPipeServer()
    {
        while (true)
        {
            try
            {
                using var server = new NamedPipeServerStream(PipeName, PipeDirection.In);
                server.WaitForConnection();

                using var reader = new StreamReader(server, Encoding.UTF8);
                string? path = reader.ReadLine();
                if (!string.IsNullOrEmpty(path))
                {
                    if (_app != null)
                    {
                        _app.HandlePaths(new[] { path });
                    }
                    else
                    {
                        lock (_pathBuffer)
                        {
                            _pathBuffer.Add(path);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore pipe errors and restart listener
                Thread.Sleep(100);
            }
        }
    }

    private static void SendPathsToMainInstance(IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            try
            {
                using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
                client.Connect(500); // Short timeout

                using var writer = new StreamWriter(client, Encoding.UTF8);
                writer.WriteLine(path);
                writer.Flush();
            }
            catch (Exception)
            {
                // Failed to send, nothing we can do
            }
        }
    }
}
