using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Text _textLevel;
    [SerializeField] Image _icon;
    int _ArrowLevel = 0;


    

    public void Init(int level)
    {
        _ArrowLevel = level;
        _textLevel.text = _ArrowLevel.ToString();
        _icon.sprite = Resources.Load<Sprite>("Arrow/Arrow" + _ArrowLevel / 10);
        gameObject.SetActive(true);
    }
}
