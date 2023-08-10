using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
        _equipArrowData = Data.Instance.ArrowLevelData.EquipData;
        _inventoryData = Data.Instance.ArrowLevelData.InventoryData;
        MergeInventoryToggleIsON();
    }

    public void Start()
    {
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
            Data.Instance.SaveInventoryData();
            _autoSaveTimer = 0;
        }

        //수동제작
        if(_makingCooltime > 0)
        {
            _makingCooltime -= Time.deltaTime;
            _imageMakingCooltime.fillAmount = _makingCooltime / 10f;
        }

        //수동합성
        if (_mergeCooltime > 0)
        {
            _mergeCooltime -= Time.deltaTime;
            _imageMergeCooltime.fillAmount = _mergeCooltime;
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
        GameObject tabGo = new GameObject();

        foreach (var data in Data.Instance.Datas.UpgradeList)
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
        //해당 업그레이드가 눌렸을경우 그 탭을 어딘지 찾아서 그 탭만 업데이트
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

    [SerializeField] Transform _inventorySlot;
    [SerializeField] Transform _equipSlot;
    [SerializeField] Image _imageMakingCooltime;
    [SerializeField] Image _imageMergeCooltime;
    int[] _equipArrowData;
    List<int> _inventoryData;
    float _autoMergeTimer = 0;
    float _autoMakingTimer = 0;
    float _autoSaveTimer = 0;
    float _makingCooltime = 0;
    float _mergeCooltime = 0;

    void MergeInventoryOpen()
    {
        _mergeInventory.SetActive(true);
        InitEquipSlot();
    }

    void InitEquipSlot()
    {
        for (int i = 0; i < _equipSlot.childCount; i++)
        {
            _equipSlot.GetChild(i).gameObject.SetActive(false);
        }

        UpdateEquip();
    }

    //장비창
    void UpdateEquip()
    {
        //장비창 갱신 - 세이브데이터에다가 인벤토리 데이터랑 장비창 데이터 저장하는것 만들기
        for (int i = 0; i < 1 + Data.Instance.UpgradeData.ManagementArcherLevel; i++)
        {
            _equipSlot.GetChild(i).gameObject.SetActive(true);
            _equipSlot.GetChild(i).GetComponent<InventorySlot>().Init(_equipArrowData[i]);
        }
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
        for(int i = 0; i < _inventoryData.Count; i++)
        {
            _inventorySlot.GetChild(i).GetComponent<InventorySlot>().Init(_inventoryData[i]);
        }
    }

    void MakingArrow()
    {
        if (_inventoryData.Count < 40)
        {
            _inventoryData.Add(GameManager.Instance.GetMakingArrowLevel());
            //_inventorySlot.GetChild(_inventoryData.Count - 1).GetComponent<InventorySlot>().Init(_inventoryData[_inventoryData.Count - 1]);
            InventorySlotAllClose();
            UpdateInventory();
        }
    }

    public void OnclickMakingArrow()
    {
        if(_makingCooltime <= 0)
        {
            MakingArrow();
            _makingCooltime = 10;
        }
    }

    public void OnClickMergeArrow()
    {
        if (_mergeCooltime <= 0)
        {
            MergeArrow();
            _mergeCooltime = 1;
        }
    }

    void MergeArrow()
    {
        for (int i = 0; i < _inventoryData.Count; i++)
        {
            for (int j = 0; j < _inventoryData.Count; j++)
            {
                if (i != j)
                {
                    if (_inventoryData[i] == _inventoryData[j])
                    {
                        _inventoryData[i]++;
                        _inventoryData.RemoveAt(j);
                        InventorySlotAllClose();
                        UpdateInventory();
                        return;
                    }
                }
            }
        }
    }

    public void OnClickSort()
    {
        var datas = from data in _inventoryData
                    orderby data descending
                    select data;
        _inventoryData = datas.ToList();
        InventorySlotAllClose();
        UpdateInventory();
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
