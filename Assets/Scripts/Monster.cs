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
        //벽을향해 계속 가게끔만들고, 벽이랑 일정거리 안으로 들어오면 공격
    }
}
