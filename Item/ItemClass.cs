using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class ItemClass : IUsable, ICollectable
{

    private class CollectionNotifier : Subject
    {
        public static CollectionNotifier instance = new CollectionNotifier();
        public CollectionNotifier()
        {
            AddObserver(Inventory.instance);
        }
        public void SubscribeTo(Observer observer)
        {
            AddObserver(observer);
        }
    }

    #region InterfacesImplementation
    public void Collect()
    {
        CollectionNotifier.instance.Notify(new OnItemCollectedEvent() { itemData = this });
    }

    public void SendNotificationToAll(object message)
    {
        CollectionNotifier.instance.Notify(message);
    }

    public virtual void MarkUsed()
    {
        itemUnnecessary = true;
    }



    public virtual void Use()
    {
        if (!ItemUnnecessary)
        {
            if(StackingEnabled && currentStackCount > 0)
            {
                Debug.Log("base use of stacking item");
                currentStackCount--;
                SendNotificationToAll(new OnSlotsMerge() { slotA = SlotIdInInventory });
                SendNotificationToAll(new OnItemUsefullnessChange() { slotId = SlotIdInInventory });
            }
            else
            {
                Debug.Log("base use of non-stacking item");
            }
        }
    }


    #endregion

    #region Methods

    public void SetIdInInventory(int id)
    {
        slotId = id;
    }

    public void SubscribeTo(Observer observer)
    {
        CollectionNotifier.instance.SubscribeTo(observer);
    }

    #endregion


    #region private variables
    private bool itemUnnecessary;
    private int slotId = -1;
    private int maxStackCount = 1;
    private int currentStackCount = 1;
    #endregion
    #region Properties

    public int ItemWeight;
    public bool StackingEnabled;
    public bool ItemIsReusable;
    public bool ItemUnnecessary => itemUnnecessary;

    public string ItemName;

    public string ItemDescription;

    public int SlotIdInInventory => slotId;
    public int StackCount { get { return currentStackCount; } set { currentStackCount = value; } }

    public bool ManualUseAllowed = true;
    public int MaxStackCount
    {
        get { return maxStackCount; }
        set { maxStackCount = value; }
    }
    
    #endregion

    #region Constructor
    public ItemClass()
    {
    }
    #endregion

    #region Helpers
    public static T DeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
    #endregion
}
