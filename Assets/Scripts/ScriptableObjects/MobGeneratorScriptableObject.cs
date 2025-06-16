using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MobGeneratorScriptableObject", order = 1)]
public class MobGeneratorScriptableObject : ScriptableObject
{
    // Mob Waves Properties
    public int[] mobsToSpawn;                   // Id array of factory mob to use each wave (ex: [1, 1, 2, 2, 5] for 5 waves)
    public int mobsPerWaves;                    // Number of mobs per wave
    public Vector3 instantiationPosition;       // Position where mobs are generated
    public float delayBetweenMobs;              // Delay between two mobs' spawn
    public float delayBeforeFirstWave;          // Delay before the first wave
    public float delayBetweenWaves;             // Delay between two waves
}
