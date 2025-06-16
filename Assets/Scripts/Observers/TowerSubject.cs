using System;
using TMPro;
using UnityEngine;

public class TowerSubject : MonoBehaviour
{
    // user accessible vars
    [SerializeField]
    private TMP_Text healthText;            // TMP_Text since its a 3D object text
    [SerializeField]
    private TextMeshProUGUI endText;        // TextMeshProUGUI since its a text on a Canvas
    [SerializeField]
    private TextMeshProUGUI endTextRestart;        // TextMeshProUGUI since its a text on a Canvas
    [SerializeField]
    private GameObject[] onDestroyParticlesEffects = null;

    // Subject Design Pattern Action on Mob Destroy
    public event Action MobDestroyed;

    // Subject Design Pattern Action on Tower takes damages
    public event Action TowerDamaged;

    // Subject Design Pattern Action on Tower regain health
    public event Action TowerHealed;

    // Properties
    public GameObject lastDestroyedMob;     // 
    public int health = 3;                  // Tower health

    private static TowerSubject instance;

    public static TowerSubject Instance {  get { return instance; } }

    private void Awake()
    {
        // Ensure instance unicity across the scene
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Display tower health
        healthText.text = health.ToString();
    }

    public void DestroyMob(GameObject mob)
    {
        if (mob != null)
        {
            // Update last destroyed mob
            lastDestroyedMob = mob;
            // Invoke action on destroy
            MobDestroyed?.Invoke();
            // Instanciate Particles Effects
            if (onDestroyParticlesEffects != null && onDestroyParticlesEffects.Length > 0)
            {
                foreach (var effect in onDestroyParticlesEffects)
                {
                    Instantiate(effect, mob.transform.position + 0.5f*Vector3.up, mob.transform.rotation);
                }
            }
            // Actually destroy the mob
            Destroy(mob, .1f);
        }
    }

    private void TakeDamages()
    {
        // One damage
        // TODO: damages taken should depend on the mob that reached the Tower
        health--;
        if (health < 0) health = 0;
        healthText.text = health.ToString();
        TowerDamaged?.Invoke();
        // ScreenShake
        CamShake.Instance.Shake(0.2f, 0.75f, 1.5f);
        // Shape Animation
        iTween.PunchScale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.75f);
        
        // Handle low health >> TODO: Handle by TowerHealthObserver ?
        if (health <= 0)
        {
            endText.text = "Game Over !";
            endText.gameObject.SetActive(true);
            endTextRestart.gameObject.SetActive(true);
        }
    }

    // Handle Collisions
    // In order to handle collisions we had to add both Box Collision to the Tower and RigidBody (?) to the Mobs
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Mob"))
        {
            TakeDamages();
            DestroyMob(collider.gameObject);
        }
    }
}
