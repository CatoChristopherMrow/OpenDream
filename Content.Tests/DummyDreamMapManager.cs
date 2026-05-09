using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DMCompiler.Json;
using OpenDreamRuntime;
using OpenDreamRuntime.Map;
using OpenDreamRuntime.Objects;
using OpenDreamRuntime.Objects.Types;
using OpenDreamRuntime.Procs;
using OpenDreamShared.Dream;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;

namespace Content.Tests;

public sealed partial class DummyDreamMapManager : IDreamMapManager {
    [Dependency] private DreamManager _dreamManager = default!;
    [Dependency] private DreamObjectTree _objectTree = default!;

    public Vector2i Size => _size;
    public int Levels => _levels.Count;
    public DreamObjectArea DefaultArea => _defaultArea ??= CreateArea(_objectTree.Area.ObjectDefinition);

    private static Vector2i _size = Vector2i.Zero;
    private static readonly List<IDreamMapManager.Level> _levels = new();
    private DreamObjectArea? _defaultArea;

    public void Initialize() {
        _size = Vector2i.Zero;
        _levels.Clear();
        _defaultArea = null;
    }

    public void UpdateTiles() { }

    public void LoadMaps(List<DreamMapJson>? maps) {
        var world = _dreamManager.WorldInstance;
        using var worldMaxX = world.GetVariable("maxx");
        using var worldMaxY = world.GetVariable("maxy");
        using var worldMaxZ = world.GetVariable("maxz");
        worldMaxX.TryGetValueAsInteger(out var maxX);
        worldMaxY.TryGetValueAsInteger(out var maxY);
        worldMaxZ.TryGetValueAsInteger(out var maxZ);

        if (maps != null) {
            foreach (DreamMapJson map in maps) {
                maxX = int.Max(maxX, map.MaxX);
                maxY = int.Max(maxY, map.MaxY);
                maxZ = int.Max(maxZ, map.MaxZ);
            }
        }

        SetWorldSize(new Vector2i(maxX, maxY));
        SetZLevels(maxZ);
    }

    public void InitializeAtoms() { }

    public void SetTurf(DreamObjectTurf turf, DreamObjectDefinition type, DreamProcArguments creationArguments) {
        turf.SetTurfType(type);
    }

    public void SetTurfAppearance(DreamObjectTurf turf, ImmutableAppearance appearance) { }

    public void SetTurfAppearance(DreamObjectTurf turf, MutableAppearance appearance) { }

    public void SetAreaAppearance(DreamObjectArea area, MutableAppearance appearance) { }

    public bool TryGetCellAt(Vector2i pos, int z, [NotNullWhen(true)] out IDreamMapManager.Cell? cell) {
        if (pos.X < 1 || pos.X > Size.X || pos.Y < 1 || pos.Y > Size.Y || z < 1 || z > Levels) {
            cell = null;
            return false;
        }

        cell = _levels[z - 1].Cells[pos.X - 1, pos.Y - 1];
        return true;
    }

    public bool TryGetTurfAt(Vector2i pos, int z, [NotNullWhen(true)] out DreamObjectTurf? turf) {
        if (TryGetCellAt(pos, z, out var cell)) {
            turf = cell.Turf;
            return true;
        }

        turf = null;
        return false;
    }

    public void SetZLevels(int levels) {
        if (levels > Levels) {
            for (var z = Levels + 1; z <= levels; z++) {
                _levels.Add(new IDreamMapManager.Level(z, default, _objectTree.Turf.ObjectDefinition, DefaultArea, Size));
            }
        } else if (levels < Levels) {
            _levels.RemoveRange(levels, Levels - levels);
        }
    }

    public void SetWorldSize(Vector2i size) {
        var oldSize = _size;
        _size = size;

        foreach (IDreamMapManager.Level level in _levels) {
            var oldCells = level.Cells;
            level.Cells = new IDreamMapManager.Cell[size.X, size.Y];

            for (var x = 1; x <= size.X; x++) {
                for (var y = 1; y <= size.Y; y++) {
                    if (x <= oldSize.X && y <= oldSize.Y) {
                        level.Cells[x - 1, y - 1] = oldCells[x - 1, y - 1];
                        continue;
                    }

                    var turf = new DreamObjectTurf(_objectTree.Turf.ObjectDefinition, x, y, level.Z);
                    var cell = new IDreamMapManager.Cell(DefaultArea, turf);
                    turf.Cell = cell;
                    level.Cells[x - 1, y - 1] = cell;
                    turf.DecRef();
                }
            }
        }
    }

    public EntityUid GetZLevelEntity(int z) {
        return EntityUid.Invalid;
    }

    public IEnumerable<DreamObjectMob> GetMobsInRange((int X, int Y, int Z) loc, int distance) {
        yield break;
    }

    public IEnumerable<AtomDirection> CalculateSteps((int X, int Y, int Z) loc, (int X, int Y, int Z) dest, int targetDistance, int maxSteps) {
        yield break;
    }

    private DreamObjectArea CreateArea(DreamObjectDefinition definition) {
        DreamObjectArea area = new(definition);
        area.InitSpawn(new());
        return area;
    }
}
