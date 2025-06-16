using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveButtonPointerHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    Image image;
    Color baseColor;

    private void Start()
    {
        image = GetComponent<Image>();
        baseColor = image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = baseColor;
        color.a = 1;
        image.color = color;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = baseColor;
        color.a = 0.07843138f;
        image.color = color;
        // Pass 'null' to the texture parameter to use the default system cursor.
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }

    // int UILayer;

    // private void Start()
    // {
    //     UILayer = LayerMask.NameToLayer("UI");
    // }

    // private void Update()
    // {
    //     print(IsPointerOverUIElement() ? "Over UI" : "Not over UI");
    // }


    // //Returns 'true' if we touched or hovering on Unity UI element.
    // public bool IsPointerOverUIElement()
    // {
    //     return IsPointerOverUIElement(GetEventSystemRaycastResults());
    // }


    // //Returns 'true' if we touched or hovering on Unity UI element.
    // private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    // {
    //     for (int index = 0; index < eventSystemRaysastResults.Count; index++)
    //     {
    //         RaycastResult curRaysastResult = eventSystemRaysastResults[index];
    //         if (curRaysastResult.gameObject.layer == UILayer && curRaysastResult.gameObject == this.gameObject)
    //             return true;
    //     }
    //     return false;
    // }


    // //Gets all event system raycast results of current mouse or touch position.
    // static List<RaycastResult> GetEventSystemRaycastResults()
    // {
    //     PointerEventData eventData = new PointerEventData(EventSystem.current);
    //     eventData.position = Input.mousePosition;
    //     List<RaycastResult> raysastResults = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(eventData, raysastResults);
    //     return raysastResults;
    // }
}
