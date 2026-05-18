using System.IO;
using System.Runtime.InteropServices;
using OpenDreamRuntime.Resources;

namespace OpenDreamRuntime.Procs;

public static class DllHelper {
    private static readonly Dictionary<string, nint> LoadedDlls = new();
    private static readonly Dictionary<(nint DLL, string ExportName), nint> LoadedExports = new();

    public static unsafe delegate* unmanaged[Cdecl]<int, byte**, byte*> ResolveDllTarget(
        DreamResourceManager resource,
        string dllName,
        string funcName) {
        // stdcall convention
        if (funcName.Contains('@'))
            throw new NotSupportedException("Stdcall calling convention is not supported in OpenDream");

        var dll = GetDll(resource, dllName);

        if (!LoadedExports.TryGetValue((dll, funcName), out var export)) {
            if (!NativeLibrary.TryGetExport(dll, funcName, out export))
                throw new MissingMethodException($"FFI: Unable to find symbol {funcName} in library {dllName}");

            LoadedExports.Add((dll, funcName), export);
        }

        return (delegate* unmanaged[Cdecl]<int, byte**, byte*>)export;
    }

    private static nint GetDll(DreamResourceManager resource, string dllName) {
        if (LoadedDlls.TryGetValue(dllName, out var dll))
            return dll;

        dll = ResolveDll(resource, dllName);
        LoadedDlls.Add(dllName, dll);
        return dll;
    }

    private static nint ResolveDll(DreamResourceManager resource, string dllName) {
        Exception? directLoadException = null;
        try {
            return NativeLibrary.Load(dllName);
        } catch (Exception e) when (e is DllNotFoundException or BadImageFormatException) {
            directLoadException = e;
        }

        // Simple load didn't pass, try next to dmb.
        var root = resource.RootPath;
        var fullPath = Path.Combine(root, dllName);
        if (!File.Exists(fullPath))
            throw new DllNotFoundException($"FFI: Unable to load {dllName}. File not found at {fullPath}. Loader error: {directLoadException.Message}", directLoadException);

        try {
            return NativeLibrary.Load(fullPath);
        } catch (Exception e) when (e is DllNotFoundException or BadImageFormatException) {
            throw new DllNotFoundException($"FFI: Unable to load {dllName} at {fullPath}. Loader error: {e.Message}", e);
        }
    }
}
