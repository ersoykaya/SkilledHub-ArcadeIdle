using System;
using System.Collections;
using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    public string codeName;
    public float duration;
    protected GameObject _interactingObject;
    private IEnumerator _waitNumerator;
    private bool _canInteractable = true;
    
    protected virtual void DoAction(GameObject interactingObj)
    {
        // Yapmamız gereken işlem gerçekleşecek
        
        Debug.Log("Interacting...");
    }

    protected void SetCanInteractable(bool param)
    {
        _canInteractable = param;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(duration);
        
        DoAction(_interactingObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_canInteractable)
        {
            if (other.CompareTag("Player"))
            {
                _interactingObject = other.gameObject;
                _waitNumerator = Wait();
                StartCoroutine(_waitNumerator);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _interactingObject = null;
            StopCoroutine(_waitNumerator);
        }
    }
}
