using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Action<GameObject> OnItemCollected;
    public Action<ShelfController> OnShelfCreated;
    public Action<CashController> OnCashCreated;

    public Transform exitTransform;
    
    private List<ShelfController> _allShelves = new();
    private List<CashController> _allCashes = new();

    private void Awake()
    {
        if (Instance != null) return;
        
        Instance = this;
    }

    private void Start()
    {
        OnShelfCreated += AddShelfToList;
        OnCashCreated += AddCashToList;
    }

    public void AddShelfToList(ShelfController newShelf)
    {
        _allShelves.Add(newShelf);
    }

    public void AddCashToList(CashController newCash)
    {
        _allCashes.Add(newCash);
    }

    public int GetShelfCount()
    {
        return _allShelves.Count;
    }

    public int GetCashCount()
    {
        return _allCashes.Count;
    }

    public ShelfController GetRandomShelf()
    {
        int randIndex = UnityEngine.Random.Range(0, _allShelves.Count);
        return _allShelves[randIndex];
    }

    public CashController GetRandomCash()
    {
        int randIndex = UnityEngine.Random.Range(0, _allCashes.Count);
        return _allCashes[randIndex];
    }
}
