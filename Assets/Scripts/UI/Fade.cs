using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image _fade;
    bool _isFadeIn;
    bool _isFadeOut;
    Color c = new Color();
    private void Awake()
    {
        _fade = GetComponent<Image>();
        c.a = 1;
        _fade.color = c;
    }

    void Update()
    {
        if(_isFadeIn)
        {
            c.a += Time.deltaTime;
            _fade.color = c;

            if(_fade.color.a >= 1)
            {
                _isFadeIn = false;
                GameManager.Instance.StageLoad();
            }
        }

        if (_isFadeOut)
        {
            c.a -= Time.deltaTime;
            _fade.color = c;

            if (_fade.color.a <= 0)
            {
                _isFadeOut = false;
                GameManager.Instance.WaveLoad();
            }
        }
    }

    public void FaidIn()
    {
        gameObject.SetActive(true);
        c.a = 0;
        _isFadeIn = true;
    }

    public void FaidOut()
    {
        gameObject.SetActive(true);
        c.a = 1;
        _isFadeOut = true;
    }
}
