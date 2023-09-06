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
        Debug.Log($"���� ü�� : {_hp}");
        _targetPos = GameManager.Instance.GetWallPos();
        _pool = pool;
    }

    private void Update()
    {
        //�������� ��� ���Բ������, ���̶� �����Ÿ� ������ ������ ����
        if (_targetPos == null) return;

        float attackCooltime = 0;
        attackCooltime += Time.deltaTime;

        if (Vector3.Distance(_targetPos, transform.position) < 1)
        {
            if(attackCooltime > attackSpeed)
            {
                //����
                GameManager.Instance.WallHP -= _attackDamage;
                attackCooltime = 0;
            }
        }
        else
        {
            Vector3 dir = _targetPos - transform.position;
            dir.Normalize();
            transform.position += new Vector3(dir.x * Time.deltaTime * 1.5f, 0, 0);
        }
    }

    public void TakeDamage(int dmg)
    {
        _hp -= dmg;
        Debug.Log($"���� ������ : {dmg}");
        if( _hp <= 0 )
        {
            GameManager.Instance.MonsterDie(gameObject);
            _pool.Release(this);
        }
    }
}
