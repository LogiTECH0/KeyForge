using Forms = System.Windows.Forms;

namespace KeyForge.App.Services;

public class TrayManager
{
    private readonly Forms.NotifyIcon trayIcon;
    private readonly Forms.ContextMenuStrip menu;
    private readonly MainWindow mainWindow;

    public TrayManager(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;

        trayIcon = new Forms.NotifyIcon();
        menu = new Forms.ContextMenuStrip();

        var icon = System.Drawing.Icon.ExtractAssociatedIcon("Group 307.ico");

        if (icon != null)
        {
            trayIcon.Icon = icon;
        }

        trayIcon.Visible = true;
        trayIcon.Text = "KeyForge";
        trayIcon.ContextMenuStrip = menu;

        var settingsItem = new Forms.ToolStripMenuItem("Settings");
        var exitItem = new Forms.ToolStripMenuItem("Exit");

        exitItem.Click += (sender, e) =>
            System.Windows.Application.Current.Shutdown();

        settingsItem.Click += (sender, e) =>
        {
            mainWindow.Show();
            mainWindow.Activate();
        };

        menu.Items.Add(settingsItem);
        menu.Items.Add(exitItem);
    }
}