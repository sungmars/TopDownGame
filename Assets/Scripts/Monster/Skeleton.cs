using UnityEngine;

public class Skeleton : Base_Monster
{
    protected override void Start()
    {
        base.Start();
        speed = 2f; // 스켈레톤 이동 속도 (기본 몬스터보다 느리게)
        damage = 15; // 스켈레톤 공격력 증가
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("스켈레톤이 사망했습니다!");
    }
}
