using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductArea : MonoBehaviour
{
    public float duration;
    public GameObject productPrefab;
    public List<SlotController> slots;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        yield return new WaitForSeconds(duration);

        foreach (var s in slots)
        {
            if (s.slotObject == null)
            {
                GameObject spawnObject = Instantiate(productPrefab);
                spawnObject.transform.position = s.transform.position;
                spawnObject.transform.rotation = s.transform.rotation;
                
                s.AddItem(spawnObject);

                break;
            }
        }
        
        StartCoroutine(Spawner());
    }
}
