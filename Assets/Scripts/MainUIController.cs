using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class MainUIController : MonoBehaviour
{

    #region 지역변수

    [SerializeField] GameObject _character;
    [SerializeField] GameObject _upgrade;
    [SerializeField] GameObject _mergeInventory;
    [SerializeField] GameObject _contents;
    [SerializeField] GameObject _shop;

    #endregion

    private void Awake()
    {
        MergeInventoryToggleIsON();
    }

    #region 토글제어

    public void CharacterToggleIsON()
    {
        AllTabClose();
        CharacterOpen();
    }

    public void UpgradeToggleIsON()
    {
        AllTabClose();
        UpgradeOpne();
    }

    public void MergeInventoryToggleIsON()
    {
        AllTabClose();
        MergeInventoryOpen();
    }

    public void ContentsToggleIsON()
    {
        AllTabClose();
        ContentsOpen();
    }

    public void ShopToggleIsON()
    {
        AllTabClose();
        ShopOpen();
    }
    
    void AllTabClose()
    {
        _character.SetActive(false);
        _upgrade.SetActive(false);
        _mergeInventory.SetActive(false);
        _contents.SetActive(false);
        _shop.SetActive(false);
    }



    #endregion

    #region 캐릭터관리창

    void CharacterOpen()
    {
        _character.SetActive(true);
    }

    #endregion

    #region 업그레이드창

    [SerializeField] GameObject _goldUpgrade;
    [SerializeField] GameObject _managementUpgrade;
    [SerializeField] GameObject _attckUpgrade;
    [SerializeField] GameObject _makingUpgrade;
    [SerializeField] GameObject _specialUpgrade;

    [SerializeField] GameObject _upgradeTab;
    //업그레이드 탭 오픈
    void UpgradeOpne()
    {
        _upgrade.SetActive(true);
        GoldUpgradeToggleOn();
    }

    //업그레이드 탭 토글제어

    public void GoldUpgradeToggleOn()
    {
        AllUpgradeTabClose();
        GoldUpgradeOpen();
    }

    public void ManagementUpgradeToggleOn()
    {
        AllUpgradeTabClose();
        ManagementUpgradeOpen();
    }

    public void AttackUpgradeToggleOn()
    {
        AllUpgradeTabClose();
        AttckUpgradeOpen();
    }

    public void MakingUpgradeToggleOn()
    {
        AllUpgradeTabClose();
        MakingUpgradeOpen();
    }

    public void SpecialUpgradeToggleOn()
    {
        AllUpgradeTabClose();
        SpecialUpgradeOpen();
    }

    void AllUpgradeTabClose()
    {
        _goldUpgrade.SetActive(false);
        _managementUpgrade.SetActive(false);
        _attckUpgrade.SetActive(false);
        _makingUpgrade.SetActive(false);
        _specialUpgrade.SetActive(false);
    }

    void InitializedUpgradeTab()
    {
        UpgradeTabData tab = new UpgradeTabData();
        tab.NowLevel = Data.Instance.UpgradeData.GoldAttackDamageLevel;
        tab.MaxLevel = 99999;
        tab.Name = "기본공격력";
        tab.Increase = 5;
        tab.Price = 5 * tab.NowLevel;
        tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}만큼 증가";
        tab.Type = UpgradeType.Gold;
        tab.ButtonIndex = 0;

        InstantiateTab(tab, _goldUpgrade.transform);

        //각 창에다가 업그레이드 탭 생성 -> 리스트로 만들어서 제이슨으로 저장후 불러와서 생성하자
        //이넘으로 업그레이드 타입 만들고 같이 저장했다가, 타입별로 부모 스위치케이스 ㄱㄱ
        //인덱스를 인자로 받아와서 스위치케이스로 나누기. 함수를 버튼에 델리게이트로 붙이면 인자값에 인덱스를 넣을 수 있을텐데
    }

    void InstantiateTab(UpgradeTabData tabData, Transform tr)
    {
        GameObject tabGo = Instantiate(_upgradeTab, tr);
        tabGo.transform.Find("LevelText").GetComponent<Text>().text = $"LV.{tabData.NowLevel}";
        tabGo.transform.Find("UpgradeName").GetComponent<Text>().text = $"{tabData.Name}(MAX {tabData.MaxLevel})";
        tabGo.transform.Find("UpgradeInfo").GetComponent<Text>().text = tabData.Explan;
        tabGo.transform.Find("TextUpgradeValue").GetComponent<Text>().text = $"+ {tabData.Increase}";
        tabGo.transform.Find("TextPrice").GetComponent<Text>().text = (tabData.Price * tabData.NowLevel).ToString();
        tabGo.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => TabUpgradeButton(tabData.Type, tabData.ButtonIndex));
    }

    void TabUpgradeButton(UpgradeType type, int idx)
    {
        //골드탭 업그레이드 버튼 눌렸을때 기능구현
        switch (type)
        {
            case UpgradeType.Gold:                
                break;
            case UpgradeType.Management:
                break;
            case UpgradeType.Attack:
                break;
            case UpgradeType.Making:
                break;
        }
    }

    void UpgradeTabUpdate()
    {
        //아니면 해당 업그레이드가 눌렸을경우 그 탭을 어딘지 찾아서 그 탭만 업데이트 
    }

    void GoldUpgradeOpen()
    {
        // 골드 업그레이드 내역 업데이트
        _goldUpgrade.SetActive(true);        
    }

    void ManagementUpgradeOpen()
    {
        // 관리 업그레이드 내역 업데이트
        _managementUpgrade.SetActive(true);
    }

    void AttckUpgradeOpen()
    {
        // 공격 업그레이드 내역 업데이트
        _attckUpgrade.SetActive(true);
    }

    void MakingUpgradeOpen()
    {
        // 제작 업그레이드 내역 업데이트
        _makingUpgrade.SetActive(true);
    }

    void SpecialUpgradeOpen()
    {
        // 특수 업그레이드 내역 업데이트
        _specialUpgrade.SetActive(true);
    }


    #endregion

    #region 합성창

    void MergeInventoryOpen()
    {
        _mergeInventory.SetActive(true);
    }

    #endregion

    #region 컨텐츠창

    void ContentsOpen()
    {
        _contents.SetActive(true);
    }

    #endregion

    #region 상점창

    void ShopOpen()
    {
        _shop.SetActive(true);
    }

    #endregion
}


public class UpgradeTabData
{
    int _nowLevel = 0;
    public int NowLevel { get { return _nowLevel; } set { _nowLevel = value; } }

    int _maxLevel = 0;
    public int MaxLevel { get { return _maxLevel; } set { _maxLevel = value; } }

    int _increase = 0;
    public int Increase { get { return _increase; } set { _increase = value; } }

    string _name = "";
    public string Name { get { return _name; } set { _name = value; } }

    string _explan = "";
    public string Explan { get { return _explan; } set { _explan = value; } }

    int _price = 0;
    public int Price { get { return _price; } set { _price = value; } }

    UpgradeType _type;
    public UpgradeType Type { get { return _type; } set { _type = value; } }

    int _buttonIndex = 0;
    public int ButtonIndex { get { return _buttonIndex; } set { _buttonIndex = value; } }
}

public enum UpgradeType
{
    Gold,
    Management,
    Attack,
    Making,
}
