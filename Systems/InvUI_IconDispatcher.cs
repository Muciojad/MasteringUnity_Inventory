using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvUI_IconDispatcher : MonoBehaviour
{
    public static InvUI_IconDispatcher instance;
    [SerializeField] private Sprite[] iconList;

    void Awake()
    {
        instance = this;
    }

    public Sprite GetIconFromList(int id)
    {
        if (id >= 0 && id < iconList.Length)
        {
            if (iconList[id] == null) Debug.Log("null w iconlist");
            return iconList[id];
        }
        else return null;
    }
}
