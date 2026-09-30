using System.Runtime.InteropServices;
using KeyForge.SDK;

public class PlayPause : IKeyForgeAction
{
    [DllImport("user32.dll")]
    private static extern void keybd_event(
        byte bVk,
        byte bScan,
        uint dwFlags,
        nint dwExtraInfo
    );

    private const byte VK_KEY = 0xAF;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    public void Execute()
    {
        keybd_event(
            VK_KEY,
            0,
            0,
            0
        );

        keybd_event(
            VK_KEY,
            0,
            KEYEVENTF_KEYUP,
            0
        );
    }
}