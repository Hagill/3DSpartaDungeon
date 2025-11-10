using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData ItemData;

    public string GetInteractPrompt()
    {
        string str = $"{ItemData.itemName}\n{ItemData.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = ItemData;
        // 델리게이트로 구독한 메서드
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
