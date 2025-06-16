using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    [SerializeField] float fireTickRate = 0.1f;
    [SerializeField] ParticleSystem particleSystem;
    private ITurretProduct turret;
    private List<IMobProduct> mobs;
    private bool isFiring = false;
    private WaitForSeconds wait;
    public bool IsFiring => isFiring;
    Coroutine fireCoroutine;

    void Awake()
    {
        turret = GetComponentInParent<ITurretProduct>();
        mobs = new List<IMobProduct>();
        particleSystem.Stop();
        wait = new WaitForSeconds(fireTickRate);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mob"))
        {
            if (other.gameObject.TryGetComponent(out IMobProduct mob))
            {
                mobs.Add(mob);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mob"))
        {
            if (other.gameObject.TryGetComponent(out IMobProduct mob))
            {
                mobs.Remove(mob);
            }
        }
    }

    public void StartFiring()
    {
        fireCoroutine = StartCoroutine(FireCoroutine());
        isFiring = true;
        particleSystem.Play();
    }

    public void StopFiring()
    {
        if (fireCoroutine is not null) StopCoroutine(fireCoroutine);
        isFiring = false;
        particleSystem.Stop();
    }

    private IEnumerator FireCoroutine()
    {
        List<int> removeIdx = new List<int>();

        while (true)
        {
            removeIdx.Clear();

            for (int i = 0; i < mobs.Count; i++)
            {
                // All mobs take damage
                bool isDead = mobs.ElementAt(i).TakeDamage(turret.TurretPower);

                if (isDead)
                {
                    removeIdx.Add(i);
                }
            }

            foreach (int idx in removeIdx)
            {
                mobs.RemoveAt(idx);
            }

            yield return wait;
        }
    }
}