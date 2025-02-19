using UnityEngine;

public class Base_Monster : MonoBehaviour
{
    [Header("몬스터 기본 속성")]
    [SerializeField] protected float speed = 3f; // 이동 속도
    [SerializeField] protected int damage = 10; // 플레이어에게 주는 데미지
    protected bool isDead = false;

    protected Transform player;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (isDead || player == null) return;
        MoveTowardsPlayer();
    }

    protected void MoveTowardsPlayer()
    {
        // 플레이어 추적
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed; // Rigidbody2D를 사용한 이동

        // 움직이는 방향에 따라 flipX 설정
        spriteRenderer.flipX = direction.x < 0;

        // 애니메이션 업데이트
        animator.SetBool("IsWalking", true);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player_HP>()?.TakeDamage(damage);
        }
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("IsDie", true);
        rb.velocity = Vector2.zero; // 죽을 때 이동 멈춤
        Destroy(gameObject, 1.5f); // 1.5초 후 제거
        Debug.Log(gameObject.name + "이(가) 사망했습니다.");
    }
}
