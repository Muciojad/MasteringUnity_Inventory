using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI item icons dispatcher.
/// Specify which icon on list corresponds to item.
/// </summary>
public class InventoryUI_IconManager
{
    public static InventoryUI_IconManager instance = new InventoryUI_IconManager();

    private Dictionary<string, int> itemNameToIconId = new Dictionary<string, int>();
    public InventoryUI_IconManager()
    {
        itemNameToIconId.Add("MedPack", 0);
        itemNameToIconId.Add("AccessCard_C2", 1);
        itemNameToIconId.Add("AccessCard_C1", 1);
    }

    public int GetIconFromName(string name)
    {
        if (itemNameToIconId.ContainsKey(name))
        {
            return itemNameToIconId[name];
        }
        else return -1;
    }
}
