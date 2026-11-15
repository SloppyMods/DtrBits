using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace DtrBits.Plugin;

public sealed class Plugin : IDalamudPlugin
{
    private const string CommandName = "/dtrbits";
    private readonly IDalamudPluginInterface pluginInterface;
    private readonly ICommandManager commands;
    private readonly WindowSystem windows = new("DtrBits");
    private readonly ConfigWindow configWindow;
    private readonly GearsetWindow gearsetWindow;
    private readonly ModuleHost host;

    public Plugin(IDalamudPluginInterface pluginInterface, ICommandManager commands, IDtrBar dtr, IFramework framework,
        IDataManager data, IObjectTable objects, IPluginLog log)
    {
        this.pluginInterface = pluginInterface;
        this.commands = commands;
        Config config;
        try { config = pluginInterface.GetPluginConfig() as Config ?? new Config(); }
        catch (Exception ex)
        {
            log.Warning(ex, "DTR Bits: unreadable settings, using defaults.");
            config = new Config();
        }
        configWindow = new ConfigWindow(config, () => pluginInterface.SavePluginConfig(config));
        gearsetWindow = new GearsetWindow(data, objects);
        windows.AddWindow(configWindow);
        windows.AddWindow(gearsetWindow);

        var actions = new GameActions(data, log);
        // To add an entry: write a DtrModule subclass and add it to this list.
        DtrModule[] modules = [new JobModule(objects, gearsetWindow), new RepairModule(actions), new SpiritbondModule(actions)];
        host = new ModuleHost(dtr, framework, log, config, modules);

        commands.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "DTR Bits: /dtrbits opens settings, /dtrbits gearsets toggles the gearset window.",
        });
        pluginInterface.UiBuilder.Draw += windows.Draw;
        pluginInterface.UiBuilder.OpenConfigUi += configWindow.Toggle;
    }

    private void OnCommand(string command, string args)
    {
        if (args.Trim().Equals("gearsets", StringComparison.OrdinalIgnoreCase)) gearsetWindow.Toggle();
        else configWindow.Toggle();
    }

    public void Dispose()
    {
        commands.RemoveHandler(CommandName);
        pluginInterface.UiBuilder.Draw -= windows.Draw;
        pluginInterface.UiBuilder.OpenConfigUi -= configWindow.Toggle;
        windows.RemoveAllWindows();
        host.Dispose();
    }
}
