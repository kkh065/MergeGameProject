using System;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] Notification _notice;

    public EventHandler EventVolumeChange;
    public EventHandler EventAllUIClose;
    private static UIManager instance = null;
    public bool UIActive { get; set; } = false;

    public static UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        VolumeChange();
    }

    private void Update()
    {
        if(!UIActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenNotice("게임을 종료하시겠습니까?", () => Application.Quit(), AllClose);
            }
        }
    }

    public void PLayUISound(SoundIndex sound)
    {
        _audio.clip = Data.Instance.SoundEffect[(int)sound];
        _audio.Play();
    }

    public void VolumeChange()
    {
        EventVolumeChange?.Invoke(this, EventArgs.Empty);
        _audio.volume = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
    }

    public void AllClose()
    {
        EventAllUIClose?.Invoke(instance, EventArgs.Empty);
        UIActive = false;
    }

    public void OpenNotice(string Main, UnityAction BtnAct1, UnityAction BtnAct2, string BtnText1 = "예", string BtnText2 = "아니오")
    {
        _notice.gameObject.SetActive(true);
        _notice.InitNotice(Main, BtnAct1, BtnAct2, BtnText1, BtnText2);
        UIActive = true;
    }
}
