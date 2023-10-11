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
        _textPrice.text = (Data.Price * Data.Level).ToString();
        _upgradeButton.onClick.AddListener(() => ButtonAction(Data.UpgradeType, Data.ButtonIndex));
        _imageIcon.sprite = Resources.Load<Sprite>("UpgradeIcon/Icon" + (int)Data.UpgradeType + Data.ButtonIndex);
        //���� Ÿ�Կ� ���� ���ҽ� ��� �ٲ������
        if (Data.Level < Data.MaxLevel)
        {
            _upgradeValue.text = $"+ {Data.Increase}";
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
        _textPrice.text = (Data.Price * Data.Level).ToString();
        //������ ���� ��ư ���� ����

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
