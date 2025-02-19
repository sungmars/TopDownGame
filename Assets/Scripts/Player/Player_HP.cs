using UnityEngine;
using System.Collections;

public class Player_HP : Base_HP
{
    public float knockbackForce = 3f; // 넉백 힘
    private Rigidbody2D prb;
    private bool isInvincible = false; // 무적 상태
    private Player_Animation playerAnimation;

    protected override void Start()
    {
        MaxHP = 100;
        base.Start();
        prb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<Player_Animation>();

        // Ensure playerAnimation is not null
        if (playerAnimation == null)
        {
            Debug.LogError("Player_Animation component is missing on the player object.");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isInvincible) return; // 무적 상태라면 데미지를 받지 않음

        base.TakeDamage(damage);
        StartCoroutine(InvincibilityTimer()); // 1초 무적 시작
        Knockback();
    }

    private void Knockback()
    {
        Vector2 knockbackDir = -(transform.position - GameObject.FindGameObjectWithTag("Enemy").transform.position).normalized;
        prb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
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
        Debug.Log("플레이어가 사망했습니다.");
        if (playerAnimation != null)
        {
            playerAnimation.Dead(); // 사망 애니메이션 실행
        }
        else
        {
            Debug.LogError("Player_Animation component is missing, cannot play death animation.");
        }
    }
}
