using System;
using UnityEngine;

public class MenuListController : MonoBehaviour
{
    [SerializeField] GameObject MenuList;
    [SerializeField] GameObject OptionUI;

    private void Start()
    {
        UIManager.Instance.EventAllUIClose += new EventHandler(EnvetCloseMenu);
    }
    public void OpenMenu()
    {
        MenuList.SetActive(true);
        //·¹º§¿¡ ¸ÂÃç¼­ ¿ÀÇÂµÈ ÄÁÅÙÃ÷ ¹öÆ°¸¸ ³ëÃâµÇ°Ô²û ÄÚµù
    }

    void EnvetCloseMenu(object sender, EventArgs s)
    {
        CloseMenu();
    }

    void CloseMenu()
    {
        MenuList.SetActive(false);
    }

    public void OnSaveButton()
    {
        Data.Instance.SaveInventoryData();
        GameManager.Instance.SaveGameData();
        CloseMenu();
    }

    public void OnExitGameButton()
    {
        Application.Quit();
    }

    public void OnOptionButton()
    {
        OptionUI.SetActive(true);
        CloseMenu();
    }
}
