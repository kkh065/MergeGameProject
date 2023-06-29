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
