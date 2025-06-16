using System;
using TMPro;
using UnityEngine;

public class WaveObserver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private MobGeneratorSubject mobGeneratorSubject;
    [SerializeField] private MobGeneratorScriptableObject mobGeneratorData;
    private string waveTextTemplate;

    void Awake()
    {
        waveTextTemplate =  "Wave {0} / " + mobGeneratorData.mobsToSpawn.Length.ToString();
        mobGeneratorSubject ??= FindAnyObjectByType<MobGeneratorSubject>();        
    }
    void Start()
    {
        mobGeneratorSubject.WaveStarted += OnWaveStarted;        
    }

    void OnDestroy()
    {
        mobGeneratorSubject.WaveStarted -= OnWaveStarted;
    }

    void OnWaveStarted()
    {
        string currWaveText = String.Format(waveTextTemplate, mobGeneratorSubject.currWave.ToString());
        waveText.SetText(currWaveText);
    }
}
