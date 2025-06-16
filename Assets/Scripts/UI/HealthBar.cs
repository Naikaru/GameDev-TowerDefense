using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _timeToDrain = 0.25f;
    [SerializeField] private Gradient _healthBarGradient;
    private Image _image;
    private float _target = 1f;
    private Coroutine drainHealthBarCoroutine;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.fillAmount = _target;
        CheckHealthBarGradientAmount();
    }

    void OnDestroy()
    {
        if (drainHealthBarCoroutine is not null) StopCoroutine(drainHealthBarCoroutine);
    }

    public void UpdateHealthBar(float percentage)
    {
        _target = percentage;
        drainHealthBarCoroutine = StartCoroutine(DrainHealthBar());
        CheckHealthBarGradientAmount();
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;
        drainHealthBarCoroutine = StartCoroutine(DrainHealthBar());
        CheckHealthBarGradientAmount();
    }

    private IEnumerator DrainHealthBar()
    {
        // Get fill amount from our image
        float fillAmount = _image.fillAmount;
        // Current color
        Color currentColor = _image.color;
        Color targetColor = _healthBarGradient.Evaluate(_target);

        // Slowly decrease health
        float elapsedTime = 0f;
        while (elapsedTime < _timeToDrain)
        {
            elapsedTime += Time.deltaTime;
            // update fill amount every frame and control how much time it exactly takes (_timeToDrain seconds)
            _image.fillAmount = Mathf.Lerp(fillAmount, _target, elapsedTime/_timeToDrain);
            // update color
            CheckHealthBarGradientAmount();
            _image.color = Color.Lerp(currentColor, targetColor, elapsedTime/_timeToDrain);
            yield return null;
        }
    }

    private void CheckHealthBarGradientAmount()
    {
        _image.color = _healthBarGradient.Evaluate(_image.fillAmount);
    }

}