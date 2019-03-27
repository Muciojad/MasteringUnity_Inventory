using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InvUI_SlotsManager slotsManager;
    private GameObject child;
    void Start()
    {
        child = transform.GetChild(0).gameObject;

        ShowHideInventory();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ShowHideInventory();
        }
    }

    void ShowHideInventory()
    {
        if (child.activeInHierarchy) child.SetActive(false);
        else
        {
            child.SetActive(true);
            FinalSlotUpdate();
        }
    }

    public void FinalSlotUpdate()
    {
        for(int i = 0; i< slotsManager.Length; i++)
        {
            slotsManager.UpdateSlot(i);
        }
    }
}
