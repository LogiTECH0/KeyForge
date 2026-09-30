using System.IO;

namespace KeyForge.App.Services;

public class FunctionRegistry
{
    private readonly ActionManager _actionManager;

    public FunctionRegistry(ActionManager actionManager)
    {
        _actionManager = actionManager;
    }

    public void LoadFunctions(string functionsDirectory)
    {
        if (!Directory.Exists(functionsDirectory))
        {
            throw new DirectoryNotFoundException(
                $"Functions directory not found: {functionsDirectory}"
            );
        }

        var files = Directory.GetFiles(
            functionsDirectory,
            "*.cs"
        );

        foreach (var file in files)
        {
            string name = Path.GetFileNameWithoutExtension(file);

            _actionManager.LoadAction(
                name,
                file
            );
        }
    }
}