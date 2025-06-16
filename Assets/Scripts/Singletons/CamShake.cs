using System;
using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private static CamShake instance;

    public static CamShake Instance {
        get
        {
            return instance;
            // no need for SetupInstance as there is no configuration needed at init
        }
    }

    // Singleton Pattern
    void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool shakingCam;

    // Main Camera Controlled by an Animator (has to be deactivated on screen shakes)
    [SerializeField] Animator animator;

    void Start()
    {
        animator ??= GetComponent<Animator>();
    }

    public void Shake(float duration, float amount, float intensity)
    {
        if (!shakingCam)
        {
            try
            {
                StartCoroutine(ShakeCamCoroutine(duration, amount, intensity));
            }
            catch(Exception ex)
            {
                print(ex.Message);
            }
        }
    }

    IEnumerator ShakeCamCoroutine(float duration, float amount, float intensity)
    {
        float elapsedTime = 0f;
        Vector3 startPos = Camera.main.transform.localPosition;
        Vector3 targetPos = Vector3.zero;
        shakingCam = true;
        animator.enabled = false;
        // Start Looping
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // Compute target position
            if (targetPos == Vector3.zero)
            {
                targetPos = startPos + UnityEngine.Random.insideUnitSphere * amount;
            }
            // Smoothing effect depend on intensity
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, targetPos, intensity * Time.deltaTime);
            // Reset target position (will set a new one if elapsedTime < duration)
            if (Vector3.Distance(Camera.main.transform.localPosition, targetPos) < 0.02f)
            {
                targetPos = Vector3.zero;
            }
            yield return null;
        }

        Camera.main.transform.localPosition = startPos;
        shakingCam = false;
        animator.enabled = true;
    }
}
