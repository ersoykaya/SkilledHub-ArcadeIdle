using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShelfController : Interactables
{
    [SerializeField] private List<SlotController> _slots;
    private List<GameObject> _tempItems = new();

    private void Start()
    {
        GameManager.Instance.OnShelfCreated.Invoke(this);
    }

    protected override void DoAction(GameObject interactingObj)
    {
        base.DoAction(interactingObj);

        List<GameObject> characterItems = interactingObj.GetComponent<CharacterController>().collectedItems;

        StartCoroutine(AddItemsInTime(characterItems, interactingObj, 0.25f));
    }

    private IEnumerator AddItemsInTime(List<GameObject> items, GameObject interactingObj, float duration = 0)
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            GameObject currentItem = items[i];
            
            yield return new WaitForSeconds(duration);
            
            if (currentItem.GetComponent<Interactables>().codeName == codeName)
            {
                foreach (var s in _slots)
                {
                    if (s.slotObject == null)
                    {
                        s.AddItem(currentItem);
                        _tempItems.Add(currentItem);
                    
                        currentItem.transform.position = s.transform.position;
                        currentItem.transform.rotation = s.transform.rotation;
                    
                        interactingObj.GetComponent<CharacterController>().DetachItem(currentItem);

                        break;
                    }
                }
            }
        }
        
        foreach (var t in _tempItems)
        {
            interactingObj.GetComponent<CharacterController>().RemoveItemFromList(t);
        }
    }
}










































