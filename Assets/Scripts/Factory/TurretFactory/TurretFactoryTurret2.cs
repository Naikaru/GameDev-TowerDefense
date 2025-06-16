using UnityEngine;

public class TurretFactoryTurret2 : TurretFactory
{
    [SerializeField] private TurretProductTurret2 productPrefab;

    public override ITurretProduct GetProduct(Vector3 position)
    {
        // Fix Turret Rotation (Blender design error) : Rotate by -90 given X Axis
        var instance = Instantiate(productPrefab.gameObject, position, Quaternion.Euler(-89.98f, 0, 180f));
        TurretProductTurret2 product = instance.GetComponent<TurretProductTurret2>();
        product.Initialize();
        return product;
    }
    public override int GetProductCost()
    {
        return TurretProductTurret2.BaseCost;
    }
}