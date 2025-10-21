using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public enum EShoppingState
{
    Idle,
    ProductSearch,
    Cashing,
    Exit
}
public enum EMovementState
{
    Waiting,
    Moving
}
public class AIController : MonoBehaviour
{
    public EShoppingState shoppingState;
    public EMovementState movementState;
    public Transform patrolParent;
    public Transform handTransform;

    private List<Transform> _patrolPoints = new();
    private NavMeshAgent _agent;
    private Transform _targetPoint;
    private ShelfController _currentShelf;
    private CashController _currentCash;
    private GameObject _product;
    private IEnumerator NaturalDec;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        InitializeEvents();
        InitializePatrolPoints();

        NaturalDec = NaturalDecision();
        StartCoroutine(NaturalDec);
    }

    private void Update()
    {
        if (movementState != EMovementState.Moving) return;
        
       // if(CalculateDistance() == true) 
         //   StartCoroutine(UpdateMovementState(EMovementState.Waiting));
    }

    private void InitializeEvents()
    {
        //...
    }

    private void InitializePatrolPoints()
    {
        for (int i = 0; i < patrolParent.childCount; i++)
        {
            _patrolPoints.Add(patrolParent.GetChild(i));
        }
    }

    private bool CalculateDistance()
    {
        float distance = Vector3.Distance(transform.position, _targetPoint.position);
        bool result = distance < 5f;

        return result;
    }

    private IEnumerator UpdateShoppingState(EShoppingState newState)
    {
        shoppingState = newState;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator UpdateMovementState(EMovementState newState)
    {
        movementState = newState;

        switch (movementState)
        {
            case EMovementState.Waiting:
                yield return new WaitForSeconds(2f);
                StartCoroutine(UpdateMovementState(EMovementState.Moving));
                break;
            
            case  EMovementState.Moving:
                SetTargetPoint();
                _agent.SetDestination(_targetPoint.position);
                break;
        }
    }

    private void CollectProducts()
    {
        if (_currentShelf.CheckSlotsEmpty())
        {
            // raf boş
        }
        else
        {
            StopCoroutine(NaturalDec);
            
            _product = _currentShelf.GetProductFromSlot();
            _product.transform.position = handTransform.position;
            _product.transform.parent = handTransform;
            
            if (GameManager.Instance.GetCashCount() > 0)
            {
                StartCoroutine(UpdateShoppingState(EShoppingState.Cashing));
                StartCoroutine(UpdateMovementState(EMovementState.Moving));
            }
            else if (GameManager.Instance.GetCashCount() == 0)
            {
                StartCoroutine(UpdateShoppingState(EShoppingState.Idle));
                StartCoroutine(UpdateMovementState(EMovementState.Moving));
            }
        }
    }

    private void Payment()
    {
        Destroy(_product);
        
        StartCoroutine(UpdateShoppingState(EShoppingState.Exit));
        StartCoroutine(UpdateMovementState(EMovementState.Moving));
    }

    private IEnumerator NaturalDecision()
    {
        int randDuration = UnityEngine.Random.Range(1, 5);
        
        yield return new WaitForSeconds(randDuration);

        if (GameManager.Instance.GetShelfCount() > 0)
        {
            int randSearch = UnityEngine.Random.Range(0, 2);
            
            if (randSearch == 0)
            {
                StartCoroutine(UpdateShoppingState(EShoppingState.ProductSearch));
                StartCoroutine(UpdateMovementState(EMovementState.Moving));
            
                yield return new WaitForSeconds(1);
            }
            else
            {
                StartCoroutine(UpdateShoppingState(EShoppingState.Idle));
                StartCoroutine(UpdateMovementState(EMovementState.Moving));
            
                yield return new WaitForSeconds(1);
            }
        }
        else if (GameManager.Instance.GetShelfCount() == 0)
        {
            StartCoroutine(UpdateShoppingState(EShoppingState.Idle));
            StartCoroutine(UpdateMovementState(EMovementState.Moving));
            
            yield return new WaitForSeconds(1);
        }

        NaturalDec = NaturalDecision();
        StartCoroutine(NaturalDec);
    }
    private void SetTargetPoint()
    {
        Debug.Log("SetTargetPoint = " + shoppingState);
        switch (shoppingState)
        {
            case EShoppingState.Idle:
                _targetPoint = GetRandomPatrolPoint();
                break;
            
            case EShoppingState.ProductSearch:
                _currentShelf = GameManager.Instance.GetRandomShelf();
                _targetPoint = _currentShelf.customerTransform;
                break;
            
            case EShoppingState.Cashing:
                _currentCash = GameManager.Instance.GetRandomCash();
                _targetPoint = _currentCash.customerTransform;
                break;
            
            case EShoppingState.Exit:
                _targetPoint = GameManager.Instance.exitTransform;
                break;
        }
    }

    private Transform GetRandomPatrolPoint()
    {
        int randIndex = UnityEngine.Random.Range(0, _patrolPoints.Count);
        Transform targetPoint = _patrolPoints[randIndex];
        
        return targetPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ShelfController>())
        {
            Debug.Log("Shelf Controller Triggered");
            
            if (shoppingState == EShoppingState.ProductSearch)
            {
                Debug.Log("Shelf Controller Product Collect");
                CollectProducts();
            }
        }

        if (other.GetComponent<CashController>())
        {
            Payment();
        }

        if (other.GetComponent<Exit>())
        {
            Destroy(gameObject);
        }
    }
}
