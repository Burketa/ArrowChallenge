using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timer;
    public Image timerShadow;

    public float timerInitialValue = 3;
    public float defaultAddTime = 1;
    public float subtractVar = 0.1f;

    private float currentMaxValue, currentValue;

    private void Start()
    {
        currentMaxValue = timerInitialValue;
        currentValue = timerInitialValue;
        ResetTimer();
    }

    public void AddTime()
    {
        timer.value += defaultAddTime;
        UpdateTimerWidth();
        timer.value = Mathf.Clamp(timer.value, 0.0f, timer.maxValue);
    }

    public void AddTime(float value)
    {
        timer.value += value;
        UpdateTimerWidth();
        timer.value = Mathf.Clamp(timer.value, 0.0f, timer.maxValue);
    }

    public void ResetTimer()
    {
        timer.maxValue = timerInitialValue;
        timer.value = timerInitialValue;
    }

    public IEnumerator StartTimer()
    {
        while (true)
        {
            timer.value -= Time.deltaTime;
            if (timer.value <= 0)
                FindObjectOfType<MainMechanics>().GameOver();
            yield return null;
        }
    }

    public void UpdateTimerWidth()
    {
        var rectTransform = timer.GetComponent<RectTransform>();    //sizeDelta.x = Width; sizeDelta.y = Height;
        var timerShadow = timer.transform.GetChild(0).GetComponent<RectTransform>();

        if (timer.maxValue > 3)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - 75, rectTransform.sizeDelta.y);
            timerShadow.sizeDelta = new Vector2(timerShadow.sizeDelta.x - 75, timerShadow.sizeDelta.y);

            float newMax = timer.maxValue - subtractVar;
            float mult = timer.value / timer.maxValue;
            float newValue = newMax * mult;

            if (newMax < 3)
                newMax = 3;

            timer.maxValue = newMax;
            timer.value = Mathf.Clamp(newValue, 0, timer.maxValue);
        }


    }
}
