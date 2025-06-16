using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobProductChampi : MonoBehaviour, IMobProduct
{
    private float mobHealth = 3;
    [SerializeField] private float mobMaxHealth = 3;
    [SerializeField] private string mobType = "normal";         // TODO: Add MobType enum
    [SerializeField] private float mobSpeed = 2f;
    [SerializeField] private int mobGoldReward = 5;
    [SerializeField] private Transform[] pathPoints = null;     // In complement to Nav Mesh Surface
    [SerializeField] private float steeringRadius = 0.5f;       // Distance at which the agent can go to the next point
    [SerializeField] private string target = "Tower";           // Final target position (the Tower)
    [SerializeField] private string pathPointsObjectName = "PathPoints";        // Path Points object name in scene
    [SerializeField] private HealthBar healthBar;               // Mob HealthBar (set on a canvas image)
    [SerializeField] private Material onHitEffectMaterial;             // Material to apply instead of mob materials

    private NavMeshAgent m_Agent;
    private Transform m_Target;
    private bool IsTargetSet = false;
    private SkinnedMeshRenderer rend; 
    private List<Material> materials;                               // Mob Renderer Materials

    public float MobHealth { get => mobHealth; set => mobHealth = value; }
    public float MobMaxHealth { get => mobMaxHealth; set => mobMaxHealth = value; }
    public string MobType { get => mobType; set => mobType = value; }
    public float MobSpeed { get => mobSpeed; set => mobSpeed = value; }
    public int MobGoldReward { get => mobGoldReward; set => mobGoldReward = value; }

    public void Initialize()
    {
        // Set Health 
        mobHealth = mobMaxHealth;
        // Health bar
        healthBar = GetComponentInChildren<HealthBar>();
        // healthBarImage.color = GetGradientColor();

        m_Agent = gameObject.AddComponent<NavMeshAgent>();
        m_Agent.speed = MobSpeed;
        m_Agent.radius = 0.35f;
        if (pathPoints == null || pathPoints.Length == 0)
        {
            // Get all path points transforms
            pathPoints = GameObject.Find(pathPointsObjectName).GetComponentsInChildren<Transform>();
        }
        if (pathPoints != null && pathPoints.Length > 0)
        {
            // For this object, setup a random point from path as a mandatory destination before reaching the target
            m_Agent.SetDestination(pathPoints[UnityEngine.Random.Range(0, pathPoints.Length)].position);
        }
        // Setup final target transform
        m_Target = GameObject.Find(target).transform;

        // Renderer
        rend = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        materials = new List<Material>(rend.materials);

        InvokeRepeating(nameof(GoToTarget), 3, 0.5f);
    }

    public void GoToTarget()
    {
        if (!IsTargetSet)
        {
            float dist = m_Agent.remainingDistance;
            // Steering
            if (dist < steeringRadius)
            {
                // Set final destination
                m_Agent.SetDestination(m_Target.position);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && mobHealth > 0)
        {
            /*
            // Could vary damages taken given the projectile type for effectiveness
            var bullet = other.gameObject.GetComponent<Bullet>();
            var dmgMultiplier = 1; 
            if (mobWeakTypes.Contains(bullet.type)
            {
                dmgMultiplier = 2;
            }
            mobHealth -= bullet.dmg * dmgMultiplier;
            */
            float damage = other.gameObject.GetComponent<Bullet>().Damage;
            TakeDamage(damage);
        }
    }

    public bool TakeDamage(float damage)
    {
        mobHealth -= damage;
        // Health bar (decrease bar x scale)
        healthBar?.UpdateHealthBar(mobMaxHealth, mobHealth);
        // healthBar.localScale = new Vector3(mobHealth * healthPointValue, 1, 1);
        // healthBarImage.color = GetGradientColor(mobHealth * healthPointValue);
        // White flash
        rend.material = onHitEffectMaterial;
        StartCoroutine(ResetMaterialsCoroutine());

        bool isDead = mobHealth <= 0;
        if (isDead)
        {
            try
            {
                TowerSubject.Instance.DestroyMob(gameObject);
                GoldManager.Instance.UpdateGold(mobGoldReward);
            }
            catch (Exception ex)
            {
                // Global exception in case multiple Projectiles collisions are triggered at the same time
                print(ex.Message);
            }
        }

        return isDead;
    }

    private Color GetGradientColor(float ratio)
    {
        // Texture2D gradientMap = (Texture2D)healthBarMat.mainTexture;
        // int x = Mathf.FloorToInt(ratio * gradientMap.width);
        // int z = Mathf.FloorToInt(ratio * gradientMap.height);
        // Color color = gradientMap.GetPixel(x, z); 
        Color color = Color.Lerp(Color.red, Color.green, ratio);
        return color;
    }

    private IEnumerator ResetMaterialsCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        rend.SetMaterials(materials);
    }
}
