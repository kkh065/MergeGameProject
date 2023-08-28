using UnityEngine;
using UnityEngine.UI;

public class WallHpSlider : MonoBehaviour
{
    [SerializeField] Image _HpImage;

    public void SetHpUI()
    {
        _HpImage.fillAmount = (float)GameManager.Instance.WallHP / (float)GameManager.Instance.WallMaxHP;
    }
}
