using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Dispatching;

namespace PDFMerger.Services;

/// <summary>
/// Windows native API interop for dispatcher queue and window management
/// </summary>
public class WindowsInterop
{
    // Dispatcher Queue Interop
    [StructLayout(LayoutKind.Sequential)]
    private struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out] ref IntPtr dispatcherQueueController);

    private object? _dispatcherQueueController = null;

    /// <summary>
    /// Ensures a dispatcher queue exists for the current thread
    /// </summary>
    public void EnsureWindowsSystemDispatcherQueueServer()
    {
        if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
        {
            return; // One already exists
        }

        if (_dispatcherQueueController == null)
        {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2;    // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

            IntPtr dispatcherQueueController = IntPtr.Zero;
            CreateDispatcherQueueController(options, ref dispatcherQueueController);
            _dispatcherQueueController = dispatcherQueueController;
        }
    }

    // Window Management Interop
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentProcessId();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    /// <summary>
    /// Brings a window to the foreground
    /// </summary>
    public static void BringWindowToForeground(IntPtr hWnd) => SetForegroundWindow(hWnd);

    /// <summary>
    /// Finds a window by class name and title
    /// </summary>
    public static IntPtr FindWindowByName(string? lpClassName, string? lpWindowName) => FindWindow(lpClassName, lpWindowName);

    /// <summary>
    /// Gets the current process ID
    /// </summary>
    public static uint GetCurrentProcId() => GetCurrentProcessId();

    /// <summary>
    /// Gets the process ID of a window
    /// </summary>
    public static uint GetWindowProcId(IntPtr hWnd)
    {
        GetWindowThreadProcessId(hWnd, out uint procId);
        return procId;
    }
}
