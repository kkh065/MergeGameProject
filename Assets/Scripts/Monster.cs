using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject _hpPanel;
    [SerializeField] Image _imageHP;
    

    Vector3 _targetPos;
    int _maxHP = 0;
    int _hp = 0;
    int _attackDamage = 1;
    float attackSpeed = 1;
    float attackCooltime = 0;
    MonsterType _type;
    IObjectPool<Monster> _pool;
    Animator _ani;

    private void Awake()
    {
        _hpPanel.SetActive(false);
        _ani = GetComponent<Animator>();
    }

    public void InitMonster(MonsterType type, IObjectPool<Monster> pool)
    {
        _targetPos = GameManager.Instance.GetWallPos();
        _type = type;
        switch (_type)
        {
            case MonsterType.Nomal:
                _maxHP = 3 + (GameManager.Instance.Stage * 2);
                gameObject.transform.localScale = Vector3.one;
                break;
            case MonsterType.Boss:
                _maxHP = GameManager.Instance.Stage * 10;
                gameObject.transform.localScale = new Vector3(2, 2, 2);
                break;
        }

        _hp = _maxHP;
        Debug.Log($"몬스터 체력 : {_hp}");
        _targetPos = GameManager.Instance.GetWallPos();
        _pool = pool;
    }

    private void FixedUpdate()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        _hp -= dmg;
        SetHpSlider();
        Debug.Log($"받은 데미지 : {dmg}");
        if( _hp <= 0 )
        {
            GameManager.Instance.MonsterDie(gameObject, _type);
            _pool.Release(this);
        }
    }

    private void Update()
    {
        //벽을향해 계속 가게끔만들고, 벽이랑 일정거리 안으로 들어오면 공격
        if (_targetPos == null) return;

        if ((transform.position.x - _targetPos.x) < 1)
        {
            attackCooltime += Time.deltaTime;
            if (attackCooltime > attackSpeed)
            {
                //공격                
                GameManager.Instance.WallHP -= _attackDamage;
                attackCooltime = 0;
                _ani.SetBool("isWalk", false);
            }
        }
        else
        {
            Vector3 dir = _targetPos - transform.position;
            dir.Normalize();
            transform.position += new Vector3(dir.x * Time.deltaTime * 200f, 0, 0);
            _ani.SetBool("isWalk", true);
        }
    }

    public void ResetMonster()
    {
        _pool.Release(this);
    }

    void SetHpSlider()
    {
        if (_hpPanel.activeSelf == false) _hpPanel.SetActive(true);
        _imageHP.fillAmount = (float)_hp / (float)_maxHP;
    }
}
