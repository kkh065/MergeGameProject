using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] Slider _sliderMainVolume;
    [SerializeField] Slider _sliderBGMVolume;
    [SerializeField] Slider _sliderEffectVolume;
    [SerializeField] Toggle _toggleMainMute;
    [SerializeField] Toggle _toggleBGMMute;
    [SerializeField] Toggle _toggleEffectMute;
    [SerializeField] Text _textMainVolume;
    [SerializeField] Text _textBGMVolume;
    [SerializeField] Text _textEffectVolume;

    float _tempMainVolume;
    float _tempBGMVolume;
    float _tempEffectVolume;

    private void Start()
    {
        //이니셜라이즈
        float Volume = PlayerPrefs.GetFloat("MainVolume", 0.5f);
       
        //메인볼륨
        _sliderMainVolume.value = Volume;
        _toggleMainMute.isOn = Volume == 0;
        _sliderMainVolume.interactable = !(Volume == 0);

        //BGM
        Volume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        _sliderBGMVolume.value = Volume;
        _toggleBGMMute.isOn = Volume == 0;
        _sliderBGMVolume.interactable = !(Volume == 0);

        //Effect Sound
        Volume = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
        _sliderEffectVolume.value = Volume;
        _toggleEffectMute.isOn = Volume == 0;
        _sliderEffectVolume.interactable = !(Volume == 0);

        UIManager.Instance.EventAllUIClose += new EventHandler(EnvetCloseMenu);
        CloseOption();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CloseOption();
        }
    }

    void EnvetCloseMenu(object sender, EventArgs s)
    {
        CloseOption();
    }

    public void OnCloseButton()
    {
        CloseOption();
    }

    void CloseOption()
    {
        gameObject.SetActive(false);
    }

    public void MainVolumeOnValueChange(float volume)
    {
        float temp = ((float)Mathf.RoundToInt(volume * 100) / 100);
        _sliderMainVolume.SetValueWithoutNotify(temp);
        PlayerPrefs.SetFloat("MainVolume", temp);
        _textMainVolume.text = $"전체볼륨 {temp * 100}%";
        AudioListener.volume = temp;
    }

    public void BGMVolumeOnValueChange(float volume)
    {
        float temp = ((float)Mathf.RoundToInt(volume * 100) / 100);
        _sliderBGMVolume.SetValueWithoutNotify(temp);
        PlayerPrefs.SetFloat("BGMVolume", temp);
        _textBGMVolume.text = $"BGM {temp * 100}%";
        GameManager.Instance.SetBGMVolume();
    }

    public void EffectVolumeOnValueChange(float volume)
    {
        float temp = ((float)Mathf.RoundToInt(volume * 100) / 100);
        _sliderEffectVolume.SetValueWithoutNotify(temp);
        PlayerPrefs.SetFloat("EffectVolume", temp);
        _textEffectVolume.text = $"효과음 {temp * 100}%";
        UIManager.Instance.VolumeChange();
    }

    public void MainMuteOnValueChange(bool Mute)
    {
        if (Mute)
        {
            _tempMainVolume = _sliderMainVolume.value;
            _sliderMainVolume.value = 0;
            _sliderMainVolume.interactable = false;
        }
        else
        {
            _sliderMainVolume.value = _tempMainVolume;
            _sliderMainVolume.interactable = true;
        }
    }

    public void BGMMuteOnValueChange(bool Mute)
    {
        if(Mute)
        {
            _tempBGMVolume = _sliderBGMVolume.value;
            _sliderBGMVolume.value = 0;
            _sliderBGMVolume.interactable = false;
        }
        else
        {
            _sliderBGMVolume.value = _tempBGMVolume;
            _sliderBGMVolume.interactable = true;
        }
    }

    public void EffectMuteOnValueChange(bool Mute)
    {
        if (Mute)
        {
            _tempEffectVolume = _sliderEffectVolume.value;
            _sliderEffectVolume.value = 0;
            _sliderEffectVolume.interactable = false;
        }
        else
        {
            _sliderEffectVolume.value = _tempEffectVolume;
            _sliderEffectVolume.interactable = true;
        }
    }
}
