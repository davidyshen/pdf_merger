using Microsoft.Win32;
using System.IO;

namespace PDFMerger.Services;

public class ContextMenuService
{
    private const string ContextMenuPath = @"Software\Classes\*\shell\MergePDFs";
    private const string CommandPath = @"Software\Classes\*\shell\MergePDFs\command";
    private const string ApplicationName = "PDFMerger";

    /// <summary>
    /// Registers the "Merge PDFs" context menu option in Windows Explorer
    /// </summary>
    public static bool RegisterContextMenu(string applicationPath)
    {
        try
        {
            using (var key = Registry.CurrentUser.CreateSubKey(ContextMenuPath))
            {
                key?.SetValue(null, "Merge PDFs");
                key?.SetValue("Icon", applicationPath);
                key?.SetValue("MUIVerb", "Merge PDFs");
                key?.SetValue("SubCommands", "");
            }

            using (var key = Registry.CurrentUser.CreateSubKey(CommandPath))
            {
                var commandValue = $"\"{applicationPath}\" \"%1\"";
                key?.SetValue(null, commandValue);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Unregisters the context menu option
    /// </summary>
    public static bool UnregisterContextMenu()
    {
        try
        {
            Registry.CurrentUser.DeleteSubKeyTree(ContextMenuPath, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if context menu is registered
    /// </summary>
    public static bool IsContextMenuRegistered()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(ContextMenuPath);
            return key != null;
        }
        catch
        {
            return false;
        }
    }
}
