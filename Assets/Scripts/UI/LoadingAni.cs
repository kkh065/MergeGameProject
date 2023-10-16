using UnityEngine;
using UnityEngine.UI;

public class LoadingAni : MonoBehaviour
{
    [SerializeField] Text _textLoading;
    Slider _sliderLoading;

    float _timeLoading;
    void Start()
    {
        _sliderLoading = GetComponent<Slider>();
        _sliderLoading.value = 0;
    }

    void Update()
    {
        if (_timeLoading > 100)
        {
            _timeLoading = 100;
            _sliderLoading.value = 1;
            Data.Instance.IsLoadEnd = true;
        }
        else
        {
            _timeLoading += Time.deltaTime * 50;
            _sliderLoading.value = _timeLoading / 100;
        }
        _textLoading.text = ((int)_timeLoading).ToString() + "%";
    }
}
