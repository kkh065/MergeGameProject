using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    Vector3 _targetPos;
    int _maxHP = 0;
    int _hp = 0;
    int _attackDamage = 1;
    float attackSpeed = 1;
    IObjectPool<Monster> _pool;

    public void InitMonster(MonsterType type, IObjectPool<Monster> pool)
    {
        _targetPos = GameManager.Instance.GetWallPos();
        switch (type)
        {
            case MonsterType.Nomal:
                _maxHP = 10 + GameManager.Instance.Stage;
                gameObject.transform.localScale = Vector3.one;
                break;
            case MonsterType.Boss:
                _maxHP = GameManager.Instance.Stage * 10;
                gameObject.transform.localScale = new Vector3(2, 2, 2);
                break;
        }

        _hp = _maxHP;
        _targetPos = GameManager.Instance.GetWallPos();
        _pool = pool;
    }

    private void Update()
    {
        //벽을향해 계속 가게끔만들고, 벽이랑 일정거리 안으로 들어오면 공격
        if (_targetPos == null) return;

        float attackCooltime = 0;
        attackCooltime += Time.deltaTime;

        if (Vector3.Distance(_targetPos, transform.position) < 0.1)
        {
            if(attackCooltime > attackSpeed)
            {
                //공격
                GameManager.Instance.WallHP -= _attackDamage;
                attackCooltime = 0;
            }
        }
        else
        {
            transform.position += (_targetPos - transform.position) * Time.deltaTime;
        }
    }

    public void TakeDamage(int dmg)
    {
        _hp -= dmg;
        if( _hp <= 0 )
        {
            GameManager.Instance.MonsterDie(gameObject);
            _pool.Release(this);
        }
    }
}
