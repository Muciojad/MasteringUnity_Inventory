using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic item controller.
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] ItemClass itemData;
    [SerializeField] private int stackCount = 3;
    private void Awake()
    {
    }
    void Start()
    {
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
        if(Inventory.instance.IsFreeSlotAvailable())
        {
            itemData.Collect();
            gameObject.SetActive(false);
        }
        
    }
}
