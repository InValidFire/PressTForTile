using HarmonyLib;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
namespace PressTForTile;

class TileTogglePatch
{
    [HarmonyPatch(typeof(CameraController), "Update")]
    [HarmonyPostfix]
    static void Postfix(CameraController __instance)
    {
        static void startHighlight(Item item, Item otherItem) {
            if (item == null || otherItem == null) {
                return;
            }
            else if (!item.isBeingDragged) {  // we don't change the highlight if it's being dragged anyways.
                item.AddToHighlightStack(otherItem);
                startHighlight(item, otherItem.GetItemAbove());
            }
        }
        static void stopHighlight(Item item) {
            if (item == null) {
                return;
            }
            else if (!item.isBeingDragged) {
                item.Highlight(show: false);
                stopHighlight(item.GetItemAbove());
            };
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Item.groundIsLocked = !Item.groundIsLocked;
            Events.instance.Raise(new GroundLockChangedEvent());
            var item = Plugin.currentItem;
            if (item == null) {
                return;
            }
            if (!item.CanBeMoved()) {
                stopHighlight(item);
            }
            else if (item.CanBeMoved()) {
                startHighlight(item, item);
            }
        }
    }
}

class CurrentItemEnterPatch
{
    [HarmonyPatch(typeof(Item), "IInputEnterHandler.OnPointerEnter")]
    [HarmonyPostfix]
    static void Postfix(Item __instance, InputEventData eventData) {
        Plugin.currentItem = __instance;
    }
}

class CurrentItemExitPatch
{
    [HarmonyPatch(typeof(Item), "IInputEnterHandler.OnPointerExit")]
    [HarmonyPostfix]
    static void Postfix(Item __instance, InputEventData eventData) {
        if (Plugin.currentItem == __instance) {
            Plugin.currentItem = null;
        }
    }
}