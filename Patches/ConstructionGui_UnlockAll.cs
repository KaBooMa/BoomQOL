using System;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.UI;
using SmashHammer.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BoomQOL.Patches;

[HarmonyPatch(typeof(ConstructionGui), nameof(ConstructionGui.Awake))]
public class ConstructionGui_UnlockAll : MonoBehaviour
{
    private static void Postfix(ConstructionGui __instance)
    {
        // Resize the GUI to fit our new button
        RectTransform gui_panel_rect = __instance.transform.FindChild("ConstructionGuiPanel").gameObject.GetComponent<RectTransform>();
        gui_panel_rect.sizeDelta += new Vector2(100, 0);

        // Grab a donor button to reference later
        GameObject donor_object = __instance.transform.Find("ConstructionGuiPanel/Contents/OperationsPanel/LayoutGroup/DeactivatePartBehavioursButton").gameObject;
        
        // Create our new button and set properties
        GameObject unlock_object = DefaultControls.CreateButton(new DefaultControls.Resources());
        unlock_object.transform.SetParent(donor_object.transform.parent, false);
        unlock_object.layer = 5;
        unlock_object.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        Text unlock_text = unlock_object.transform.FindChild("Text (Legacy)").GetComponent<Text>();
        unlock_text.text = "Unlock\nAttachments";
        unlock_text.fontSize = 8;

        // Add a SmashHammer Element. Not sure if this is needed, but other buttons had it.
        Element element = unlock_object.AddComponent<Element>();

        // Add a custom SmashHammer Tooltip
        Tooltip tooltip = unlock_object.AddComponent<Tooltip>();
        tooltip.tooltipText = "Unlock all attachments";
        tooltip.tooltipHideEvent = donor_object.GetComponent<Tooltip>().tooltipHideEvent;
        tooltip.tooltipShowEvent = donor_object.GetComponent<Tooltip>().tooltipShowEvent;

        // Add our delegate method to our button to handle clicking
        Action action = delegate {
            OnClick(__instance);
        };
        Button button = unlock_object.GetComponent<Button>();
        button.onClick.AddListener(action);

        // Set all children to the UI layer
        unlock_object.SetLayerInChildren(5);
    }

    public static void OnClick(ConstructionGui __instance) 
    {
        // Loops all parts in contruction and their attachments, disabling locked status
        Construction construction = __instance.targetedConstruction.Cast<Construction>();
        foreach (PartDescriptor part in construction.Parts)
        {
            foreach (KeyValuePair<IPart, AttachmentBase> attachment in part.Attachments.ownedAttachments)
            {
                attachment.value.IsLocked = false;
            }
        }
    }
}
