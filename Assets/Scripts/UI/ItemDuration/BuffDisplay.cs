using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour
{
    public Image itemIcon;
    public Image durationFill;
    public TextMeshProUGUI timeText;

    private BuffInfo currentBuff;

    public void SetupBuff(BuffInfo buff)
    {
        currentBuff = buff;
        itemIcon.sprite = buff.icon;
        this.gameObject.SetActive(true);

        if (timeText != null)
        {
            UpdateTextAmount();
        }

        UpdateFillAmount();
    }

    public void ClearBuff()
    {
        currentBuff = null;
        this.gameObject.SetActive(false);
    }

    public void UpdateBuffUIElements()
    {
        if (currentBuff != null)
        {
            UpdateFillAmount();
            UpdateTextAmount();
        }
    }

    private void UpdateFillAmount()
    {
        if (currentBuff != null && currentBuff.totalDuration > 0)
        {
            durationFill.fillAmount = (currentBuff.totalDuration - currentBuff.remainingDuration) / currentBuff.totalDuration;

        }
        else
        {
            durationFill.fillAmount = 1;
        }
    }

    private void UpdateTextAmount()
    {
        if (timeText != null && currentBuff != null)
        {
            timeText.text = Mathf.CeilToInt(currentBuff.remainingDuration).ToString();
        }
    }
}
