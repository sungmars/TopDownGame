using UnityEngine;
using System.Collections;

public class Player_HP : Base_HP
{
    public float knockbackForce = 3f; // 넉백 힘
    private Rigidbody2D rb;
    private bool isInvincible = false; // 무적 상태
    private Player_Animation playerAnimation;
    private new bool isDead = false;
    private Vector2 knockbackVelocity;
    private bool isKnockedBack = false; // 넉백 상태 확인

    protected override void Start()
    {
        MaxHP = 100;
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Player_Animation>();
    }

    public override void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return; // 무적 상태 또는 사망 상태면 데미지 무시

        base.TakeDamage(damage);
        StartCoroutine(InvincibilityTimer()); // 1초 무적 시작

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy"); // 공격한 적 찾기
        if (enemy != null)
        {
            Knockback(enemy.transform.position); // 적의 위치를 기준으로 넉백
        }
        else
        {
            Debug.LogWarning("넉백 대상(Enemy)을 찾을 수 없습니다.");
        }
    }

    private void Knockback(Vector2 enemyPosition)
    {
        if (isDead) return; // 사망 시 넉백 발생 X

        Vector2 knockbackDir = (transform.position - (Vector3)enemyPosition).normalized; // 몬스터 반대 방향

        // 넉백 방향이 (0,0)이면 랜덤 방향으로 강제 설정
        if (knockbackDir == Vector2.zero)
        {
            knockbackDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Debug.Log("넉백 방향이 (0,0)이라 랜덤 방향으로 설정됨: " + knockbackDir);
        }

        knockbackVelocity = knockbackDir * knockbackForce; // 적용할 넉백 벡터
        isKnockedBack = true; // 넉백 시작

        Debug.Log($"플레이어 넉백 방향: {knockbackDir}, 힘: {knockbackVelocity}");

        StartCoroutine(ResetKnockback());
    }

    // FixedUpdate에서 velocity 강제 적용
    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            rb.velocity = knockbackVelocity;
            Debug.Log($"넉백 적용 중: {rb.velocity}");
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f); // 넉백 지속 시간 증가
        isKnockedBack = false;
        rb.velocity = Vector2.zero;
        Debug.Log("넉백 속도 초기화됨");
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        Debug.Log("플레이어 무적 상태 시작");
        yield return new WaitForSeconds(1f);
        isInvincible = false;
        Debug.Log("플레이어 무적 상태 해제");
    }

    protected override void Die()
    {
        if (isDead) return; // 중복 실행 방지
        isDead = true;
        isKnockedBack = false; // 사망 시 넉백 중지
        rb.velocity = Vector2.zero; // 사망 시 즉시 멈춤
        Debug.Log("플레이어가 사망했습니다.");
        playerAnimation.Dead(); // 사망 애니메이션 실행
    }
}
