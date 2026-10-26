using KeyForge.SDK;

namespace KeyForge.App.Services;

public class ActionManager
{
    private readonly FunctionLoader _functionLoader;

    private readonly Dictionary<string, IKeyForgeAction> _actions = new();

    public ActionManager(FunctionLoader functionLoader)
    {
        _functionLoader = functionLoader;
    }

    public void LoadAction(string name, string filePath)
    {
        var action = _functionLoader.Load(filePath);

        if (action == null)
        {
            throw new Exception(
                $"Could not load action '{name}'."
            );
        }

        _actions[name] = action;
    }
    public void ClearActions()
    {
        _actions.Clear();
    }

    public void Execute(string name)
    {
        if (!_actions.TryGetValue(name, out var action))
        {
            throw new Exception(
                $"Action '{name}' was not found."
            );
        }

        action.Execute();
    }
    public IReadOnlyCollection<string> GetActionNames()
    {
        return _actions.Keys;
    }
}