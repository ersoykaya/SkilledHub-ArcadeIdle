
using System;
using UnityEngine;

[System.Serializable]
public struct CurrencyStruct
{
    public float gold;
    public float diamond;
}
public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    public CurrencyStruct currency;

    public Action<float> OnGoldUpdated;
    public Action<float> OnDiamondUpdated;
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
    
        Instance = this;
    }

    private void Start()
    {
        UpdateGold(0);
        UpdateDiamond(0);
    }

    public void UpdateGold(float amount)
    {
        currency.gold += amount;
        OnGoldUpdated?.Invoke(currency.gold);
    }

    public void UpdateDiamond(float amount)
    {
        currency.diamond += amount;
        OnDiamondUpdated?.Invoke(currency.diamond);
    }

    public bool Purchase(float priceAmount)
    {
        if (currency.gold >= priceAmount)
        {
            UpdateGold(-priceAmount);
            return true;
        }
        else
        {
            return false;
        }
    }
}
