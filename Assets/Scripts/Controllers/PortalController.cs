using UnityEngine;

public class PortalController : MonoBehaviour
{
    // Observer over MobGenerator
    [SerializeField] MobGeneratorSubject mobGeneratorSubject;

    void Awake()
    {
        mobGeneratorSubject ??= FindAnyObjectByType<MobGeneratorSubject>();
    }
    void Start()
    {
        mobGeneratorSubject.MobGenerated += OnMobGenerated;
    }

    void OnDestroy()
    {
        mobGeneratorSubject.MobGenerated -= OnMobGenerated;
    }

    void OnMobGenerated()
    {
        // Punch Portal Scale
        iTween.PunchScale(gameObject, new Vector3(100, 100, 100), 1f);
    }
}
