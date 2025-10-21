using System;
using UnityEngine;

public class CashController : MonoBehaviour
{
    public Transform customerTransform;
    private void Start()
    {
        GameManager.Instance.OnCashCreated.Invoke(this);
    }
}
