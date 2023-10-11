using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioSource _audio;

    public EventHandler EventVolumeChange;
    public EventHandler EventAllUIClose;
    private static UIManager instance = null;

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
    }
}
