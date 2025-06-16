using UnityEngine;

public class MobFactoryChampi : MobFactory
{
    [SerializeField] private MobProductChampi m_ProductPrefab;
    public override IMobProduct GetProduct(Vector3 position)
    {
        GameObject instance = Instantiate(m_ProductPrefab.gameObject, position, Quaternion.identity);
        // Modify mob's parent
        instance.transform.parent = MobGenerator.generator;

        // Create Product of type IMobProduct
        MobProductChampi mobProduct = instance.GetComponent<MobProductChampi>();
        mobProduct.Initialize();

        return mobProduct;
    }
}
