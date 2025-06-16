using UnityEngine;

public abstract class TurretFactory : MonoBehaviour
{
    // Ask the factory to create a product at a certain position
    public abstract ITurretProduct GetProduct(Vector3 position);

    // Ask the product its base cost
    public abstract int GetProductCost();
}