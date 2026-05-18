using OpenDreamRuntime.Objects;
using OpenDreamRuntime.Objects.Types;
using OpenDreamRuntime.Resources;
using OpenDreamShared.Dream;
using BlendType = OpenDreamRuntime.Objects.DreamIconOperationBlend.BlendType;
using DreamValueTypeFlag = OpenDreamRuntime.DreamValue.DreamValueTypeFlag;
using Color = Robust.Shared.Maths.Color;

namespace OpenDreamRuntime.Procs.Native {
    internal static class DreamProcNativeIcon {
        [DreamProc("Width")]
        public static DreamValue NativeProc_Width(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            return new DreamValue(((DreamObjectIcon)src!).Icon.Width);
        }

        [DreamProc("Height")]
        public static DreamValue NativeProc_Height(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            return new DreamValue(((DreamObjectIcon)src!).Icon.Height);
        }

        [DreamProc("Insert")]
        [DreamProcParameter("new_icon", Type = DreamValueTypeFlag.DreamObject)]
        [DreamProcParameter("icon_state", Type = DreamValueTypeFlag.String)]
        [DreamProcParameter("dir", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("frame", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("moving", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("delay", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_Insert(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            //TODO Figure out what happens when you pass the wrong types as args

            DreamValue newIcon = bundle.GetArgument(0, "new_icon");
            DreamValue iconState = bundle.GetArgument(1, "icon_state");
            DreamValue dir = bundle.GetArgument(2, "dir");
            DreamValue frame = bundle.GetArgument(3, "frame");
            DreamValue moving = bundle.GetArgument(4, "moving");
            DreamValue delay = bundle.GetArgument(5, "delay");

            // TODO: moving & delay

            var resourceManager = IoCManager.Resolve<DreamResourceManager>();
            if (!resourceManager.TryLoadIcon(newIcon, out var iconRsc))
                throw new Exception($"Cannot insert {newIcon}");

            ((DreamObjectIcon)src!).Icon.InsertStates(iconRsc, iconState, dir, frame, useDefaultStateAsSource: true); // TODO: moving & delay
            return DreamValue.Null;
        }

        public static void Blend(DreamIcon icon, DreamValue blend, BlendType function, int x, int y) {
            if (blend.TryGetValueAsString(out var colorStr)) {
                if (!ColorHelpers.TryParseColor(colorStr, out var color))
                    throw new Exception($"Invalid color {colorStr}");

                icon.ApplyOperation(new DreamIconOperationBlendColor(function, x, y, color));
            } else {
                icon.ApplyOperation(new DreamIconOperationBlendImage(function, x, y, blend));
            }
        }

        [DreamProc("Blend")]
        [DreamProcParameter("icon", Type = DreamValueTypeFlag.DreamObject)]
        [DreamProcParameter("function", Type = DreamValueTypeFlag.Float, DefaultValue = (int)BlendType.Add)] // ICON_ADD
        [DreamProcParameter("x", Type = DreamValueTypeFlag.Float, DefaultValue = 1)]
        [DreamProcParameter("y", Type = DreamValueTypeFlag.Float, DefaultValue = 1)]
        public static DreamValue NativeProc_Blend(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            //TODO Figure out what happens when you pass the wrong types as args

            DreamValue icon = bundle.GetArgument(0, "icon");
            DreamValue function = bundle.GetArgument(1, "function");

            bundle.GetArgument(2, "x").TryGetValueAsInteger(out var x);
            bundle.GetArgument(3, "y").TryGetValueAsInteger(out var y);

            if (!function.TryGetValueAsInteger(out var functionValue))
                throw new Exception($"Invalid 'function' argument {function}");

            Blend(((DreamObjectIcon)src!).Icon, icon, (BlendType)functionValue, x, y);
            return DreamValue.Null;
        }

        [DreamProc("Scale")]
        [DreamProcParameter("width", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("height", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_Scale(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            //TODO Figure out what happens when you pass the wrong types as args

            bundle.GetArgument(0, "width").TryGetValueAsInteger(out var width);
            bundle.GetArgument(1, "height").TryGetValueAsInteger(out var height);

            DreamIcon iconObj = ((DreamObjectIcon)src!).Icon;
            iconObj.Width = width;
            iconObj.Height = height;
            return DreamValue.Null;
        }

        [DreamProc("Crop")]
        [DreamProcParameter("x1", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("y1", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("x2", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("y2", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_Crop(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            bundle.GetArgument(0, "x1").TryGetValueAsInteger(out var x1);
            bundle.GetArgument(1, "y1").TryGetValueAsInteger(out var y1);
            bundle.GetArgument(2, "x2").TryGetValueAsInteger(out var x2);
            bundle.GetArgument(3, "y2").TryGetValueAsInteger(out var y2);

            ((DreamObjectIcon)src!).Crop(x1, y1, x2, y2);
            return DreamValue.Null;
        }

        [DreamProc("DrawBox")]
        [DreamProcParameter("rgb")]
        [DreamProcParameter("x1", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("y1", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("x2", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("y2", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_DrawBox(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            DreamValue rgb = bundle.GetArgument(0, "rgb");
            if (!rgb.TryGetValueAsString(out var colorString) || !ColorHelpers.TryParseColor(colorString, out var color))
                return DreamValue.Null;

            bundle.GetArgument(1, "x1").TryGetValueAsInteger(out var x1);
            bundle.GetArgument(2, "y1").TryGetValueAsInteger(out var y1);
            var x2Arg = bundle.GetArgument(3, "x2");
            var y2Arg = bundle.GetArgument(4, "y2");
            int x2 = x2Arg.TryGetValueAsInteger(out var x2Value) ? x2Value : x1;
            int y2 = y2Arg.TryGetValueAsInteger(out var y2Value) ? y2Value : y1;

            ((DreamObjectIcon)src!).DrawBox(color, x1, y1, x2, y2);
            return DreamValue.Null;
        }

        [DreamProc("Flip")]
        [DreamProcParameter("dir", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_Flip(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            bundle.GetArgument(0, "dir").TryGetValueAsInteger(out var dir);

            ((DreamObjectIcon)src!).Flip((AtomDirection)dir);
            return DreamValue.Null;
        }

        [DreamProc("GetPixel")]
        [DreamProcParameter("x", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("y", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("icon_state", Type = DreamValueTypeFlag.String)]
        [DreamProcParameter("dir", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("frame", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("moving", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_GetPixel(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            bundle.GetArgument(0, "x").TryGetValueAsInteger(out var x);
            bundle.GetArgument(1, "y").TryGetValueAsInteger(out var y);

            return ((DreamObjectIcon)src!).GetPixel(x, y, bundle.GetArgument(2, "icon_state"), bundle.GetArgument(3, "dir"), bundle.GetArgument(4, "frame"));
        }

        [DreamProc("SetIntensity")]
        [DreamProcParameter("r", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("g", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("b", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_SetIntensity(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            bundle.GetArgument(0, "r").TryGetValueAsFloat(out var r);
            var gArg = bundle.GetArgument(1, "g");
            var bArg = bundle.GetArgument(2, "b");
            float g = gArg.TryGetValueAsFloat(out var gValue) ? gValue : r;
            float b = bArg.TryGetValueAsFloat(out var bValue) ? bValue : r;

            ((DreamObjectIcon)src!).SetIntensity(r, g, b);
            return DreamValue.Null;
        }

        [DreamProc("MapColors")]
        public static DreamValue NativeProc_MapColors(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            Dictionary<Color, Color> colorMap = new();

            for (int i = 0; i + 1 < bundle.Arguments.Length; i += 2) {
                if (!bundle.Arguments[i].TryGetValueAsString(out var fromString) ||
                    !bundle.Arguments[i + 1].TryGetValueAsString(out var toString) ||
                    !ColorHelpers.TryParseColor(fromString, out var fromColor) ||
                    !ColorHelpers.TryParseColor(toString, out var toColor)) {
                    continue;
                }

                colorMap[fromColor] = toColor;
            }

            ((DreamObjectIcon)src!).MapColors(colorMap);
            return DreamValue.Null;
        }

        [DreamProc("Shift")]
        [DreamProcParameter("dir", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("offset", Type = DreamValueTypeFlag.Float)]
        [DreamProcParameter("wrap", Type = DreamValueTypeFlag.Float, DefaultValue = 0)]
        public static DreamValue NativeProc_Shift(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            bundle.GetArgument(0, "dir").TryGetValueAsInteger(out var dir);
            bundle.GetArgument(1, "offset").TryGetValueAsInteger(out var offset);
            bundle.GetArgument(2, "wrap").TryGetValueAsInteger(out var wrap);

            ((DreamObjectIcon)src!).Shift((AtomDirection)dir, offset, wrap != 0);
            return DreamValue.Null;
        }

        [DreamProc("SwapColor")]
        [DreamProcParameter("old_rgb")]
        [DreamProcParameter("new_rgb")]
        public static DreamValue NativeProc_SwapColor(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            DreamValue oldRgb = bundle.GetArgument(0, "old_rgb");
            DreamValue newRgb = bundle.GetArgument(1, "new_rgb");
            if (!oldRgb.TryGetValueAsString(out var oldColorString) || !newRgb.TryGetValueAsString(out var newColorString) ||
                !ColorHelpers.TryParseColor(oldColorString, out var oldColor) || !ColorHelpers.TryParseColor(newColorString, out var newColor)) {
                return DreamValue.Null;
            }

            ((DreamObjectIcon)src!).SwapColor(oldColor, newColor);
            return DreamValue.Null;
        }

        [DreamProc("Turn")]
        [DreamProcParameter("angle", Type = DreamValueTypeFlag.Float)]
        public static DreamValue NativeProc_Turn(NativeProc.Bundle bundle, DreamObject? src, DreamObject? usr) {
            DreamValue angleArg = bundle.GetArgument(0, "angle");
            if (!angleArg.TryGetValueAsFloat(out float angle)) {
                return new DreamValue(src!); // Defaults to input on invalid angle
            }

            _NativeProc_TurnInternal((DreamObjectIcon)src!, angle);
            return DreamValue.Null;
        }

        /// <summary> Turns a given icon a given amount of degrees clockwise. </summary>
        public static void _NativeProc_TurnInternal(DreamObjectIcon src, float angle) {
            src.Turn(angle);
        }
    }
}
