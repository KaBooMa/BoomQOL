using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BoomQOL.Patches;
using HarmonyLib;

namespace BoomQOL;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
//[BepInDependency("GearLib")] // Currently no dependency on GearLib. This will change as more QOLs are added
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;

        Harmony.CreateAndPatchAll(typeof(ConstructionGui_UnlockAll));
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}
