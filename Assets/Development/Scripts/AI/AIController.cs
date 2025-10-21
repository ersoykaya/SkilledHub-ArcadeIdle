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
    private Animator _animator;

    private bool _checkUpdate = false;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        InitializeEvents();
        InitializePatrolPoints();

        NaturalDec = NaturalDecision();
        StartCoroutine(NaturalDec);
    }

    private void Update()
    {
        if (movementState != EMovementState.Moving) return;
        
        Debug.Log("Distance == " + CalculateDistance());
        if (CalculateDistance() < 1f && _checkUpdate)
        {
            _checkUpdate = false;
            _animator.SetBool("IsMoving", false);
            
            StopCoroutine(NaturalDec);
            NaturalDec = NaturalDecision();
            StartCoroutine(NaturalDec);
        }
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

    private float CalculateDistance()
    {
        Vector3 myTransform = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetTransform = new Vector3(_targetPoint.position.x, 0, _targetPoint.position.z);
        
        float distance = Vector3.Distance(myTransform, targetTransform);
        return distance;
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
                _animator.SetBool("IsMoving", true);
                SetTargetPoint();
                _agent.SetDestination(_targetPoint.position);
                break;
        }
    }

    private void CollectProducts()
    {
        if (_currentShelf.CheckSlotsEmpty() == false)
        {
            _product = _currentShelf.GetProductFromSlot();
            _product.transform.position = handTransform.position;
            _product.transform.parent = handTransform;
        }
    }

    private void Payment()
    {
        Destroy(_product);
        
        StopCoroutine(NaturalDec);
        
        StartCoroutine(UpdateShoppingState(EShoppingState.Exit));
        StartCoroutine(UpdateMovementState(EMovementState.Moving));
    }

    private IEnumerator NaturalDecision()
    {
        bool canIdleMove = true;
        
        int randDuration = UnityEngine.Random.Range(1, 5);
        
        yield return new WaitForSeconds(randDuration);

        if (_product != null)
        {
            if (GameManager.Instance.GetCashCount() > 0)
            {
                StartCoroutine(UpdateShoppingState(EShoppingState.Cashing));
                StartCoroutine(UpdateMovementState(EMovementState.Moving));

                canIdleMove = false;
            }
        }
        else
        {
            if (GameManager.Instance.GetShelfCount() > 0)
            {
                int randSearch = UnityEngine.Random.Range(0, 2);
            
                if (randSearch == 0)
                {
                    StartCoroutine(UpdateShoppingState(EShoppingState.ProductSearch));
                    StartCoroutine(UpdateMovementState(EMovementState.Moving));
                    
                    canIdleMove = false;
                }
            }
        }

        if (canIdleMove == true)
        {
            StartCoroutine(UpdateShoppingState(EShoppingState.Idle));
            StartCoroutine(UpdateMovementState(EMovementState.Moving));
        }

        _checkUpdate = true;
    }
    private void SetTargetPoint()
    {
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
            if (shoppingState == EShoppingState.ProductSearch)
            {
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
