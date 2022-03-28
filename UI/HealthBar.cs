using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthController healthController;

    [SerializeField] Image healthbarFill;

    private void Awake()
    {
        healthController.HealthChanged += OnHealthChanged; 
    }
    private void OnHealthChanged(float valueInPercentage)
    {
        healthbarFill.fillAmount = valueInPercentage;
    }
    private void OnDestroy()
    {
        healthController.HealthChanged -= OnHealthChanged;
    }
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
