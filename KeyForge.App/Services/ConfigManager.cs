using System.IO;
using System.Text.Json;
using KeyForge.App.Models;

namespace KeyForge.App.Services;

public class ConfigManager
{
    public KeyForgeConfig Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Config file '{filePath}' does not exist.");
        }

        string json = File.ReadAllText(filePath);
        var config = JsonSerializer.Deserialize<KeyForgeConfig>(json);

        if (config == null)
        {
            throw new Exception($"Failed to load KeyForge configuration.");
        }
        return config;
    }
}