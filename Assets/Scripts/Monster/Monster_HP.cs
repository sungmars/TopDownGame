using UnityEngine;
using System.Collections;

public class Monster_HP : Base_HP
{
    public float knockbackForce = 2f; // 넉백 힘
    private Rigidbody2D mrb;
    private Base_Monster monster;
    private bool isDie = false;

    protected override void Start()
    {
        MaxHP = 45;
        base.Start();
        mrb = GetComponent<Rigidbody2D>();
        monster = GetComponent<Base_Monster>();
    }
    
    public override void TakeDamage(int damage)
    {
        if (isDie) return; // 사망 상태면 데미지 무시

        base.TakeDamage(damage);
        Knockback(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    private void Knockback(Vector2 playerPosition)
    {
        if (isDie) return; // 사망 시 넉백 발생 X

        Vector2 knockbackDir = (transform.position - (Vector3)playerPosition).normalized; // 플레이어 반대 방향
        mrb.velocity = Vector2.zero; // 기존 속도 초기화
        mrb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocity());
    }

    private IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        mrb.velocity = Vector2.zero;
    }

    protected override void Die()
    {
        if (isDie) return; // 중복 실행 방지
        isDie = true;
        mrb.velocity = Vector2.zero; // 사망 시 즉시 멈춤
        Debug.Log("몬스터가 사망했습니다.");
        monster.Die();
    }
}
