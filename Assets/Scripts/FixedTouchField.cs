using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    void Update()
    {
        if (Pressed)
        {
            Vector2 newTouchDist;

            if (PointerId >= 0 && PointerId < Input.touchCount)
            {
                newTouchDist = Input.touches[PointerId].position - PointerOld;
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                newTouchDist = (Vector2)Input.mousePosition - PointerOld;
                PointerOld = Input.mousePosition;
            }

            TouchDist = newTouchDist;
        }
        else
        {
            TouchDist = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
        TouchDist = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        TouchDist = Vector2.zero;
    }
}
