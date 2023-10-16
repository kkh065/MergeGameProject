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
        _textInfo.text = ExplainData(UPData);
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

    string ExplainData(UpgradeData data)
    {
        switch (data.UpgradeType)
        {
            case UpgradeType.Gold:
                switch (data.ButtonIndex)
                {
                    case 0: return $"{data.Name}�� {data.Increase * data.Level}��ŭ ����"; //���ݷ� ����
                    case 1: return $"{data.Name}�� {data.Increase * data.Level}��ŭ ����"; //���ݼӵ� ����
                    case 2: return $"{data.Name}�� {data.Increase * data.Level}%��ŭ ����"; //ġ��Ÿ Ȯ��
                    case 3: return $"{data.Name}�� {data.Increase * data.Level}��ŭ ����"; //ġ��Ÿ ����
                    case 4: return $"{data.Name}�� {data.Increase * data.Level}��ŭ ����"; //���� ü�� ����
                }
                break;
            case UpgradeType.Management:
                switch (data.ButtonIndex)
                {
                    case 0: return $"��ġ ������ ��ó�� �� {data.Increase * data.Level}ĭ ����"; //ĳ���� �� ����
                    case 1: return $"���� ü�� {data.Increase * data.Level} ����"; //����ü�� ����
                }
                break;
            case UpgradeType.Attack:
                switch (data.ButtonIndex)
                {
                    case 0: return $"{data.Name} {data.Increase * data.Level} ����"; //���ݷ� ����
                    case 1: return $"{data.Name}�� {data.Increase * data.Level}��ŭ ����"; //���ݼӵ� ����
                    case 2: return $"{data.Name}�� {data.Increase * data.Level}%��ŭ ����"; //ġ��Ÿ Ȯ��
                    case 3: return $"{data.Name}�� {data.Increase * data.Level}��ŭ ����"; //ġ��Ÿ ����
                }
                break;
            case UpgradeType.Making:
                switch (data.ButtonIndex)
                {
                    case 0: return $"ȭ�� ���� �ð� {data.Increase * data.Level}�� ����";
                    case 1:
                    case 2:
                        return $"���۵� ȭ���� ���� {data.Increase * data.Level} ����";
                    case 3: return $"(�ڵ�){11f - (data.Increase * data.Level)}�ʿ� 1�� ȭ���� ����";
                    case 4: return $"(�ڵ�){11f - (data.Increase * data.Level)}�ʿ� 1�� ȭ���� ����";
                }
                break;
        }
        return "";
    }

    public void UpdateTabData(UpgradeData Data)
    {
        _nowLevel.text = $"LV.{Data.Level}";
        _textPrice.text = Data.Price.ToString();
        _textInfo.text = ExplainData(Data);
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
