using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingAni : MonoBehaviour
{
    [SerializeField] Text _textLoading;
    Slider _sliderLoading;

    float _timeLoading;
    // Start is called before the first frame update
    void Start()
    {
        _sliderLoading = GetComponent<Slider>();
        _sliderLoading.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeLoading > 100)
        {
            _timeLoading = 100;
            _sliderLoading.value = 1;
            Data.Instance.NextScene();
        }
        else
        {
            _timeLoading += Time.deltaTime * 30;
            _sliderLoading.value = _timeLoading / 100;
        }
        _textLoading.text = ((int)_timeLoading).ToString() + "%";
    }
}