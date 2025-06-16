using System;
using System.Collections.Generic;
using UnityEngine;

public enum TurretType
{
    Projectile,
    Flamethrower    
}

public class TurretController : MonoBehaviour
{
    // Turret Factory
    private ITurretProduct turretProduct;

    // Exposed Variables
    [SerializeField] private TurretType type;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform firePos;
    [SerializeField] private float fireDelay = 1f;
    [SerializeField] private float fireForce = 400f;
    [SerializeField] private ProjectileObjectPool objectPool;
    [SerializeField] private Flamethrower flamethrower;


    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem buildVfx;
    [SerializeField] private ParticleSystem upgradeVfx;
    [SerializeField] private Transform sellVfxTransform;
    [SerializeField] private GameObject sellVfxPrefab;

    [Header("Outline Effect")]
    private Renderer[] renderers;

    // Private Variables
    private List<GameObject> mobs = new List<GameObject>();
    private Transform target;
    public Action DestroyEvent;

    void Awake()
    {
        // Init Turret Factory
        turretProduct ??= GetComponent<ITurretProduct>();

        // TurretType 'Weapon'
        if (type == TurretType.Projectile)
        {
            // Get Projectle Pool
            objectPool ??= FindAnyObjectByType<ProjectileObjectPool>();
        }
        if (type == TurretType.Flamethrower)
        {
            // Get Flamethrower in Children
            flamethrower = GetComponentInChildren<Flamethrower>();
        }

        // Renderers
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        // Visual Effects
        PlayBuildVfx();
        // Handle Turrets Behaviour every 0.5s
        InvokeRepeating(nameof(HandleBehaviour), 0.5f, 0.5f);
        // Firing rate of 1s
        InvokeRepeating(nameof(Fire), fireDelay, fireDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 pos = target.position;
            // do not rotate through y axis
            pos.y = pivot.position.y;
            pivot.LookAt(pos);
        }
    }

    void OnDestroy()
    {
        // Remove renderers from outline
        var outline = GetComponentInParent<QuickOutline>();
        if (outline is not null)
        {
            foreach (Renderer rend in renderers)
            {
                outline.RemoveRenderer(rend);
            }
        }
    }

    // On mouse click >>> since we moved turret to tile child, should be able to call parent's event or set propagation ? 
    // private void OnMouseUp()
    // {
    //     TurretMenu.Instance.SetTurret(turretProduct);
    //     TurretMenu.Instance.ShowMenu();
    // }

    void SearchTarget()
    {
        foreach (var mob in mobs)
        {
            if (mob != null && !target && Vector3.Distance(transform.position, mob.transform.position) <= turretProduct.TurretRadius)
            {
                target = mob.transform;
                // take first target and exit loop
                break;
            }
        }
    }

    void HandleBehaviour()
    {
        if (!target)
        {
            SearchTarget();
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > turretProduct.TurretRadius)
            {
                target = null;
            }
        }
    }

    void Fire()
    {
        if (type == TurretType.Projectile && target)
        {
            // Get projectile from PooledObject
            GameObject projectileObj = objectPool.GetPooledObject().gameObject;

            // could not get a projectile instance 
            if (projectileObj == null)
            {
                return;
            }

            // Firing Direction
            // Vector3 fireDir = pivot.forward;
            Vector3 fireDir = target.position - transform.position;
            // Activate projectile
            projectileObj.SetActive(true);
            // Reset projectile position and rotation to the turret's muzzle/barrel position and rotation
            projectileObj.transform.SetPositionAndRotation(firePos.position, firePos.rotation);
            // Move projectile forward
            projectileObj.GetComponent<Rigidbody>().AddForce(fireDir * fireForce, ForceMode.Acceleration);

            // Bullet properties
            Bullet bullet = projectileObj.GetComponent<Bullet>();
            bullet.Type = turretProduct.TurretType;
            bullet.Damage = turretProduct.TurretPower;
            bullet?.Deactivate();

            // // Testing
            // GameObject go = Instantiate(bullet, firePos.position, firePos.rotation);
            // go.GetComponent<Rigidbody>().AddForce(go.transform.forward * fireForce, ForceMode.Acceleration);

            // Audio
            SFXObserver.Instance.PlayTurretFireSfx();
        }

        if (type == TurretType.Flamethrower)
        {
            if (flamethrower.IsFiring && !target)
            {
                flamethrower.StopFiring();
            }

            if (target && !flamethrower.IsFiring)
            {
                flamethrower.StartFiring();
            }
        }
    }

    public void AddMob(GameObject mob)
    {
        mobs.Add(mob);
    }

    public void RemoveMob(GameObject mob)
    {
        mobs.Remove(mob);
    }

    public void Sell()
    {
        // Visual Effects
        Instantiate(sellVfxPrefab, sellVfxTransform.position, transform.rotation);
        // Emit Event
        DestroyEvent.Invoke();
        // Detroy Game Object
        Destroy(gameObject, 0.1f);
    }

    public void PlayBuildVfx()
    {
        buildVfx.gameObject.SetActive(true);
        buildVfx.Play();
    }

    public void PlayUpgradeVfx()
    {
        upgradeVfx.gameObject.SetActive(true);
        upgradeVfx.Play();
    }
}
