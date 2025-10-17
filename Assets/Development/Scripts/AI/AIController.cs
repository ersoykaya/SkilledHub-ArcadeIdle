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

    private List<Transform> _patrolPoints = new();
    private NavMeshAgent _agent;
    private Transform _targetPoint;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        InitializeEvents();
        InitializePatrolPoints();

        StartCoroutine(NaturalDecision());
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

    private IEnumerator NaturalDecision()
    {
        int randDuration = UnityEngine.Random.Range(1, 5);
        
        yield return new WaitForSeconds(randDuration);
        
        Debug.Log("Natural Decision");

        if (GameManager.Instance.GetShelfCount() > 0)
        {
            Debug.Log("There is Shelf");
            StartCoroutine(UpdateShoppingState(EShoppingState.ProductSearch));
            StartCoroutine(UpdateMovementState(EMovementState.Moving));
            
            yield return new WaitForSeconds(1);
        }
        else if (GameManager.Instance.GetShelfCount() == 0)
        {
            Debug.Log("There is No Shelf");
            StartCoroutine(UpdateShoppingState(EShoppingState.Idle));
            StartCoroutine(UpdateMovementState(EMovementState.Moving));
            
            yield return new WaitForSeconds(1);
        }

        /*if (GameManager.Instance.GetCashCount() > 0)
        {
            StartCoroutine(UpdateShoppingState(EShoppingState.Cashing));
            StartCoroutine(UpdateMovementState(EMovementState.Moving));
            yield return new WaitForSeconds(1);
        }
        else if (GameManager.Instance.GetCashCount() == 0)
        {
            StartCoroutine(UpdateShoppingState(EShoppingState.Idle));
            StartCoroutine(UpdateMovementState(EMovementState.Moving));
            yield return new WaitForSeconds(1);
        }*/

        StartCoroutine(NaturalDecision());
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
                _targetPoint = GameManager.Instance.GetRandomShelf().transform;
                break;
            
            case EShoppingState.Cashing:
                _targetPoint = GameManager.Instance.GetRandomCash().transform;
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
}
