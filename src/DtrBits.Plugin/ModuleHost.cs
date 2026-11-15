using System.Diagnostics;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Plugin.Services;

namespace DtrBits.Plugin;

internal sealed class ModuleHost : IDisposable
{
    private const long RefreshMs = 500;
    private readonly IDtrBar dtr;
    private readonly IFramework framework;
    private readonly IPluginLog log;
    private readonly Config config;
    private readonly List<(DtrModule Module, IDtrBarEntry Entry)> entries = [];
    private readonly Stopwatch clock = Stopwatch.StartNew();
    private long lastRefresh;

    public ModuleHost(IDtrBar dtr, IFramework framework, IPluginLog log, Config config, IEnumerable<DtrModule> modules)
    {
        this.dtr = dtr; this.framework = framework; this.log = log; this.config = config;
        foreach (var module in modules)
        {
            var entry = dtr.Get(module.Title);
            entry.Shown = false;
            entry.OnClick = module.OnClick;
            entries.Add((module, entry));
        }
        framework.Update += OnUpdate;
    }

    private void OnUpdate(IFramework _)
    {
        if (clock.ElapsedMilliseconds - lastRefresh < RefreshMs) return;
        lastRefresh = clock.ElapsedMilliseconds;
        foreach (var (module, entry) in entries)
        {
            try
            {
                var text = module.IsEnabled(config) ? module.Text(config) : null;
                entry.Shown = text is not null;
                if (text is null) continue;
                entry.Text = text;
                if (module.Tooltip is { } tip) entry.Tooltip = tip;
            }
            catch (Exception ex)
            {
                entry.Shown = false;
                log.Warning(ex, "DTR Bits: {0} failed to update", module.Title);
            }
        }
    }

    public void Dispose()
    {
        framework.Update -= OnUpdate;
        foreach (var (_, entry) in entries) entry.Remove();
        entries.Clear();
    }
}
