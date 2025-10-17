using System;
using UnityEngine;

public class CashController : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnCashCreated.Invoke(this);
    }
}
