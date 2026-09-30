using System.IO;
using System.Windows;
using KeyForge.App.Services;

namespace KeyForge.App;

public partial class MainWindow : Window
{
    private readonly ActionManager _actionManager;
    private readonly FunctionRegistry _functionRegistry;
    private readonly KeyboardHook _keyboardHook;
    private readonly HotkeyManager _hotkeyManager;
    private readonly TrayManager _trayManager;

    public MainWindow()
    {
        InitializeComponent();
        Closing += MainWindow_Closing;


        var functionLoader = new FunctionLoader();

        _actionManager = new ActionManager(
            functionLoader
        );

        _functionRegistry = new FunctionRegistry(
            _actionManager
        );
        var configManager = new ConfigManager();
        string configPath = Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory)!
                .Parent!
                .Parent!
                .Parent!
                .Parent!
                .FullName,
            "config",
            "keybinds.json"
        );
        var config = configManager.Load(configPath);
        _keyboardHook = new KeyboardHook();
        _hotkeyManager = new HotkeyManager(
            _actionManager,
            _keyboardHook,
            config
        );

        _keyboardHook.Start();
        string functionsDirectory = GetFunctionsDirectory();

        _functionRegistry.LoadFunctions(functionsDirectory);
        _trayManager = new TrayManager(this);
    }
    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
    private string GetFunctionsDirectory()
    {
        return Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory)!
                .Parent!
                .Parent!
                .Parent!
                .Parent!
                .FullName,
            "Functions"
        );
    }

    private void TestFunction_Click(
    object sender,
    RoutedEventArgs e)
    {
        try
        {
            var configManager = new ConfigManager();

            string configPath = Path.Combine(
                Directory.GetParent(AppContext.BaseDirectory)!
                    .Parent!
                    .Parent!
                    .Parent!
                    .Parent!
                    .FullName,
                "config",
                "keybinds.json"
            );

            var config = configManager.Load(configPath);

            string result = string.Join(
                "\n",
                config.Keybinds.Select(
                    keybind =>
                        $"{keybind.Keys} → {keybind.Action}"
                )
            );

            System.Windows.MessageBox.Show(
                result,
                "KeyForge Config"
            );
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(
                ex.ToString(),
                "KeyForge Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}