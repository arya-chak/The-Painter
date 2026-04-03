using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Settings")]
    public float joystickRadius = 75f;   // half of background width
    public float deadZone = 0.15f;       // ignore tiny accidental inputs

    [Header("References")]
    public RectTransform knob;           // drag JoystickKnob here in Inspector

    // Read this from PlayerController
    public Vector2 Direction { get; private set; }

    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        // Clamp to radius
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, joystickRadius);
        knob.anchoredPosition = clamped;

        // Normalize and apply dead zone
        Vector2 raw = clamped / joystickRadius;
        Direction = raw.magnitude >= deadZone ? raw : Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        knob.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
    }
}