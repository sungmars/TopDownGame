using UnityEngine;
using System.Collections;

public class Player_HP : Base_HP
{
    public float knockbackForce = 3f; // 넉백 힘
    private Rigidbody2D prb;
    private bool isInvincible = false; // 무적 상태
    private Player_Animation playerAnimation;
    private Vector2 knockbackVelocity;
    private bool isKnockedBack = false; // 넉백 상태 확인
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine; // 깜빡임 코루틴 참조
    public GameObject GameOver;//활성화할 게임오버 패널
    protected override void Start()
    {
        MaxHP = 100;
        base.Start();
        prb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<Player_Animation>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        GameOver.SetActive(false);
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
            prb.velocity = knockbackVelocity;
            Debug.Log($"넉백 적용 중: {prb.velocity}");
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.5f); // 넉백 지속 시간 증가
        isKnockedBack = false;
        prb.velocity = Vector2.zero;
        Debug.Log("넉백 속도 초기화됨");
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        Debug.Log("플레이어 무적 상태 시작");

        // 기존에 실행 중이던 깜빡임 효과 중지 후 새로 시작
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkEffect());

        yield return new WaitForSeconds(1f);

        isInvincible = false;
        Debug.Log("플레이어 무적 상태 해제");

        // 깜빡임 중지 및 원래 색상 복원
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 원래 상태
        }
    }

    private IEnumerator BlinkEffect()
    {
        float blinkDuration = 1f; // 무적 시간(1초) 동안 깜빡임
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
        if (isDead) return; // 중복 실행 방지
        isDead = true;
        isKnockedBack = false; // 사망 시 넉백 중지
        prb.velocity = Vector2.zero; // 사망 시 즉시 멈춤
        GameOver.SetActive(true);
        // 사망 시 깜빡임 중지 및 원래 색상 복원
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 원래 상태
        }

        GetComponent<PlayerController>().enabled = false;
        GetComponent<Player_Attack>().enabled = false;
        prb.simulated = false; // 물리 충돌도 비활성화

        Debug.Log("플레이어가 사망했습니다.");
        playerAnimation.Dead(); // 사망 애니메이션 실행
    }

}
