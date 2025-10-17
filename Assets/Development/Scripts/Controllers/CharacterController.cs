using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject handSocket;
    
    private Rigidbody _rigidbody;
    private Animator _animator;

    private GameObject _tempObject;
    
    [HideInInspector] public List<GameObject> collectedItems = new List<GameObject>();

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        GameManager.Instance.OnItemCollected += AttachItem;
    }

    public void MoveCharacter(Vector3 direction)
    {
        _rigidbody.linearVelocity = direction * moveSpeed;
        
        Quaternion lookRot = Quaternion.LookRotation(direction);
        lookRot.x = 0;
        lookRot.z = 0;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);
        
        float magnitude = direction.magnitude;
        bool result = magnitude > 0f;
        
        _animator.SetBool("IsRunning", result);
    }

    public void AttachItem(GameObject item)
    {
        float heightLevel = collectedItems.Count * 0.5f;
       item.transform.position = handSocket.transform.position + (Vector3.up * heightLevel);
       item.transform.rotation = handSocket.transform.rotation;
       item.transform.parent =  handSocket.transform;

       _tempObject = item;
       collectedItems.Add(item);
       
       _animator.SetBool("HasItem", true);
    }

    public void DetachItem(GameObject item)
    {
        item.transform.parent = null;
    }

    public void RemoveItemFromList(GameObject item)
    {
        collectedItems.Remove(item);
        
        if (collectedItems.Count == 0)
        {
            _animator.SetBool("HasItem", false);
        }
    }

    public void ClearCollectedItems()
    {
        collectedItems.Clear();
    }
}
