using TMPro;
using UnityEngine;

public class MobGeneratorObserver : MonoBehaviour
{
    [SerializeField] MobGeneratorScriptableObject mobGeneratorData;
    [SerializeField] TowerSubject towerSubject;
    [SerializeField] TextMeshProUGUI endText;

    // Level ends on destroyedMobs == totalMobs (== mobGeneratorData.totalMobs)
    int totalMobs;
    int destroyedMobs;

    private void Awake()
    {
        towerSubject ??= FindAnyObjectByType<TowerSubject>();
    }

    private void Start()
    {
        totalMobs = mobGeneratorData.mobsToSpawn.Length * mobGeneratorData.mobsPerWaves;
        towerSubject.MobDestroyed += OnMobDestroyed;
        towerSubject.TowerDamaged += OnTowerDamaged;
        
    }

    private void OnDestroy()
    {
        towerSubject.MobDestroyed -= OnMobDestroyed;
        towerSubject.TowerDamaged -= OnTowerDamaged;
    }

    private void OnMobDestroyed()
    {
        destroyedMobs++;
        if (destroyedMobs >= totalMobs && towerSubject.health > 0)
        {
            endText.SetText("Victory !");
            endText.gameObject.SetActive(true);
        }
        // TODO: all mob should stop moving and stay in IDLE state
    }

    private void OnTowerDamaged()
    {
        if (towerSubject.health <= 0)
        {
            // Destroy self obj in order to stop mob generation
            Destroy(gameObject);
        }
    }
}
