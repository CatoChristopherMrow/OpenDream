using OpenDreamRuntime.Procs;
using OpenDreamShared.Dream;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = Robust.Shared.Maths.Color;

namespace OpenDreamRuntime.Objects.Types;

public sealed class DreamObjectIcon : DreamObject {
    public DreamIcon Icon;

    public DreamObjectIcon(DreamObjectDefinition objectDefinition) : base(objectDefinition) {
        Icon = new(DreamManager, DreamResourceManager);
    }

    public override void Initialize(DreamProcArguments args) {
        base.Initialize(args);

        // TODO confirm BYOND behavior of invalid args for icon, dir, and frame
        DreamValue icon = args.GetArgument(0);
        DreamValue state = args.GetArgument(1);
        DreamValue dir = args.GetArgument(2);
        DreamValue frame = args.GetArgument(3);
        DreamValue moving = args.GetArgument(4);

        if (!icon.IsNull) {
            if (icon.TryGetValueAsDreamObject<DreamObjectIcon>(out var iconObj)) {
                if (state.IsNull && dir.IsNull && frame.IsNull && moving.IsNull) {
                    // Copy the DreamIcon rather than create the entire DMI from it
                    Icon.CopyFrom(iconObj.Icon);
                } else {
                    Icon.InsertStates(iconObj.Icon.GenerateDMI(), state, dir, frame, isConstructor: true);
                }
            } else {
                if (!DreamResourceManager.TryLoadIcon(icon, out var iconRsc))
                    throw new Exception($"Cannot create an icon from {icon}");

                Icon.InsertStates(iconRsc, state, dir, frame, isConstructor: true);
            }
        }
    }

    protected override bool TryGetVar(string varName, out DreamValue value) {
        switch (varName) {
            case "icon":
                // TODO: Figure out what this gives you (whatever ref ID 0xC is)
                value = DreamValue.Null;
                return true;
            default:
                return base.TryGetVar(varName, out value);
        }
    }

    protected override void SetVar(string varName, DreamValue value) {
        switch (varName) {
            case "icon":
                break;
            default:
                base.SetVar(varName, value);
                break;
        }
    }

    public DreamObjectIcon Clone() {
        var newIcon = new DreamObjectIcon(ObjectDefinition) {
            Icon = new(DreamManager, DreamResourceManager)
        };
        newIcon.Icon.CopyFrom(Icon);

        return newIcon;
    }

    public void Turn(float angle) {
        Icon.MutateFrames(image => image.Mutate(context => context.Rotate(angle)));
    }

    public void Flip(AtomDirection dir) {
        Icon.MutateFrames(image => image.Mutate(context => {
            if ((dir & AtomDirection.East) != 0 || (dir & AtomDirection.West) != 0)
                context.Flip(FlipMode.Horizontal);

            if ((dir & AtomDirection.North) != 0 || (dir & AtomDirection.South) != 0)
                context.Flip(FlipMode.Vertical);
        }));
    }

    public void Crop(int x1, int y1, int x2, int y2) {
        Icon.Crop(x1, y1, x2, y2);
    }

    public void Shift(AtomDirection dir, int offset, bool wrap) {
        Icon.Shift(dir, offset, wrap);
    }

    public void DrawBox(Color color, int x1, int y1, int x2, int y2) {
        Icon.DrawBox(new Rgba32(color.RByte, color.GByte, color.BByte, color.AByte), x1, y1, x2, y2);
    }

    public void SwapColor(Color oldColor, Color newColor) {
        Icon.SwapColor(new Rgba32(oldColor.RByte, oldColor.GByte, oldColor.BByte, oldColor.AByte), new Rgba32(newColor.RByte, newColor.GByte, newColor.BByte, newColor.AByte));
    }

    public void SetIntensity(float r, float g, float b) {
        Icon.SetIntensity(r, g, b);
    }

    public void MapColors(Dictionary<Color, Color> colorMap) {
        Dictionary<Rgba32, Rgba32> rgbaMap = new(colorMap.Count);
        foreach (var (from, to) in colorMap) {
            rgbaMap[new Rgba32(from.RByte, from.GByte, from.BByte, from.AByte)] =
                new Rgba32(to.RByte, to.GByte, to.BByte, to.AByte);
        }

        Icon.MapColors(rgbaMap);
    }

    public DreamValue GetPixel(int x, int y, DreamValue state, DreamValue dir, DreamValue frame) {
        return Icon.GetPixel(x, y, state, dir, frame);
    }
}
