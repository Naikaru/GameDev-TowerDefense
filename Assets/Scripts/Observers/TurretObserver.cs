using UnityEngine;

public class TurretObserver : MonoBehaviour
{
    [SerializeField] MobGeneratorSubject mobGeneratorSubject;
    [SerializeField] TowerSubject towerSubject;
    [SerializeField] TurretController turretController;
    [SerializeField] GameObject[] onDestroyEffects;

    void Awake()
    {
        mobGeneratorSubject ??= FindAnyObjectByType<MobGeneratorSubject>();
        towerSubject ??= FindAnyObjectByType<TowerSubject>();
        turretController ??= GetComponent<TurretController>();
    }

    void Start()
    {
        mobGeneratorSubject.MobGenerated += OnMobGenerate;
        towerSubject.MobDestroyed += OnMobDestroyed; 
        towerSubject.TowerDamaged += OnTowerDamaged;
    }

    void OnDestroy()
    {
        mobGeneratorSubject.MobGenerated -= OnMobGenerate;
        towerSubject.MobDestroyed -= OnMobDestroyed; 
        towerSubject.TowerDamaged -= OnTowerDamaged;
    }

    void OnMobGenerate()
    {
        turretController.AddMob(mobGeneratorSubject.lastCreatedMob);
    }

    void OnMobDestroyed()
    {
        if (towerSubject.lastDestroyedMob != null)
        {
            turretController.RemoveMob(towerSubject.lastDestroyedMob);
        }
    }

    void OnTowerDamaged()
    {
        if (towerSubject.health <= 0)
        {
            foreach(var effect in onDestroyEffects)
            {
                Instantiate(effect, transform.position, Quaternion.identity);                
            }
            Destroy(gameObject);
        }
    }
}
