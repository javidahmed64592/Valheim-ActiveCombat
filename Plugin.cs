using ActiveCombat.Core;

using BepInEx;

using HarmonyLib;

using Jotunn.Utils;

namespace ActiveCombat;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[BepInDependency(Jotunn.Main.ModGuid)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
public class ActiveCombatPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "javidahmed64592.activecombat";
    public const string PluginName = "Active Combat";
    public const string PluginVersion = "0.1.1";

    internal static BepInEx.Logging.ManualLogSource Log { get; private set; }

    private Harmony _harmony;

    private void Awake()
    {
        Log = Logger;
        CombatConfig.Bind(Config);
        Translations.Load();

        _harmony = new Harmony(PluginGUID);
        _harmony.PatchAll();
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}
