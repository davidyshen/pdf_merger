using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace PDFMerger.Services;

public static class ShellService
{
    private const string KeyPath = @"Software\Classes\*\shell\MergePDFs";

    public static void RegisterContextMenu()
    {
        if (IsPackaged()) return;

        try
        {
            string exePath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
            if (string.IsNullOrEmpty(exePath)) return;

            // Register for all files (*)
            // Note: We use CurrentUser so we don't need Admin privileges
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(KeyPath))
            {
                key.SetValue("", "Merge PDFs");
                key.SetValue("Icon", exePath);

                // Add a hint that this is for PDF files (optional, but helps)
                // key.SetValue("AppliesTo", ".pdf"); 

                using (RegistryKey commandKey = key.CreateSubKey("command"))
                {
                    commandKey.SetValue("", $"\"{exePath}\" \"%1\"");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to register context menu: {ex.Message}");
        }
    }

    public static bool IsPackaged()
    {
        try
        {
            // Check if we have a package identity
            return Windows.ApplicationModel.Package.Current.Id != null;
        }
        catch
        {
            return false;
        }
    }

    public static void UnregisterContextMenu()
    {
        try
        {
            Registry.CurrentUser.DeleteSubKeyTree(KeyPath, false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to unregister context menu: {ex.Message}");
        }
    }

    public static bool IsRegistered()
    {
        try
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(KeyPath))
            {
                if (key == null) return false;

                using (RegistryKey? commandKey = key.OpenSubKey("command"))
                {
                    if (commandKey == null) return false;

                    string? value = commandKey.GetValue("") as string;
                    string exePath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;

                    return !string.IsNullOrEmpty(value) && value.Contains(exePath, StringComparison.OrdinalIgnoreCase);
                }
            }
        }
        catch
        {
            return false;
        }
    }
}
