using UnityEngine;

public class GrassAnim : MonoBehaviour
{
    [SerializeField] Vector3 amount;
    [SerializeField] float time;

    private float randomTime;

    void Start()
    {
        float randomTime = Random.Range(time - 0.5f, time + 0.5f);

        Invoke("Animate", randomTime / 2);
    }
    
    void Animate()
    {
        iTween.PunchScale(gameObject, iTween.Hash(
            "amount", amount,
            "time", randomTime,
            "looptype", iTween.LoopType.loop
        ));
    }
}
