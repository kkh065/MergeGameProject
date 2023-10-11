using UnityEngine;
using UnityEngine.UI;

public class CurrencyContoroller : MonoBehaviour
{
    [SerializeField] Text _goldText;
    [SerializeField] Text _diaText;
    [SerializeField] Text _reincarnationText;


    public void UpdateGold()
    {
        _goldText.text = GameManager.Instance.Gold.ToString();
    }

    public void UpdateDia()
    {
        _diaText.text = GameManager.Instance.Dia.ToString();
    }

    public void UpdateReincarnation()
    {
        _reincarnationText.text = GameManager.Instance.Reincarnation.ToString();
    }
}
