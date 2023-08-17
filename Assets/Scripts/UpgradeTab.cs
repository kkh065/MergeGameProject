using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTab : MonoBehaviour
{
    [SerializeField] Text _nowLevel;
    [SerializeField] Text _upgradeName;
    [SerializeField] Text _textInfo;
    [SerializeField] Text _upgradeValue;
    [SerializeField] Text _textPrice;
    [SerializeField] Button _upgradeButton;
    [SerializeField] Image _imageIcon;
    public void Init(UpgradeData Data, Action<UpgradeType, int> ButtonAction)
    {
        _nowLevel.text = $"LV.{Data.Level}";
        _upgradeName.text = $"{Data.Name}(MAX {Data.MaxLevel})";
        _textInfo.text = Data.Explan;
        _upgradeValue.text = $"+ {Data.Increase}";
        _textPrice.text = (Data.Price * Data.Level).ToString();
        _upgradeButton.onClick.AddListener(() => ButtonAction(Data.UpgradeType, Data.ButtonIndex));
        _imageIcon.sprite = Resources.Load<Sprite>("UpgradeIcon/Icon" + (int)Data.UpgradeType + Data.ButtonIndex);
        //값의 타입에 따라서 리소스 모양 바꿔줘야함
    }
}
