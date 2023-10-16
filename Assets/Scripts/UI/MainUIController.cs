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
        AllTabClose();
        MergeInventoryOpen();
    }

    public void Start()
    {
        InitializedGame();
    }

    public void InitializedGame()
    {
        //제작 쿨타임 관련 초기화
        SetCooltime();

        //업그레이드 슬롯 관련 초기화
        InitializedUpgradeTab();

        //인벤토리 관련 초기화
        InventorySlotAllClose();
        InitEquipSlot();
        UpdateInventory(); // 인벤토리 데이터에 따라서 인벤토리창을 업데이트
    }

    private void Update()
    {
        //자동제작
        _autoMakingTimer += Time.deltaTime;
        if(_autoMakingTimer > _autoMakingCooltime)
        {
            MakingArrow();
            _autoMakingTimer = 0;
        }

        //자동합성
        _autoMergeTimer += Time.deltaTime;
        if (_autoMergeTimer > _AutoMergeCooltime)
        {
            MergeArrow();
            _autoMergeTimer = 0;
        }

        _autoSaveTimer += Time.deltaTime;
        //자동저장
        if (_autoSaveTimer > 600)
        {
            Data.Instance.SaveInventoryData();
            GameManager.Instance.SaveGameData();
            _autoSaveTimer = 0;
        }

        //수동제작 쿨타임
        if(_makingCooltime > 0)
        {
            _makingCooltime -= Time.deltaTime;
            _imageMakingCooltime.fillAmount = _makingCooltime / _maxMakingCooltime;
        }

        //수동합성 쿨타임
        if (_mergeCooltime > 0)
        {
            _mergeCooltime -= Time.deltaTime;
            _imageMergeCooltime.fillAmount = _mergeCooltime;
        }
    }

    #region 토글제어

    public void CharacterToggleIsON(bool IsOn)
    {
        if(IsOn)
        {
            AllTabClose();
            CharacterOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }       
    }

    public void UpgradeToggleIsON(bool IsOn)
    {
        if (IsOn)
        {
            AllTabClose();
            UpgradeOpne();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void MergeInventoryToggleIsON(bool IsOn)
    {
        if (IsOn)
        {
            AllTabClose();
            MergeInventoryOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void ContentsToggleIsON(bool IsOn)
    {
        if (IsOn)
        {
            AllTabClose();
            ContentsOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void ShopToggleIsON(bool IsOn)
    {
        if (IsOn)
        {
            AllTabClose();
            ShopOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
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
        AllUpgradeTabClose();
        GoldUpgradeOpen();
    }

    //업그레이드 탭 토글제어

    public void GoldUpgradeToggleOn(bool IsOn)
    {
        if (IsOn)
        {
            AllUpgradeTabClose();
            GoldUpgradeOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void ManagementUpgradeToggleOn(bool IsOn)
    {
        if (IsOn)
        {
            AllUpgradeTabClose();
            ManagementUpgradeOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void AttackUpgradeToggleOn(bool IsOn)
    {
        if (IsOn)
        {
            AllUpgradeTabClose();
            AttckUpgradeOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void MakingUpgradeToggleOn(bool IsOn)
    {
        if (IsOn)
        {
            AllUpgradeTabClose();
            MakingUpgradeOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    public void SpecialUpgradeToggleOn(bool IsOn)
    {
        if (IsOn)
        {
            AllUpgradeTabClose();
            SpecialUpgradeOpen();
            UIManager.Instance.PLayUISound(SoundIndex.UIToggle);
        }
    }

    void AllUpgradeTabClose()
    {
        _goldUpgrade.SetActive(false);
        _managementUpgrade.SetActive(false);
        _attckUpgrade.SetActive(false);
        _makingUpgrade.SetActive(false);
        _specialUpgrade.SetActive(false);
    }

    Dictionary<UpgradeData, GameObject> UpgradeTabList = new Dictionary<UpgradeData, GameObject>();
    void InitializedUpgradeTab()
    {
        //업그레이드 탭 세팅
        GameObject tabGo = new GameObject();

        foreach (var data in Data.Instance.UpgradeDatas.upgradeDataList)
        {
            switch (data.UpgradeType)
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
            UpgradeTabList.Add(data, tabGo);
        }
    }

    void TabUpgradeButton(UpgradeType type, int idx)
    {
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
        UpgradeData ud = Data.Instance.GetUpgradeData(type, idx);
        if(ud.Level < ud.MaxLevel)
        {
            switch(ud.priceType)
            {
                case PriceType.Gold:
                    if (GameManager.Instance.Gold >= ud.Price)
                    {
                        GameManager.Instance.Gold -= ud.Price;
                        RefreshData(ud);
                    }
                    break;
                case PriceType.Dia:
                    if (GameManager.Instance.Dia >= ud.Price)
                    {
                        GameManager.Instance.Dia -= ud.Price;
                        RefreshData(ud);
                    }
                    break;
                case PriceType.Reincarnation:
                    if (GameManager.Instance.Reincarnation >= ud.Price)
                    {
                        GameManager.Instance.Reincarnation -= ud.Price;
                        RefreshData(ud);
                    }
                    break;
            }
        }
    }

    void RefreshData(UpgradeData data)
    {
        data.Level++;
        GameManager.Instance.SaveGameData();
        Data.Instance.SaveUpgradeData();
        UpdateGameData(data);
        UpgradeTabList[data].GetComponent<UpgradeTab>().UpdateTabData(data);
    }

    void UpdateGameData(UpgradeData data)
    {
        switch(data.UpgradeType)
        {
            case UpgradeType.Gold:
                switch (data.ButtonIndex)
                {
                    case 0: GameManager.Instance.UpdateCaracter(); break; //공격력 증가
                    case 1: GameManager.Instance.UpdateCaracter(); break; //공격속도 증가
                    case 2: GameManager.Instance.UpdateCaracter(); break; //치명타 확률
                    case 3: GameManager.Instance.UpdateCaracter(); break; //치명타 배율
                    case 4: GameManager.Instance.UpdateWallData(); break; //담장 체력 증가
                }
                break;
            case UpgradeType.Management:
                switch (data.ButtonIndex)
                {
                    case 0: GameManager.Instance.UpdateCaracter(); break; //캐릭터 수 증가
                    case 1: GameManager.Instance.UpdateWallData(); break; //담장체력 증가
                }
                break;
            case UpgradeType.Attack:
                switch (data.ButtonIndex)
                {
                    case 0: GameManager.Instance.UpdateCaracter(); break; //공격력 증가
                    case 1: GameManager.Instance.UpdateCaracter(); break; //공격속도 증가
                    case 2: GameManager.Instance.UpdateCaracter(); break; //치명타 확률
                    case 3: GameManager.Instance.UpdateCaracter(); break; //치명타 배율
                }
                break;
            case UpgradeType.Making:
                switch (data.ButtonIndex)
                {
                    case 0:
                    case 1:
                    case 2:
                        //수동제작시간감소, 제작화살레벨 증가 알아서 자동적용.
                        break;
                    case 3:
                    case 4:
                        SetCooltime();
                        break;
                }
                break;
        }
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
    
    float _autoMergeTimer = 0;
    float _autoMakingTimer = 0;
    float _autoSaveTimer = 0;
    float _maxMakingCooltime = 0;
    float _makingCooltime = 0;
    float _mergeCooltime = 0;

    float _autoMakingCooltime = 0;
    float _AutoMergeCooltime = 0;
    
    void SetCooltime()
    {
        //자동화살제작 쿨타임
        _autoMakingCooltime = 11f -
            ((float)Data.Instance.GetUpgradeData(UpgradeType.Making, 3).Level * Data.Instance.GetUpgradeData(UpgradeType.Making, 3).Increase);
        //자동화살합성 쿨타임
        _AutoMergeCooltime = 11f -
            ((float)Data.Instance.GetUpgradeData(UpgradeType.Making, 4).Level * Data.Instance.GetUpgradeData(UpgradeType.Making, 4).Increase);
    }

    void MergeInventoryOpen()
    {
        _mergeInventory.SetActive(true);
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
        //업그레이드 데이터 받아와서 수정 필요
        //장비창 갱신 - 세이브데이터에다가 인벤토리 데이터랑 장비창 데이터 저장하는것 만들기
        for (int i = 0; i < GameManager.Instance.EquipArrowData.Length; i++)
        {
            if (GameManager.Instance.EquipArrowData[i] > 0)
            {
                _equipSlot.GetChild(i).gameObject.SetActive(true);
                _equipSlot.GetChild(i).GetComponent<EquipSlot>().Init(GameManager.Instance.EquipArrowData[i], i);
            }
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
        for(int i = 0; i < GameManager.Instance.InventoryData.Count; i++)
        {
            _inventorySlot.GetChild(i).GetComponent<InventorySlot>().Init(GameManager.Instance.InventoryData[i], i);
        }
    }

    void MakingArrow()
    {
        if (GameManager.Instance.InventoryData.Count < 40)
        {
            GameManager.Instance.InventoryData.Add(GameManager.Instance.GetMakingArrowLevel());
            //_inventorySlot.GetChild(_inventoryData.Count - 1).GetComponent<InventorySlot>().Init(_inventoryData[_inventoryData.Count - 1]);
            InventorySlotAllClose();
            UpdateInventory();
            UIManager.Instance.PLayUISound(SoundIndex.MakingArrow);
        }
    }

    public void OnclickMakingArrow()
    {
        if(_makingCooltime <= 0)
        {
            MakingArrow();
            _maxMakingCooltime = 5f -
                ((float)Data.Instance.GetUpgradeData(UpgradeType.Making, 0).Level * Data.Instance.GetUpgradeData(UpgradeType.Making, 0).Increase);
            _makingCooltime = _maxMakingCooltime;
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
        for (int i = 0; i < GameManager.Instance.InventoryData.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.InventoryData.Count; j++)
            {
                if (i != j)
                {
                    if (GameManager.Instance.InventoryData[i] == GameManager.Instance.InventoryData[j])
                    {
                        GameManager.Instance.InventoryData[i]++;
                        GameManager.Instance.InventoryData.RemoveAt(j);
                        InventorySlotAllClose();
                        UpdateInventory();
                        UIManager.Instance.PLayUISound(SoundIndex.MergeArrow);
                        return;
                    }
                }
            }
        }
    }

    public void OnClickSort()
    {
        var datas = from data in GameManager.Instance.InventoryData
                    orderby data descending
                    select data;
        GameManager.Instance.InventoryData = datas.ToList();
        InventorySlotAllClose();
        UpdateInventory();
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
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


