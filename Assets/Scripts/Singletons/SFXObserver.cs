using UnityEngine;

public class SFXObserver : MonoBehaviour
{
    [SerializeField] MobGeneratorSubject mobGeneratorSubject;
    [SerializeField] TowerSubject towerSubject;
    [SerializeField] AudioClip onMobGeneratedSfx;
    [SerializeField] AudioClip onMobDestroyedSfx;
    [SerializeField] AudioClip onTowerDamagedSfx;
    [SerializeField] AudioClip turretFireSfx;
    [SerializeField] AudioClip clickSfx;
    [SerializeField] AudioClip buildTurretSfx;
    [SerializeField] AudioClip upgradeTurretSfx;
    [SerializeField] AudioClip sellTurretSfx;

    [SerializeField] AudioSource audioSourceMusic;
    AudioSource audioSource;

    // Singleton
    private static SFXObserver instance;

    public static SFXObserver Instance
    {
        get 
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;    
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Init properties
        mobGeneratorSubject ??= FindAnyObjectByType<MobGeneratorSubject>();
        towerSubject ??= FindAnyObjectByType<TowerSubject>();
        audioSource = GetComponent<AudioSource>();

    }

    public static void SetupInstance()
    {
        instance = FindFirstObjectByType<SFXObserver>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "SFXObserver_Singleton";
            instance = gameObj.AddComponent<SFXObserver>();
        }
    }

    void Start()
    {
        mobGeneratorSubject.MobGenerated += OnMobGenerated;
        // towerSubject.MobDestroyed += OnMobDestroyed; 
        towerSubject.TowerDamaged += OnTowerDamaged; 
        // audio source settings
        audioSourceMusic.volume = PlayerPrefs.GetFloat("musicVolume", 0.25f);
        audioSource.volume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
    }

    void OnDestroy()
    {
        mobGeneratorSubject.MobGenerated -= OnMobGenerated;
        // towerSubject.MobDestroyed -= OnMobDestroyed; 
        towerSubject.TowerDamaged -= OnTowerDamaged; 
    }

    void OnMobGenerated()
    {
        audioSource.PlayOneShot(onMobGeneratedSfx);
    }

    void OnMobDestroyed()
    {
        audioSource.PlayOneShot(onMobDestroyedSfx);
    }

    void OnTowerDamaged()
    {
        audioSource.PlayOneShot(onTowerDamagedSfx);
    }

    public void PlayTurretFireSfx()
    {
        audioSource.PlayOneShot(turretFireSfx);
    }

    public void PlayClickSfx()
    {
        audioSource.PlayOneShot(clickSfx);
    }

    public void PlayBuildTurretSfx()
    {
        audioSource.PlayOneShot(buildTurretSfx);
    }
    public void PlaySellTurretSfx()
    {
        audioSource.PlayOneShot(sellTurretSfx);
    }
    public void PlayUpgradeTurretSfx()
    {
        audioSource.PlayOneShot(upgradeTurretSfx);
    }
}
