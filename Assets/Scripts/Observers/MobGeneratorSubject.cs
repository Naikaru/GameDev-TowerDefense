using System;
using UnityEngine;

public class MobGeneratorSubject : MonoBehaviour
{
    public event Action MobGenerated;
    public event Action WaveStarted;
    public GameObject lastCreatedMob;
    public int currWave;

    public void CreateMob(GameObject mob)
    {
        lastCreatedMob = mob;
        MobGenerated?.Invoke();
    }

    public void StartWave(int wave)
    {
        currWave = wave;
        WaveStarted?.Invoke();
    }
}
