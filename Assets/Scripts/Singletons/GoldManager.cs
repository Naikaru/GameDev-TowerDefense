using System.Linq;
using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    private static GoldManager instance;

    public static GoldManager Instance
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

    // Properties
    [SerializeField] private TextMeshProUGUI m_GoldText;
    [SerializeField] private int m_Gold = 50;      // Start game with 100 golds

    public int Gold { get => m_Gold; set => m_Gold = value; }

    [SerializeField] private GameObject m_GoldExpensePrefab;
    [SerializeField] private GameObject m_GoldExpenseParent;
    private Vector3 m_GoldExpensePosition;
    private Vector3 m_GoldExpenseSizeDelta;

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
        instance = FindFirstObjectByType<GoldManager>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GoldManager_Singleton";
            instance = gameObj.AddComponent<GoldManager>();
            instance.DisplayGold();
        }
    }

    private void Start()
    {
        var rectTransform = m_GoldExpenseParent.GetComponent<RectTransform>();
        m_GoldExpensePosition = rectTransform.anchoredPosition;
        m_GoldExpenseSizeDelta = rectTransform.sizeDelta;
        DisplayGold();
    }

    public void UpdateGold(int value)
    {
        if (m_Gold + value < 0)
        {
            throw new System.Exception("Impossible to update to negative gold amount");
        }
        m_Gold += value;
        DisplayGold();

        // Animations
        if (value > 0)
        {
            // Punch gold text scale on gains (could use clip)
            iTween.PunchScale(m_GoldText.gameObject, 2.5f * Vector3.one, 0.5f);
        }
        else
        {
            // Punch gold text scale on expenses
            GameObject go = Instantiate(m_GoldExpensePrefab, Vector3.zero, Quaternion.identity, m_GoldExpenseParent.transform);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            go.GetComponent<TextMeshProUGUI>().SetText(value.ToString());
            Animator animator = go.GetComponent<Animator>();
            // float destroyAfter = animator.runtimeAnimatorController.animationClips.First().length + 0.1f;
            float destroyAfter = animator.GetCurrentAnimatorStateInfo(0).length + 0.1f;
            Destroy(go, destroyAfter);
        }
    }

    private void DisplayGold()
    {
        m_GoldText.SetText(m_Gold.ToString());
    }
}
