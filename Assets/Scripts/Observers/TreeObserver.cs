using UnityEngine;

public class TreeObserver : MonoBehaviour
{
    [SerializeField] TowerSubject towerSubject;
    [SerializeField] Vector3 direction = Vector3.up;

    Vector3 position;

    void Awake()
    {
        towerSubject ??= FindAnyObjectByType<TowerSubject>();
        position = Vector3.Scale(transform.position, direction) + Vector3.Scale(Vector3.one, direction);
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
        if (towerSubject.health <= 0)
        {
            // Animation over trees
            iTween.PunchPosition(gameObject, position, 1.25f);
        }
    }
}
