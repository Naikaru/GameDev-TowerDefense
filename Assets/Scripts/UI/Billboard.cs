using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] bool reverse = false;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        if (reverse)
        {
            transform.Rotate(0, 180, 0);
        }
        // transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}