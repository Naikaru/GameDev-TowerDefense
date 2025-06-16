using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TurretMenuItemType
{
    Upgrade,
    Sell
}

public class TurretMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TurretMenuItemType itemType;
    public TurretMenuItemType ItemType => itemType;
    [SerializeField] Color overImageColor;
    [SerializeField] TMP_Text valueText;
    [SerializeField] Image image;

    Color baseImageColor;
    Image overImageOutline;

    void Awake()
    {
        overImageOutline = GetComponent<Image>();
        baseImageColor = image.color;
        Reset();
    }

    void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        overImageOutline.enabled = false;
        image.color = baseImageColor;
    }

    public void SetValue(int value, bool isPositive)
    {
        valueText.SetText((isPositive ? "+" : "-") + value.ToString());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overImageOutline.enabled = true;
        image.color = overImageColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overImageOutline.enabled = false;
        image.color = baseImageColor;
    }
}
