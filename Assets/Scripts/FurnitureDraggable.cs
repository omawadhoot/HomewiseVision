using UnityEngine;

public class FurnitureDraggable : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private bool isDragging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = GetTouchWorldPos(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingObject(touchPos))
                    {
                        isDragging = true;
                        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
                        offset = transform.position - GetTouchWorldPos(touch.position);
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        transform.position = GetTouchWorldPos(touch.position) + offset;
                    }
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }

    Vector3 GetTouchWorldPos(Vector2 touchPos)
    {
        Vector3 screenPos = new Vector3(touchPos.x, touchPos.y, zCoord);
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    bool IsTouchingObject(Vector3 touchPos)
    {
        Collider collider = GetComponent<Collider>();
        return collider != null && collider.bounds.Contains(touchPos);
    }
}