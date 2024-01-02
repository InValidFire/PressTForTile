using BepInEx;
using HarmonyLib;

namespace PressTForTile;

[BepInPlugin("Ember.Mod.PressTForTile", "Press T For Tile", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public Harmony harmonyMain;
    public static Item currentItem = null;
    private void Awake()
    {
        // Plugin startup logic
        harmonyMain = new Harmony("Ember.Mod.PressTForTile");
        harmonyMain.PatchAll(typeof(TileTogglePatch));
        harmonyMain.PatchAll(typeof(CurrentItemEnterPatch));
        harmonyMain.PatchAll(typeof(CurrentItemExitPatch));
    }
}
