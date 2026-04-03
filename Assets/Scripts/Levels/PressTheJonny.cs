using UnityEngine;
using UnityEngine.EventSystems;

public class PressTheJonny : Level, IPointerDownHandler
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform redCircle;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            eventData.position,
            eventData.pressEventCamera, // important for WebGL + camera canvases
            out Vector2 localPos
        );

        redCircle.anchoredPosition = localPos;
    }

    public override bool Verify()
    {
        return Vector2.Distance(redCircle.anchoredPosition, new Vector2(88.8f, 78.7f)) < 30f;
    }
}