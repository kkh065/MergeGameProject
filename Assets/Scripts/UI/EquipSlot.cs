using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    [SerializeField] Text _textLevel;
    [SerializeField] Image _icon;
    int _ArrowLevel = 0;

    public int Index { private set; get; }

    public void Init(int level, int idx)
    {
        _ArrowLevel = level;
        Index = idx;
        _textLevel.text = _ArrowLevel.ToString();
        _icon.sprite = Resources.Load<Sprite>("Arrow/Arrow" + _ArrowLevel / 10);
        gameObject.SetActive(true);
    }

    public void OnDrop(BaseEventData data)
    {
        InventorySlot inven = data.selectedObject.GetComponent<InventorySlot>();

        int Templevel = GameManager.Instance.InventoryData[inven.Index];
        GameManager.Instance.InventoryData[inven.Index] = GameManager.Instance.EquipArrowData[Index];
        GameManager.Instance.EquipArrowData[Index] = Templevel;

        inven.Init(GameManager.Instance.InventoryData[inven.Index], inven.Index);
        Init(GameManager.Instance.EquipArrowData[Index], Index);
        GameManager.Instance.UpdateCaracter();
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
    }
}
