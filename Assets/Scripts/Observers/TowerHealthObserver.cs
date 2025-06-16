using UnityEngine;

public class TowerHealthObserver : MonoBehaviour
{
    [SerializeField] TowerSubject towerSubject;
    [SerializeField] GameObject volumeHealtLow;

    void Awake()
    {
        towerSubject ??= FindAnyObjectByType<TowerSubject>();
    }
    void Start()
    {
        towerSubject.TowerDamaged += OnTowerDamaged;
    }

    void OnDestroy()
    {
        towerSubject.TowerDamaged -= OnTowerDamaged;
    }

    void OnTowerDamaged()
    {
        // Change global volume Settings on low health
        if (towerSubject.health <= 1)
        {
            volumeHealtLow.SetActive(true);
        }
    }

    void OnTowerHealed()
    {
        if (towerSubject.health > 1)
        {
            volumeHealtLow.SetActive(false);
        }
    }
}
