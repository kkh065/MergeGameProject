using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class MainUIController : MonoBehaviour
{

    private void Awake()
    {
        MergeInventoryToggleIsON();
    }

    #region �������

    [SerializeField] GameObject _character;
    [SerializeField] GameObject _upgrade;
    [SerializeField] GameObject _mergeInventory;
    [SerializeField] GameObject _contents;
    [SerializeField] GameObject _shop;

    public void CharacterToggleIsON()
    {
        AllTabClose();
    }

    public void UpgradeToggleIsON()
    {
        AllTabClose();
        UpgradeOpne();
    }

    public void MergeInventoryToggleIsON()
    {
        AllTabClose();
    }

    public void ContentsToggleIsON()
    {
        AllTabClose();
    }

    public void ShopToggleIsON()
    {
        AllTabClose();
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


    #region ���׷��̵���

    [SerializeField] GameObject _goldUpgrade;
    [SerializeField] GameObject _managementUpgrade;
    [SerializeField] GameObject _attckUpgrade;
    [SerializeField] GameObject _makingUpgrade;
    [SerializeField] GameObject _specialUpgrade;

    //���׷��̵� �� ����
    void UpgradeOpne()
    {
        _upgrade.SetActive(true);
        GoldUpgradeToggleOn();
    }

    //���׷��̵� �� �������

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
        // ��� ���׷��̵� ���� ������Ʈ
        _goldUpgrade.SetActive(true);        
    }

    void ManagementUpgradeOpen()
    {
        // ���� ���׷��̵� ���� ������Ʈ
        _managementUpgrade.SetActive(true);
    }

    void AttckUpgradeOpen()
    {
        // ���� ���׷��̵� ���� ������Ʈ
        _attckUpgrade.SetActive(true);
    }

    void MakingUpgradeOpen()
    {
        // ���� ���׷��̵� ���� ������Ʈ
        _makingUpgrade.SetActive(true);
    }

    void SpecialUpgradeOpen()
    {
        // Ư�� ���׷��̵� ���� ������Ʈ
        _specialUpgrade.SetActive(true);
    }


    #endregion
}
