using UnityEngine;

public abstract class MobFactory : MonoBehaviour
{

    // Ask the factory to create a product at a certain position
    public abstract IMobProduct GetProduct(Vector3 position);
}
