using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Utility;

namespace OpenDreamClient.Interface.Html.Tags;

/// <summary>
/// Modified version of RobustToolbox's font tag
/// Supports text color and uses font sizes that better match BYOND's
/// </summary>
[UsedImplicitly]
public sealed partial class TagFont : IMarkupTagHandler {
    [Dependency] private IResourceCache _resourceCache = default!;

    private static readonly ResPath DefaultFontPath = new("/Fonts/NotoSans-Regular.ttf");

    public string Name => "font"; // Overrides RobustToolbox's font tag

    public void PushDrawContext(MarkupNode node, MarkupDrawingContext context) {
        var font = CreateFont(context.Font, node, _resourceCache);

        node.Attributes.TryGetValue("color", out var colorParameter);
        var textColor = colorParameter.ColorValue ?? context.Color.Peek();

        context.Font.Push(font);
        context.Color.Push(textColor);
    }

    public void PopDrawContext(MarkupNode node, MarkupDrawingContext context) {
        context.Font.Pop();
        context.Color.Pop();
    }

    private static Font CreateFont(
        Stack<Font> contextFontStack,
        MarkupNode node,
        IResourceCache cache) {
        var size = 1;

        if (contextFontStack.TryPeek(out var previousFont)) {
            switch (previousFont) {
                case VectorFont vectorFont:
                    size = (vectorFont.Size - 8) / 2;
                    break;
                case StackedFont stackedFont:
                    if (stackedFont.Stack.Length == 0 || stackedFont.Stack[0] is not VectorFont stackVectorFont)
                        break;

                    size = (stackVectorFont.Size - 8) / 2;
                    break;
            }
        }

        if (node.Attributes.TryGetValue("size", out var sizeParameter))
            size = (int)(sizeParameter.LongValue ?? size);

        size = Math.Max(size, 1);

        // TODO: Support fonts other than the default
        var fontResource = cache.GetResource<FontResource>(DefaultFontPath);
        return new VectorFont(fontResource, size * 2 + 8); // This gives a font size close enough to BYOND's
    }
}
