using UnityEngine;

public class MobFactoryCactus : MobFactory
{
    [SerializeField]
    private GameObject m_ProductPrefab;

    public override IMobProduct GetProduct(Vector3 position)
    {
        GameObject instance = Instantiate(m_ProductPrefab, position, Quaternion.identity);
        // Modify mob's parent
        instance.transform.parent = MobGenerator.generator;

        MobProductCactus mobProduct = instance.GetComponent<MobProductCactus>();
        mobProduct.Initialize();

        return mobProduct;
    }
}
