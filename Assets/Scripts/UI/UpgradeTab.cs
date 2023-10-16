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
                    case 0: return $"{data.Name}이 {data.Increase * data.Level}만큼 증가"; //공격력 증가
                    case 1: return $"{data.Name}가 {data.Increase * data.Level}만큼 증가"; //공격속도 증가
                    case 2: return $"{data.Name}이 {data.Increase * data.Level}%만큼 증가"; //치명타 확률
                    case 3: return $"{data.Name}이 {data.Increase * data.Level}만큼 증가"; //치명타 배율
                    case 4: return $"{data.Name}이 {data.Increase * data.Level}만큼 증가"; //담장 체력 증가
                }
                break;
            case UpgradeType.Management:
                switch (data.ButtonIndex)
                {
                    case 0: return $"배치 가능한 아처의 수 {data.Increase * data.Level}칸 증가"; //캐릭터 수 증가
                    case 1: return $"담장 체력 {data.Increase * data.Level} 증가"; //담장체력 증가
                }
                break;
            case UpgradeType.Attack:
                switch (data.ButtonIndex)
                {
                    case 0: return $"{data.Name} {data.Increase * data.Level} 증가"; //공격력 증가
                    case 1: return $"{data.Name}가 {data.Increase * data.Level}만큼 증가"; //공격속도 증가
                    case 2: return $"{data.Name}이 {data.Increase * data.Level}%만큼 증가"; //치명타 확률
                    case 3: return $"{data.Name}이 {data.Increase * data.Level}만큼 증가"; //치명타 배율
                }
                break;
            case UpgradeType.Making:
                switch (data.ButtonIndex)
                {
                    case 0: return $"화살 제작 시간 {data.Increase * data.Level}초 감소";
                    case 1:
                    case 2:
                        return $"제작된 화살의 레벨 {data.Increase * data.Level} 증가";
                    case 3: return $"(자동){11f - (data.Increase * data.Level)}초에 1번 화살을 제작";
                    case 4: return $"(자동){11f - (data.Increase * data.Level)}초에 1번 화살을 결합";
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
