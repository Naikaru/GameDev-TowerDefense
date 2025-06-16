using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    private void Start()
    {
        // Select first item
        GetComponentInChildren<ShopItem>().Select();
    }

    public void Select(GameObject item)
    {
        item.GetComponent<ShopItem>().Select();
    }

    public void Unselect(GameObject item)
    {
        item.GetComponent<ShopItem>().Unselect();
    }
}
