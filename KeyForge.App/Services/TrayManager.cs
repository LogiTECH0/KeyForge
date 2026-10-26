using System.Windows;
using Forms = System.Windows.Forms;

namespace KeyForge.App.Services;

public class TrayManager
{
    private readonly Forms.NotifyIcon trayIcon;
    private readonly Forms.ContextMenuStrip menu;
    private readonly MainWindow mainWindow;
    private readonly FunctionRegistry functionRegistry;
    private readonly string functionsDirectory;

    public TrayManager(MainWindow mainWindow, FunctionRegistry functionRegistry, string functionsDirectory)
    {
        this.mainWindow = mainWindow;
        this.functionRegistry = functionRegistry;
        this.functionsDirectory = functionsDirectory;
        trayIcon = new Forms.NotifyIcon();
        menu = new Forms.ContextMenuStrip();

        var icon = System.Drawing.Icon.ExtractAssociatedIcon("kf-icon.ico");

        if (icon != null)
        {
            trayIcon.Icon = icon;
        }

        trayIcon.Visible = true;
        trayIcon.Text = "KeyForge";
        trayIcon.ContextMenuStrip = menu;

        var titleItem = new Forms.ToolStripMenuItem("KeyForge");
        titleItem.Enabled = false;
        var settingsItem = new Forms.ToolStripMenuItem("Settings");
        var exitItem = new Forms.ToolStripMenuItem("Exit");
        var reloadItem = new Forms.ToolStripMenuItem("Reload Functions");
        var separator = new Forms.ToolStripSeparator();
        var separator2 = new Forms.ToolStripSeparator();

        exitItem.Click += (sender, e) =>
            System.Windows.Application.Current.Shutdown();

        reloadItem.Click += (sender, e) =>
        {
            try
            {
                functionRegistry.LoadFunctions(functionsDirectory);
                trayIcon.ShowBalloonTip(
                    2000,
                    "KeyForge",
                    "Functions reloaded successfully.",
                    Forms.ToolTipIcon.Info
                );
            }
            catch (Exception ex)
            {
                trayIcon.ShowBalloonTip(
                    3000,
                    "KeyForge Error",
                    ex.Message,
                    Forms.ToolTipIcon.Error
                );
            }
        };

        settingsItem.Click += (sender, e) =>
        {
            mainWindow.Show();
            mainWindow.Activate();
        };

        menu.Items.Add(titleItem);
        menu.Items.Add(separator);
        menu.Items.Add(settingsItem);
        menu.Items.Add(reloadItem);
        menu.Items.Add(separator2);
        menu.Items.Add(exitItem);
    }
}