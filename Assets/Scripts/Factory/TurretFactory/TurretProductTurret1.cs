using UnityEngine;

public class TurretProductTurret1 : MonoBehaviour, ITurretProduct
{
    public static int BaseCost = 25;

    private int turretCost = 25;
    private int turretUpgradeCost = 35;
    private string turretType = "base";
    private float turretPower = 1f;
    private float turretRadius = 5f;
    private uint turretLevel = 1;
    private uint turretMaxLevel = 2;
    [SerializeField] private GameObject[] turretPartsByLevel;
    // Ref to controller
    private TurretController turretController;

    public int TurretCost { get => turretCost; set => turretCost = value; }
    public string TurretType { get => turretType; set => turretType = value; }
    public float TurretPower { get => turretPower; set => turretPower = value; }
    public float TurretRadius { get => turretRadius; set => turretRadius = value; }
    public uint TurretLevel { get => turretLevel; set => turretLevel = value; }
    public int TurretUpgradeCost { get => turretUpgradeCost; set => turretUpgradeCost = value; }
    public bool TurretCanUpgrade { get => turretLevel < turretMaxLevel; }

    public void Initialize()
    {
        turretController = gameObject.GetComponent<TurretController>();
        // As turrets can be created at any time in the game,
        // Add previously generated "Mob" objects
        var mobs = GameObject.FindGameObjectsWithTag("Mob");
        foreach (var mob in mobs)
        {
            turretController.AddMob(mob);
        }
    }

    public void Upgrade()
    {
        turretLevel++;
        // Make use of ScriptableObject to define turrets properties according to turret's level
        // ScriptableObject turretData = ...
        // var levelData = turretData.levelProperties[turretLevel]
        // turret.property_0 = levelData.property_0
        // turret.property_1 = levelData.property_1
        // ...
        switch (turretLevel)
        {
            case 1:
                {
                    turretCost = 25;
                    turretUpgradeCost = 35;
                    turretPower = 1f;
                    turretRadius = 5f;
                    break;
                }
            case 2:
                {
                    turretCost = 35;
                    turretUpgradeCost = 100;
                    turretPower = 1.5f;
                    turretRadius = 7f;
                    break;
                }
            case 3:
                {
                    turretCost = 100;
                    turretUpgradeCost = 250;
                    turretPower = 2f;
                    turretRadius = 10f;
                    break;
                }
            default:
                {
                    break;
                }
        }
        // Update turret prefab        
        if (turretLevel > 1 && turretLevel <= turretPartsByLevel.Length)
        {
            // Deactivate previous level turret part prefab
            turretPartsByLevel[turretLevel - 2].SetActive(false);
            // Activate current level turret part prefab
            turretPartsByLevel[turretLevel - 1].SetActive(true);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TurretRadius);
    }
#endif

}