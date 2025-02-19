using UnityEngine;

public class Base_HP : MonoBehaviour
{
    public int MaxHP { get; protected set; }
    public int CurrentHP { get; protected set; }
    public bool isDead = false;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        CurrentHP = MaxHP;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return; // 이미 사망한 경우 추가 데미지 무시

        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return; // 중복 실행 방지
        isDead = true;

        Debug.Log(gameObject.name + "이(가) 사망했습니다.");

        // 트랜스폼 고정
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // 속도 초기화
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // 위치, 회전 고정
        }
        else
        {
            // Rigidbody2D가 없을 경우 Transform을 강제로 고정
            Vector3 fixedPosition = transform.position;
            Quaternion fixedRotation = transform.rotation;
            transform.position = fixedPosition;
            transform.rotation = fixedRotation;
        }

    }
}
