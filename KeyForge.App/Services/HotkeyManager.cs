using System.Windows.Input;
using KeyForge.App.Models;

namespace KeyForge.App.Services;

public class HotkeyManager
{
    private readonly HashSet<string> _pressedKeys = new();
    private readonly HashSet<string> _currentCombination = new();
    private readonly ActionManager _actionManager;
    private readonly KeyboardHook _keyboardHook;
    private readonly KeyForgeConfig _config;

    public HotkeyManager(ActionManager actionManager, KeyboardHook keyboardHook, KeyForgeConfig config)
    {
        _actionManager = actionManager;
        _keyboardHook = keyboardHook;
        _config = config;
        keyboardHook.KeyDown += OnKeyDown;
        keyboardHook.KeyUp += OnKeyUp;
    }
    public void OnKeyDown(Key key)
    {
        string keyName = key.ToString();
        Console.WriteLine($"DOWN: {key}");
        _pressedKeys.Add(keyName);
        _currentCombination.Add(keyName);
    }
    public void OnKeyUp(Key key)
    {
        string keyName = key.ToString();
        _pressedKeys.Remove(keyName);
        Console.WriteLine($"UP: {key}");
        if (_pressedKeys.Count == 0 && _currentCombination.Count > 0)
        {
            string combination = string.Join(",", _currentCombination.OrderBy(s => s));
            Console.WriteLine($"Current Combination: {_currentCombination}");
            Console.WriteLine($"Combination: {combination}");
            var hotkey = _config.Keybinds.FirstOrDefault(k => k.Keys == combination);
            try
            {
                _actionManager.Execute(hotkey.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"KeyForge action error: {ex.Message}");
            }
            _currentCombination.Clear();
        }
    }

}
