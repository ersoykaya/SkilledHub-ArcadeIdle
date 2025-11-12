using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public List<GameObject> customerPrefabs;
    public float spawnRate;
    public Transform spawnPoint;
    public int maxCapacity;
    
    private List<GameObject> customers = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomer());

        GameManager.Instance.OnCustomerExit += (customer) =>
        {
            customers.Remove(customer);
            Destroy(customer);
        };
    }

    private IEnumerator SpawnCustomer()
    {
        yield return new WaitForSeconds(spawnRate);

        if (maxCapacity > customers.Count)
        {
            int randIndex = UnityEngine.Random.Range(0, customerPrefabs.Count);
            GameObject customer = Instantiate(customerPrefabs[randIndex], spawnPoint.position, spawnPoint.rotation);
            customers.Add(customer);
        }

        StartCoroutine(SpawnCustomer());
    }
}
