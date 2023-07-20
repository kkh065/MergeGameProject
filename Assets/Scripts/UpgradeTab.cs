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
    public void Init(UpgradeTabData tabData, Action<UpgradeType, int> ButtonAction)
    {
        _nowLevel.text = $"LV.{tabData.NowLevel}";
        _upgradeName.text = $"{tabData.Name}(MAX {tabData.MaxLevel})";
        _textInfo.text = tabData.Explan;
        _upgradeValue.text = $"+ {tabData.Increase}";
        _textPrice.text = (tabData.Price * tabData.NowLevel).ToString();
        _upgradeButton.onClick.AddListener(() => ButtonAction(tabData.Type, tabData.ButtonIndex));
    }
}
