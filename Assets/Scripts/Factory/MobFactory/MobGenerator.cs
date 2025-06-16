using System.Collections;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    [SerializeField]
    MobGeneratorScriptableObject mobGeneratorData;
    [SerializeField]
    MobFactory[] factories;
    [SerializeField]
    string generatorPositionObjectName = "GeneratorPosition";

    public static Transform generator;
    // Properties
    private bool wavesEnded = false;
    // Subject
    MobGeneratorSubject mobGeneratorSubject;

    private void Start()
    {
        // Set up subject
        mobGeneratorSubject = GetComponent<MobGeneratorSubject>();

        // Set up generator
        generator = GameObject.Find(generatorPositionObjectName).transform;

        // Start Coroutine
        StartCoroutine(GenerateMobs());
    }

    private IEnumerator GenerateMobs()
    {
        yield return new WaitForSeconds(mobGeneratorData.delayBeforeFirstWave);

        int waveIndex = 0;
        // Waves
        foreach (int wave in mobGeneratorData.mobsToSpawn)
        {
            // Start Wave
            waveIndex ++;
            mobGeneratorSubject.StartWave(waveIndex);

            yield return new WaitForSeconds(mobGeneratorData.delayBetweenWaves);
            // current wave's mobs
            for (int i = 0; i < mobGeneratorData.mobsPerWaves; i++)
            {
                yield return new WaitForSeconds(mobGeneratorData.delayBetweenMobs);
                IMobProduct mobProduct = factories[i%2].GetProduct(mobGeneratorData.instantiationPosition);
                // OR
                //IMobProduct mobProduct = factories[wave].GetProduct(generator.position);
                // Notify mob instantiation
                mobGeneratorSubject.CreateMob(mobProduct.gameObject);
            }

            // Bonus Gold at each wave end
            GoldManager.Instance.UpdateGold(25);
        }
        wavesEnded = true;
    }
}
