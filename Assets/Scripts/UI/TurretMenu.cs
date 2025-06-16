using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretMenu : MonoBehaviour, IDeselectHandler
{
    // TODO: Check optimization / good practice between
    // 1. (Done here) Applying TurretMenu directly to the TurretMenu UI object
    // 2. Use a reference to the UI object and control behaviour from outside
    // [SerializeField] GameObject menuUI;
    private static TurretMenu instance;
    private bool isDisplayed;
    public bool IsDisplayed { get; }

    public static TurretMenu Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    public ITurretProduct turret;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private static void SetupInstance()
    {
        instance = FindFirstObjectByType<TurretMenu>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "TurretMenu_Singleton";
            instance = gameObj.AddComponent<TurretMenu>();
        }
    }

    // /!\ UI Button OnClick event only functions with a single argument
    // use Custom event definition
    /*

    public Button btn;

    void OnEnable()
    {
        //Register Button Events
        btn.onClick.AddListener(() => buttonCallBack("Hello Affan", 88));
    }

    private void buttonCallBack(string myStringValue, int myIntValue)
    {
        Debug.Log("Button Clicked. Received string: " + myStringValue + " with int: " + myIntValue);
    }

    void OnDisable()
    {
        //Un-Register Button Events
        btn.onClick.RemoveAllListeners();
    }
    */

    [SerializeField] TurretMenuItem upgradeMenuItem;
    [SerializeField] TurretMenuItem sellMenuItem;

    private void Start()
    {
        if (upgradeMenuItem is null)
        {
            upgradeMenuItem = GetComponentsInChildren<TurretMenuItem>().Where(item => item.ItemType == TurretMenuItemType.Upgrade).FirstOrDefault();
        }

        if (sellMenuItem is null)
        {
            sellMenuItem = GetComponentsInChildren<TurretMenuItem>().Where(item => item.ItemType == TurretMenuItemType.Sell).FirstOrDefault();
        }

        // Hide interface
        Hide();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Mouse was clicked outside
        HideMenu();
    }

    public void ShowMenu(float delay = 0.1f)
    {
        SetMenuValues();

        // Invoke Show function with delay
        Invoke(nameof(Show), delay);
    }

    public void HideMenu(float delay = 0.1f)
    {
        // Invoke Show function with delay
        Invoke(nameof(Hide), delay);
    }

    private void Show()
    {
        isDisplayed = true;
        // Center menu to the turret game object
        // gameObject.transform.position = turret.gameObject.transform.position;
        // Set visible
        gameObject.SetActive(true);

        // Set the GameObject as selected
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void Hide()
    {
        isDisplayed = false;
        gameObject.SetActive(false);
        // Make sure cursor is reset to null
        // Pass 'null' to the texture parameter to use the default system cursor.
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // Set the GameObject as selected
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetTurret(ITurretProduct product)
    {
        turret = product;
    }


    private void SetMenuUpgradeValue()
    {
        if (turret != null && turret.TurretCanUpgrade)
        {
            upgradeMenuItem.gameObject.SetActive(true);
            upgradeMenuItem.Reset();
            upgradeMenuItem.SetValue(turret.TurretUpgradeCost, false);
        }
        else
        {
            upgradeMenuItem.gameObject.SetActive(false);
        }
    }

    public void Upgrade()
    {
        // TODO: handle this error (should not be able to call Upgrade if turret is null)
        if (turret == null)
            return;

        if (turret.TurretCanUpgrade && GoldManager.Instance.Gold >= turret.TurretUpgradeCost)
        {
            // Spend golds
            GoldManager.Instance.UpdateGold(-turret.TurretUpgradeCost);
            // Upgrade turret stats
            turret.Upgrade();
            // Play upgrade sfx
            SFXObserver.Instance.PlayUpgradeTurretSfx();
            // Play upgrade vfx
            turret.gameObject.GetComponent<TurretController>().PlayUpgradeVfx();
        }
    }

    private void SetMenuSellValue()
    {
        if (turret != null)
        {
            sellMenuItem.Reset();
            int goldSell = (int)Mathf.Ceil(0.6f * turret.TurretCost);
            sellMenuItem.SetValue(goldSell, true);
        }
    }

    public void Sell()
    {
        // Refund at rate of 60%
        int goldSell = (int)Mathf.Ceil(0.6f * turret.TurretCost);
        GoldManager.Instance.UpdateGold(goldSell);
        // Audio
        SFXObserver.Instance.PlaySellTurretSfx();
        // Destroy
        turret.gameObject.GetComponent<TurretController>().Sell();
    }

    void SetMenuValues()
    {
        SetMenuUpgradeValue();
        SetMenuSellValue();
    }
}
