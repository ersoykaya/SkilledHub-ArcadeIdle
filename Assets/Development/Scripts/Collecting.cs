using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Collecting : Interactables
{
    [SerializeField] private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody.isKinematic = true;
    }

    protected override void DoAction(GameObject interactingObj)
    {
        base.DoAction(interactingObj);
        
        SetCanInteractable(false);
        
        GameManager.Instance.OnItemCollected.Invoke(gameObject);
    }
}
