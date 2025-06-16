using UnityEngine;

public interface ITurretProduct 
{
    public int TurretCost { get; set; }
    public int TurretUpgradeCost { get; set; }
    public string TurretType { get; set; }
    public float TurretPower { get; set; }
    public float TurretRadius { get; set; }
    public uint TurretLevel { get; set; }
    public bool TurretCanUpgrade { get; }

    // Turret could also play on fire rate (which could be upgradable)

    // Initialize turret
    public void Initialize();

    // Upgrade turret
    public void Upgrade();    

    // Ref to the game object
    GameObject gameObject { get; }
}