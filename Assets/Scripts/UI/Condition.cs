using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float currentValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;


    void Start()
    {
        currentValue = startValue;
    }

    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return currentValue / maxValue;
    }

    public void Add(float value)
    {
        currentValue = Mathf.Min(currentValue + value, maxValue);
    }

    public void Sub(float value)
    {
        currentValue = Mathf.Max(currentValue - value, 0);
    }
}
