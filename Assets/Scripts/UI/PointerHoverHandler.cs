using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;


    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        GUI.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Pass 'null' to the texture parameter to use the default system cursor.
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }
}
