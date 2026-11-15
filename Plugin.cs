using BepInEx;

using CombatFlow.Core;

using HarmonyLib;

using Jotunn.Utils;

namespace CombatFlow;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency(Jotunn.Main.ModGuid)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
public class CombatFlowPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "javidahmed64592.combatflow";
    public const string PluginName = "Combat Flow";
    public const string PluginVersion = "0.1.0";

    internal static BepInEx.Logging.ManualLogSource Log { get; private set; }

    private Harmony _harmony;

    private void Awake()
    {
        Log = Logger;
        CombatConfig.Bind(Config);

        _harmony = new Harmony(PluginGUID);
        _harmony.PatchAll();
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}
