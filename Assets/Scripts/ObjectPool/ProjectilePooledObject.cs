using UnityEngine;

public class ProjectilePooledObject : MonoBehaviour
{
    private ProjectileObjectPool pool;
    public ProjectileObjectPool Pool { get => pool; set => pool = value; }

    public void Release()
    {
        pool.ReturnToPool(this);
    }
}
