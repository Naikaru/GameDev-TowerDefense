using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // deactivate after delay
    [SerializeField] private float deactivateAfter = 3f;

    // reference to the PoolObject component so we can return this proj to the pool
    private ProjectilePooledObject pooledObject;
    private string type;                 // TODO: create TypeEnum
    private float damage;                  // Given by turret controller

    public string Type { get => type; set => type = value; }
    public float Damage { get => damage; set => damage = value; }

    void Awake()
    {
        pooledObject = GetComponent<ProjectilePooledObject>();
    }

    public void Deactivate()
    {
        // Start coroutine after deactivateAfter delay
        StartCoroutine(DeactivateRoutine(deactivateAfter));
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        // reset the moving Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;    // former velocity
        rb.angularVelocity = Vector3.zero;

        // set inactive and return to pool
        pooledObject.Release();
        gameObject.SetActive(false);
    }
}
