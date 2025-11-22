using UnityEngine;
using UnityEngine.EventSystems;

public class JuicyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private float pressedScale = 0.9f;  // How small it gets when pressed
    [SerializeField] private float scaleSpeed = 10f;     // How fast it scales
    private Vector3 originalScale;
    private Vector3 targetScale;


    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly interpolate toward the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = originalScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // In case the user drags finger/mouse out of the button
        targetScale = originalScale;
    }
}
