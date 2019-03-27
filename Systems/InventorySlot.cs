using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for single slot in inventory.
/// </summary>
[System.Serializable]
public class InventorySlot 
{
    #region Public properties
    public ItemClass itemInfo;
    public bool slotAvailable => available;
    #endregion

    private bool available;

    public InventorySlot() { MarkAvailable(true); }
    public void MarkAvailable(bool isAvailable)
    {
        available = isAvailable;
    }
}
