using System;
using UnityEngine;
using UnityEngine.UI;

public class WallHpSlider : MonoBehaviour
{
    Image _HpImage;
    [SerializeField] Text _HpText;
    [SerializeField] AudioSource _audio;

    private void Awake()
    {
        _HpImage = GetComponent<Image>();
    }

    private void Start()
    {
        UIManager.Instance.EventVolumeChange += new EventHandler(EventVolumeChange);
        _audio.volume = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
    }

    void EventVolumeChange(object sender, EventArgs s)
    {
        _audio.volume = PlayerPrefs.GetFloat("EffectVolume", 0.5f);
    }

    public void SetHpUI()
    {
        _HpImage.fillAmount = (float)GameManager.Instance.WallHP / (float)GameManager.Instance.WallMaxHP;
        _HpText.text = GameManager.Instance.WallHP.ToString();
    }

    public void HitSound()
    {
        _audio.clip = Data.Instance.SoundEffect[(int)SoundIndex.WallTakeDamage];
        _audio.Play();
    }
}
