using UnityEngine;

public class FreeRoamTouchController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float lookSpeed = 0.1f; // Adjust for smooth rotation

    private CharacterController controller;
    private float rotationX = 0f;

    private Vector2 touchStartPos;
    private Vector2 touchDelta;
    private bool isMoving = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isMoving = true;
                    break;

                case TouchPhase.Moved:
                    touchDelta = touch.position - touchStartPos;
                    Vector3 moveDirection = new Vector3(touchDelta.x, 0, touchDelta.y).normalized;
                    controller.Move(moveDirection * moveSpeed * Time.deltaTime);
                    break;

                case TouchPhase.Ended:
                    isMoving = false;
                    break;
            }
        }
    }

    void HandleLook()
    {
        if (Input.touchCount > 1) // Second finger for looking around
        {
            Touch touch = Input.GetTouch(1);
            rotationX -= touch.deltaPosition.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.Rotate(Vector3.up * touch.deltaPosition.x * lookSpeed);
            Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }
}