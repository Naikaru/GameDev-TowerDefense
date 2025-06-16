using UnityEngine;

public class TurretFactoryTurret1 : TurretFactory
{
    [SerializeField] private TurretProductTurret1 productPrefab;

    public override ITurretProduct GetProduct(Vector3 position)
    {
        // Fix Turret Rotation (Blender design error) : Rotate by -90 given X Axis
        var instance = Instantiate(productPrefab.gameObject, position, Quaternion.Euler(-90f, 0, 180f));
        TurretProductTurret1 product = instance.GetComponent<TurretProductTurret1>();
        product.Initialize();
        return product;
    }

    public override int GetProductCost()
    {
        return TurretProductTurret1.BaseCost;
    }
}