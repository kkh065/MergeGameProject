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
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
    }

    void EnvetCloseMenu(object sender, EventArgs s)
    {
        CloseMenu();
    }

    void CloseMenu()
    {
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
        MenuList.SetActive(false);
        UIManager.Instance.UIActive = false;
    }

    public void OnSaveButton()
    {
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
        Data.Instance.SaveInventoryData();
        GameManager.Instance.SaveGameData();
        CloseMenu();
    }

    public void OnExitGameButton()
    {
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
        UIManager.Instance.OpenNotice("게임을 종료하시겠습니까?", () => Application.Quit(), UIManager.Instance.AllClose);
    }

    public void OnOptionButton()
    {
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
        UIManager.Instance.UIActive = true;
        OptionUI.SetActive(true);
        CloseMenu();
    }
}
