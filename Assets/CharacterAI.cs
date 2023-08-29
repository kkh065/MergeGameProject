using UnityEngine;
using UnityEngine.Pool;

public class CharacterAI : MonoBehaviour
{
    float _attackSpeed;
    float _attackCool;
    int _damage = 0;
    int _criticalDamage = 0;
    int _index = 0;

    IObjectPool<ArrowController> _arrowPool;


    public void InitCharacter(int idx, IObjectPool<ArrowController> ArrowPool)
    {
        //해당 캐릭터의 인덱스 설정
        _index = idx;
        _arrowPool = ArrowPool;
    }

    public void UpdateCharacterState()
    {
        //인덱스에 맞는 화살가져와서 데미지 설정.
        //각종 업그레이드들 (공격력,공격속도,치명타확률,치명타배율) 적용
    }

    // Update is called once per frame
    void Update()
    {
        _attackCool += Time.deltaTime;
        if (_attackCool > _attackSpeed && GameManager.Instance.LiveMonsterList.Count > 0)
        {
            Attack();
            _attackCool = 0;
        }
    }

    void Attack()
    {
        //크리티컬 확률 계산 후 크리티컬이면 크리뎀, 아니면 그냥뎀 넣어서 쏘자
        int LastDamage = 0;
        if(true)//크리티컬 계산
        {
            //크리티컬이면
            LastDamage = _criticalDamage;
        }
        else
        {
            LastDamage = _damage;
        }

        ArrowController Arrow = _arrowPool.Get();
        Arrow.transform.position = transform.position;
        Arrow.InitArrow(GameManager.Instance.LiveMonsterList[0], LastDamage, GameManager.Instance.EquipArrowData[_index], _arrowPool);
        //화살생성, 위치초기화, 화살 이닛 실행
    }
}
