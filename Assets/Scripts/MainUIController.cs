using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
        InitializedGame();
    }

    public void InitializedGame()
    {
        //업그레이드 슬롯 관련 초기화
        InitializedUpgradeTab();

        //인벤토리 관련 초기화
        InventorySlotAllClose();
        //InitEquipSlot();
        //인벤토리 데이터를 세이브파일에서 읽어오는 작업 해야함
        UpdateInventory(); // 인벤토리 데이터에 따라서 인벤토리창을 업데이트
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //UpgradeTabDataCreate();
        }


        //자동제작
        _autoMakingTimer += Time.deltaTime;
        if(_autoMakingTimer > 10)
        {
            MakingArrow();
            _autoMakingTimer = 0;
        }

        //자동합성
        _autoMergeTimer += Time.deltaTime;
        if (_autoMergeTimer > 10)
        {
            MergeArrow();
            _autoMergeTimer = 0;
        }

        _autoSaveTimer += Time.deltaTime;
        //자동저장
        if (_autoSaveTimer > 600)
        {
            Data.Instance.SaveInventoryData(_equipArrowData, _inventoryData);
            _autoSaveTimer = 0;
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

    [SerializeField] Transform _inventorySlot;
    [SerializeField] Transform _equipSlot;
    int[] _equipArrowData;
    List<int> _inventoryData = new List<int>();
    float _autoMergeTimer = 0;
    float _autoMakingTimer = 0;
    float _autoSaveTimer = 0;

    void MergeInventoryOpen()
    {
        _mergeInventory.SetActive(true);
    }

    void InitEquipSlot()
    {
        _equipArrowData = new int[Data.Instance.UpgradeData.ManagementArcherLevel + 1];
        for (int i = 0; i < _equipSlot.childCount; i++)
        {
            _equipSlot.GetChild(i).gameObject.SetActive(i < Data.Instance.UpgradeData.ManagementArcherLevel + 1);
        }
    }

    //장비창
    void UpdateEquip()
    {
        //장비창 갱신 - 세이브데이터에다가 인벤토리 데이터랑 장비창 데이터 저장하는것 만들기
    }


    //인벤토리
    void InventorySlotAllClose()
    {
        for (int i = 0; i < _inventorySlot.childCount; i++)
        {
            _inventorySlot.GetChild(i).gameObject.SetActive(false);
        }
    }

    void UpdateInventory()
    {
        //인벤토리 데이터를 누군가가 리스트로 들고있긴 해야겠네.
        for(int i = 0; i < _inventoryData.Count; i++)
        {
            _inventorySlot.GetChild(i).GetComponent<InventorySlot>().Init(_inventoryData[i]);
        }
    }

    public void MakingArrow()
    {
        //제작 쿨타임 만들어야함. 쿨타임만들기.
        if (_inventoryData.Count < 40)
        {
            _inventoryData.Add(GameManager.Instance.GetMakingArrowLevel());
            _inventorySlot.GetChild(_inventoryData.Count - 1).GetComponent<InventorySlot>().Init(_inventoryData[_inventoryData.Count - 1]);
        }
    }

    public void MergeArrow()
    {
        //화살 조합 어떻게 구현할 지 고민해보기.
    }

   

    // 화살클래스를 만들기 : 화살의정보는 화살의 레벨 뿐인가? 이미지는 화살레벨 10단위로 변하게끔 구현하고...
    // 인벤토리 하나당 각자 스크립트를 들고 있고 거기에 자기 정보를 담아둬야함...(인벤토리 인덱스 정보 필요)
    // 미리 생성해두고 켯다 껏다 하려면, 게임오브젝트 리스트를 들고있어야 하는데..겟차일드로 해결

    // 장착인벤토리 리스트 - 아처수 업그레이드에 따라 최대치 증가해야함 최대 8개
    // 비어있는 상태 없음. 기본적으로 데이터가 없으면 제작화살레벨과 동일한 화살 착용하게끔 적용
    // 제작화살레벨이 오르면 장착인벤토리 리스트를 돌면서 제작레벨보다 낮은 친구들을 전부 제작레벨로 만들어줌
    // 장착(교환) 방식 구현해야함. 드래그 앤 드롭으로 구현, 조합은 나중에 구현(자동조합만 먼저 구현)

    // 자동제작 : 리스트에 추가 및 제일뒤에 아이템 생성 - 현재 제작화살레벨(업그레이드 반영) 데이터 필요
    // 리스트는 
    // 자동조합 : 리스트에서 한바퀴돌면서 동일레벨 화살 찾으면 합성 후 인벤토리 업데이트 - 최대 화살레벨 갱신

    // 씬에다가 인벤토리 슬롯 만들어야함. 프리펩일 필요는 없을듯


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
