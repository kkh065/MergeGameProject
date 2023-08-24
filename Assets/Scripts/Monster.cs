using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Vector2 _targetPos;
    int _maxHP = 0;
    int _hp = 0;
    int _attackDamage = 1;

    public void InitMonster(MonsterType type)
    {
        _targetPos = GameManager.Instance.GetWallPos();
        switch (type)
        {
            case MonsterType.Nomal:
                _maxHP = 10 + GameManager.Instance.Stage;
                break;
            case MonsterType.Boss:
                _maxHP = GameManager.Instance.Stage * 10;
                break;
        }

        _hp = _maxHP;
    }

    private void Update()
    {
        //�������� ��� ���Բ������, ���̶� �����Ÿ� ������ ������ ����
    }
}
