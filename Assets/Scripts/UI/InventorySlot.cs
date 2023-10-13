using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Text _textLevel;
    [SerializeField] Image _icon;
    int _ArrowLevel = 0;

    Vector3 _basePos;
    public int Index { private set; get; }

    public void Init(int level, int idx)
    {
        _ArrowLevel = level;
        Index = idx;
        _textLevel.text = _ArrowLevel.ToString();
        _icon.sprite = Resources.Load<Sprite>("Arrow/Arrow" + _ArrowLevel / 10);
        gameObject.SetActive(true);
    }

    public void OnDragBegin(BaseEventData data)
    {
        _basePos = this.transform.position;
        data.selectedObject = gameObject;
        gameObject.GetComponentsInChildren<Image>()[0].raycastTarget = false;
    }

    public void OnDrag(BaseEventData data)
    {
        PointerEventData pointer_data = (PointerEventData)data;
        this.transform.position = pointer_data.position;
    }

    public void OnDragEnd(BaseEventData data)
    {
        transform.position = _basePos;
        gameObject.GetComponentsInChildren<Image>()[0].raycastTarget = true;
    }
}
