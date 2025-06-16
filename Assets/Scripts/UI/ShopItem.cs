using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline[] outlines;

    private Color outlineDefaultEffectColor = new Color(0, 0.1764706f, 0.3215686f, .5f);
    // private Color outlineOverEffectColor = new Color(1, 1, 1, .5f);
    private Color outlineOverEffectColor = new Color(0.7764706f, 0.5450981f, 0.627451f, 0.6f);
    // private Color outlineSelectedEffectColor = new Color(1, 1, 1, 1f);
    private Color outlineSelectedEffectColor = new Color(0.7764706f, 0.5450981f, 0.627451f, 1f);
    private Vector2 outlineDefaultEffectDistance = new Vector2(2, -2);
    private Vector2 outlineOverEffectDistance = new Vector2(3, -3);


    private bool isSelected = false;

    private void Awake()
    {
        outlines = GetComponents<Outline>().Concat(GetComponentsInChildren<Outline>()).ToArray();
        SetOutlines(outlineDefaultEffectColor, outlineDefaultEffectDistance);
    }

    public void Select()
    {
        isSelected = true;
        SetOutlines(outlineSelectedEffectColor, outlineDefaultEffectDistance);
    }

    public void Unselect()
    {
        isSelected = false;
        SetOutlines(outlineDefaultEffectColor, outlineDefaultEffectDistance);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetOutlines(outlineOverEffectColor, outlineOverEffectDistance);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) Select();
        else Unselect();
    }

    private void SetOutlines(Color color, Vector2 distance)
    {
        foreach (Outline outline in outlines)
        {
            outline.effectColor = color;
            outline.effectDistance = distance;
        }
    }
}
