using System;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Action<Color> OnSpaceBarClicked;
    
    private Vector3 _beginMousePosition;
    private Vector3 _currentMousePosition;
    private Vector3 _deltaMousePosition;

    public CharacterController controller;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _beginMousePosition = Input.mousePosition; // (x, y, 0)
        }

        if (Input.GetMouseButton(0))
        {
            _currentMousePosition = Input.mousePosition; // (x, y, 0)

            _deltaMousePosition = _currentMousePosition - _beginMousePosition; // (x, y, 0)

            Vector3 moveDirection = new Vector3(_deltaMousePosition.x, 0, _deltaMousePosition.y);
            moveDirection.Normalize();
            
            controller.MoveCharacter(moveDirection);
        }

        if (Input.GetMouseButtonUp(0))
        {
            controller.MoveCharacter(Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Color newColor = UnityEngine.Random.ColorHSV();
            OnSpaceBarClicked.Invoke(newColor);
        }
    }
}
