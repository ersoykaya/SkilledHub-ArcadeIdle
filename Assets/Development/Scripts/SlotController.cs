using System;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [HideInInspector] public GameObject slotObject;

    private void Start()
    {
        GameManager.Instance.OnItemCollected += OnCollectedItem;
    }

    public void AddItem(GameObject item)
    {
        if (slotObject == null)
        {
            slotObject = item;
        }
    }

    public void RemoveItem()
    {
        slotObject = null;
    }

    public void OnCollectedItem(GameObject item)
    {
        if (slotObject == item)
        {
            RemoveItem();
        }
    }
}
