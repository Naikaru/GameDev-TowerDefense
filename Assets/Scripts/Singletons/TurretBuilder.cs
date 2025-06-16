using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuilder : MonoBehaviour
{
    private int factoryIdx = 0; // Selected turret
    [SerializeField] TurretFactory[] turretFactories;
    [SerializeField] GameObject[] turretPreviews;
    private GameObject turretPreview;
    private static TurretBuilder instance;

    public Action TurretBuildEvent;

    public static TurretBuilder Instance
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
    }

    public static void SetupInstance()
    {
        instance = FindFirstObjectByType<TurretBuilder>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "TurretBuilder_Singleton";
            instance = gameObj.AddComponent<TurretBuilder>();
        }
    }

    public ITurretProduct Build(Vector3 pos)
    {
        // Select appropriate factory
        TurretFactory turretFactory = turretFactories[factoryIdx];
        // Get Base Cost
        int turretCost = turretFactory.GetProductCost();

        if (GoldManager.Instance.Gold >= turretCost)
        {
            // Decrease gold
            GoldManager.Instance.UpdateGold(-turretCost);
            // Instantiate product
            ITurretProduct product = turretFactory.GetProduct(pos);
            // Play build sound effect
            SFXObserver.Instance.PlayBuildTurretSfx();
            // Fire event
            TurretBuildEvent?.Invoke();

            return product;
        }
        // Could not build the turret
        return null;
    }

    public void ShowPreview(Vector3 pos)
    {
         // Select appropriate object preview
        GameObject turret = turretPreviews[factoryIdx];
         // Position and show Preview
        turret.transform.position = pos;        
        turret.SetActive(true);
        turretPreview = turret;
    }

    public void HidePreview()
    {
        if (turretPreview is not null)
        {
            // Hide Preview
            turretPreview.SetActive(false);
        }
    }

    public void SelectFactory(int idx)
    {
        factoryIdx = idx;
    }
}
