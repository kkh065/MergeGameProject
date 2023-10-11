using UnityEngine;
using UnityEngine.UI;

public class WallHpSlider : MonoBehaviour
{
    Image _HpImage;
    [SerializeField] Text _HpText;

    private void Awake()
    {
        _HpImage = GetComponent<Image>();
    }
    public void SetHpUI()
    {
        _HpImage.fillAmount = (float)GameManager.Instance.WallHP / (float)GameManager.Instance.WallMaxHP;
        _HpText.text = GameManager.Instance.WallHP.ToString();
    }
}
