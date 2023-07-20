using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    UpgradeTabList datas;
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

    public void Start()
    {
        datas = new UpgradeTabList();
        datas.UpgradeList = new List<UpgradeTabData>();
        InitializedUpgradeTab();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //UpgradeTabDataCreate();
        }

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

    [SerializeField] GameObject _goldUpgradeContent;
    [SerializeField] GameObject _managementUpgradeContent;
    [SerializeField] GameObject _attckUpgradeContent;
    [SerializeField] GameObject _makingUpgradeContent;
    [SerializeField] GameObject _specialUpgradeContent;

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
        //업그레이드 탭 세팅

        if (File.Exists(Application.persistentDataPath + "/UpgradeTabList.json"))
        {
            string json = "";
            using (StreamReader inStream = new StreamReader(Application.persistentDataPath + "/UpgradeTabList.json"))
            {
                json = inStream.ReadToEnd();
            }

            if (string.IsNullOrEmpty(json) == false)
            {
                datas = JsonUtility.FromJson<UpgradeTabList>(json);
                UpgradeTabInitToType();
            }
            else Debug.Log("내용이 없습니다.");
        }
        else Debug.Log("파일이 없습니다.");

        //각 창에다가 업그레이드 탭 생성 -> 리스트로 만들어서 제이슨으로 저장후 불러와서 생성하자
        //이넘으로 업그레이드 타입 만들고 같이 저장했다가, 타입별로 부모 스위치케이스 ㄱㄱ
        //인덱스를 인자로 받아와서 스위치케이스로 나누기. 함수를 버튼에 델리게이트로 붙이면 인자값에 인덱스를 넣을 수 있을텐데
    }

    void UpgradeTabInitToType()
    {
        GameObject tabGo = new GameObject();

        foreach (var data in datas.UpgradeList)
        {
            switch (data.Type)
            {
                case UpgradeType.Gold:
                    tabGo = Instantiate(_upgradeTab, _goldUpgradeContent.transform);
                    break;
                case UpgradeType.Management:
                    tabGo = Instantiate(_upgradeTab, _managementUpgradeContent.transform);
                    break;
                case UpgradeType.Attack:
                    tabGo = Instantiate(_upgradeTab, _attckUpgradeContent.transform);
                    break;
                case UpgradeType.Making:
                    tabGo = Instantiate(_upgradeTab, _makingUpgradeContent.transform);
                    break;
            }
            tabGo?.GetComponent<UpgradeTab>().Init(data, TabUpgradeButton);
            //게임오브젝트 네임을 바꿔놓을까? 나중에 업데이트할때 찾기 쉽게?
        }
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

    #region 업그레이드창 데이터파일 생성코드
    void UpgradeTabDataCreate()
    {
        if (Input.GetMouseButtonDown(0))
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
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldAttackSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "공격속도";
            tab.Increase = 0.01f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldCriticalLevel;
            tab.MaxLevel = 20;
            tab.Name = "치명타확률";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}%만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldCriticalDamageLevel;
            tab.MaxLevel = 500;
            tab.Name = "치명타 데미지";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name}이 {tab.Increase * tab.NowLevel}%만큼 증가";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.GoldWallHpLevel;
            tab.MaxLevel = 30;
            tab.Name = "담장 강화";
            tab.Increase = 10;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"담장 체력을 {tab.Increase * tab.NowLevel}만큼 증가시킨다";
            tab.Type = UpgradeType.Gold;
            tab.ButtonIndex = 4;
            datas.UpgradeList.Add(tab);


            //관리업그레이드
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.ManagementArcherLevel;
            tab.MaxLevel = 7;
            tab.Name = "아처 모집";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"배치 가능한 아처의 수 {tab.Increase * tab.NowLevel}칸 증가";
            tab.Type = UpgradeType.Management;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.ManagementWallHpLevel;
            tab.MaxLevel = 30;
            tab.Name = "담장 강화";
            tab.Increase = 10;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"담장 체력 {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Management;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            //공격 업그레이드
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaAttackDamegeLevel;
            tab.MaxLevel = 500;
            tab.Name = "기본 공격력";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaAttackSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "공격속도";
            tab.Increase = 0.05f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaCriticalLevel;
            tab.MaxLevel = 20;
            tab.Name = "치명타확률";
            tab.Increase = 0.5f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel}% 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaCriticalDamageLevel;
            tab.MaxLevel = 500;
            tab.Name = "치명타 데미지";
            tab.Increase = 5;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"{tab.Name} {tab.Increase * tab.NowLevel}% 증가";
            tab.Type = UpgradeType.Attack;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            //제작업그레이드
            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingSpeedLevel;
            tab.MaxLevel = 20;
            tab.Name = "빠른 제작";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"화살 제작 시간 {tab.Increase * tab.NowLevel}초 감소";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 0;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingArrowLevelLevel;
            tab.MaxLevel = 150;
            tab.Name = "제작 화살 레벨 증가";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"제작된 화살의 레벨 {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 1;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.DiaMakingArrowLevelLevel;
            tab.MaxLevel = 80;
            tab.Name = "제작 화살 레벨 증가";
            tab.Increase = 1;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"제작된 화살의 레벨 {tab.Increase * tab.NowLevel} 증가";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 2;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MakingAutoSpeedLevel;
            tab.MaxLevel = 50;
            tab.Name = "자동 제작";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"(자동)10초에 {tab.Increase * tab.NowLevel}번 화살을 제작";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 3;
            datas.UpgradeList.Add(tab);

            tab = new UpgradeTabData();
            tab.NowLevel = Data.Instance.UpgradeData.MergeAutoSpeedLevel;
            tab.MaxLevel = 50;
            tab.Name = "자동 합성";
            tab.Increase = 0.2f;
            tab.Price = 5 * tab.NowLevel;
            tab.Explan = $"(자동)10초에 {tab.Increase * tab.NowLevel}번 화살을 결합";
            tab.Type = UpgradeType.Making;
            tab.ButtonIndex = 4;
            datas.UpgradeList.Add(tab);

            string Json = JsonUtility.ToJson(datas);

            string path = Application.persistentDataPath + "/UpgradeTabList.json";

            using (StreamWriter outStream = File.CreateText(path))
            {
                outStream.Write(Json);
            }

            Debug.Log(Json);
        }
    }

    #endregion


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
[Serializable]
public class UpgradeTabList
{
    public List<UpgradeTabData> UpgradeList;
}
[Serializable]
public class UpgradeTabData
{
    public int NowLevel = 0;
    public int MaxLevel = 0;
    public float Increase = 0;
    public string Name = "";
    public string Explan = "";
    public int Price = 0;
    public UpgradeType Type;
    public int ButtonIndex = 0;
}
[Serializable]
public enum UpgradeType
{
    Gold,
    Management,
    Attack,
    Making,
}
