using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemDuration : MonoBehaviour
{
    public GameObject UIDuration;

    public BuffDisplay[] buffSlots;

    private List<BuffInfo> activeBuffs = new List<BuffInfo>();

    private void Start()
    {
        if(UIDuration != null)
        {
            UIDuration.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            activeBuffs[i].remainingDuration -= Time.deltaTime;
            if(activeBuffs[i].remainingDuration <= 0)
            {
                activeBuffs.RemoveAt(i);
                UpdateBuffUI();
            }
        }

        for (int i = 0; i < activeBuffs.Count; i++)
        {
            buffSlots[i].UpdateBuffUIElements();
        }

        if (UIDuration != null)
        {
            bool beActive = activeBuffs.Count > 0;
            if(UIDuration.activeSelf != beActive)
            {
                UIDuration.SetActive(beActive);
            }
        }
    }

    public void AddBuff(ConsumableType type, Sprite icon, float duration)
    {
        BuffInfo existingBuff = activeBuffs.Find(b => b.type == type);
        if (existingBuff != null)
        {
            existingBuff.remainingDuration = duration;
            existingBuff.totalDuration = duration;
        }
        else
        {
            BuffInfo newBuff = new BuffInfo { type = type, icon = icon, remainingDuration = duration, totalDuration = duration };
            activeBuffs.Add(newBuff);
        }

        UpdateBuffUI();

        if (UIDuration != null && !UIDuration.activeSelf)
        {
            UIDuration.SetActive(true);
        }
    }

    private void UpdateBuffUI()
    {
        foreach (BuffDisplay slot in buffSlots)
        {
            if (slot != null) slot.ClearBuff();
        }

        for (int i = 0; i < activeBuffs.Count && i < buffSlots.Length; i++)
        {
            if (buffSlots[i] != null)
            {
                buffSlots[i].SetupBuff(activeBuffs[i]);
            }
        }
    }
}
