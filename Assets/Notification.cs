using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] Text _textMain;
    [SerializeField] Text _textButtonYes;
    [SerializeField] Text _textButtonNo;
    [SerializeField] Button _button1;
    [SerializeField] Button _button2;

    private void Start()
    {
        UIManager.Instance.EventAllUIClose += new EventHandler(EventClose);
        NoticeClose();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            NoticeClose();
        }
    }

    public void InitNotice(string Main, UnityAction BtnAct1, UnityAction BtnAct2, string BtnText1, string BtnText2)
    {
        _textMain.text = Main;
        _button1.onClick.AddListener(BtnAct1);
        if(BtnAct2 != null)
        {
            _button2.onClick.AddListener(BtnAct2);
        }
        else
        {
            _button2.onClick.AddListener(NoticeClose);
        }
        _textButtonYes.text = BtnText1;
        _textButtonNo.text = BtnText2;
    }
    void EventClose(object sender, EventArgs s)
    {
        NoticeClose();
    }

    void NoticeClose()
    {
        UIManager.Instance.PLayUISound(SoundIndex.UIButton);
        gameObject.SetActive(false);
        UIManager.Instance.UIActive = false;
    }
}
