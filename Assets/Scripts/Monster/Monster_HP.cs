using UnityEngine;
using System.Collections;

public class Monster_HP : Base_HP
{
    public float knockbackForce = 2f; // 넉백 힘
    private Rigidbody2D mrb;
    private Base_Monster monster;
    private bool isDie = false;
    private Vector2 knockbackVelocity;
    private bool isKnockedBack = false; // 넉백 상태 확인
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine; // 깜빡임 코루틴 참조

    protected override void Start()
    {
        MaxHP = 45;
        base.Start();
        mrb = GetComponent<Rigidbody2D>();
        monster = GetComponent<Base_Monster>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void TakeDamage(int damage)
    {
        if (isDie) return; // 사망 상태면 데미지 무시

        base.TakeDamage(damage);
        Knockback(GameObject.FindGameObjectWithTag("Player").transform.position);

        // 기존에 실행 중이던 깜빡임 효과 중지 후 새로 시작
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkEffect());
    }

    private void Knockback(Vector2 playerPosition)
    {
        if (isDie) return; // 사망 시 넉백 발생 X

        Vector2 knockbackDir = (transform.position - (Vector3)playerPosition).normalized; // 플레이어 반대 방향

        // 넉백 방향이 (0,0)이면 랜덤 방향으로 강제 설정
        if (knockbackDir == Vector2.zero)
        {
            knockbackDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Debug.Log("넉백 방향이 (0,0)이라 랜덤 방향으로 설정됨: " + knockbackDir);
        }

        knockbackVelocity = knockbackDir * knockbackForce; // 적용할 넉백 벡터
        isKnockedBack = true; // 넉백 시작

        Debug.Log($"몬스터 넉백 방향: {knockbackDir}, 힘: {knockbackVelocity}");

        StartCoroutine(ResetKnockback());
    }

    // FixedUpdate에서 velocity 강제 적용
    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            mrb.velocity = knockbackVelocity;
            Debug.Log($"몬스터 넉백 적용 중: {mrb.velocity}");
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f); // 넉백 지속 시간 증가
        isKnockedBack = false;
        mrb.velocity = Vector2.zero;
        Debug.Log("몬스터 넉백 속도 초기화됨");
    }

    private IEnumerator BlinkEffect()
    {
        float blinkDuration = 2f; // 2초 동안 깜빡임
        float blinkInterval = 0.2f; // 깜빡이는 간격

        float timer = 0f;
        while (timer < blinkDuration)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // 반투명
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 원래 상태
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval * 2;
        }
    }

    protected override void Die()
    {
        if (isDie) return; // 중복 실행 방지
        isDie = true;
        isKnockedBack = false; // 사망 시 넉백 중지
        mrb.velocity = Vector2.zero; // 사망 시 즉시 멈춤

        // 사망 시 깜빡임 중지 및 원래 색상 복원
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 원래 상태
        }

        Debug.Log("몬스터가 사망했습니다.");
        monster.Die();
    }
}
