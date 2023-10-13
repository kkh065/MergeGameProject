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
    [SerializeField] Image _currencyIcon;
    public void Init(UpgradeData UPData, Action<UpgradeType, int> ButtonAction)
    {
        _nowLevel.text = $"LV.{UPData.Level}";
        _upgradeName.text = $"{UPData.Name}(MAX {UPData.MaxLevel})";
        _textInfo.text = UPData.Explan;
        _textPrice.text = UPData.Price.ToString();
        _upgradeButton.onClick.AddListener(() => ButtonAction(UPData.UpgradeType, UPData.ButtonIndex));
        _imageIcon.sprite = Resources.Load<Sprite>("UpgradeIcon/Icon" + (int)UPData.UpgradeType + UPData.ButtonIndex);

        switch(UPData.priceType)
        {
            case PriceType.Gold:
                _currencyIcon.sprite = Data.Instance.CurrencyImage["Gold"];
                break;
            case PriceType.Dia:
                _currencyIcon.sprite = Data.Instance.CurrencyImage["Dia"];
                break;
            case PriceType.Reincarnation:
                _currencyIcon.sprite = Data.Instance.CurrencyImage["Reincarnation"];                
                break;
        }

        if (UPData.Level < UPData.MaxLevel)
        {
            _upgradeValue.text = $"+ {UPData.Increase}";
        }
        else
        {
            _upgradeValue.text = "Max";
        }
    }

    public void UpdateTabData(UpgradeData Data)
    {
        _nowLevel.text = $"LV.{Data.Level}";
        _textInfo.text = Data.Explan;
        _textPrice.text = Data.Price.ToString();
        //레벨에 따라서 버튼 세팅 변경

        if (Data.Level < Data.MaxLevel)
        {
            _upgradeValue.text = $"+ {Data.Increase}";
        }
        else
        {
            _upgradeValue.text = "Max";
        }
    }
}
