using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedpackController : MonoBehaviour
{
    /// <summary>
    /// Basic item set up
    /// </summary>
    [SerializeField] private MedPack itemData;
    [SerializeField] private int stackCount = 3;


    private void Awake()
    {
        // get observer from UI SlotsManager
        var uiObserver = GameObject.Find("Slots").GetComponent<InvUI_SlotsManager>().GetObserver();
        // subscribe to it and to observer in inventory
        itemData.SubscribeTo(uiObserver);
        itemData.SubscribeTo(Inventory.InventoryInternal_ThrowRubbishObserver.instance);
        itemData.MaxStackCount = stackCount;
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            CollectItem();
        }
    }


    void CollectItem()
    {
        if (Inventory.instance.IsFreeSlotAvailable())
        {
            itemData.Collect();
            gameObject.SetActive(false);
        }

    }
}
