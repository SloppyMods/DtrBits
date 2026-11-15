using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace DtrBits.Plugin;

internal abstract class DtrModule(string title)
{
    public string Title { get; } = title;
    public abstract bool IsEnabled(Config config);
    public abstract SeString? Text(Config config);
    public virtual string? Tooltip => null;
    public abstract void OnClick(DtrInteractionEvent e);

    protected static SeString Colored(string text, ushort color) =>
        color == 0 ? new SeString(new TextPayload(text)) : new SeStringBuilder().AddUiForeground(text, color).Build();
}
