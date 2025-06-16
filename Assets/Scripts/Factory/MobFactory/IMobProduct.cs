using UnityEngine;

public interface IMobProduct
{
    public float MobMaxHealth { get; set; }
    public float MobHealth { get; set; }
    public string MobType { get; set; }
    public float MobSpeed { get; set; }
    public int MobGoldReward { get; set; }

    // Initialize mob
    public void Initialize();

    public void GoToTarget();

    public bool TakeDamage(float damage);

    // Ref to the game object
    GameObject gameObject { get; }
}
