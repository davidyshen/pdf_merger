using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Dispatching;

namespace PDFMerger;

public class WindowsSystemDispatcherQueueHelper
{
    [StructLayout(LayoutKind.Sequential)]
    struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out] ref IntPtr dispatcherQueueController);

    object? m_dispatcherQueueController = null;
    public void EnsureWindowsSystemDispatcherQueueServer()
    {
        if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
        {
            // one already exists, so we'll just use it.
            return;
        }

        if (m_dispatcherQueueController == null)
        {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2;    // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

            IntPtr dispatcherQueueController = IntPtr.Zero;
            CreateDispatcherQueueController(options, ref dispatcherQueueController);
            m_dispatcherQueueController = dispatcherQueueController;
        }
    }
}