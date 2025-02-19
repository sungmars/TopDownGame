using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public float attackRange = 1.5f; // 공격 거리
    public LayerMask enemyLayer; // 몬스터만 감지
    private Base_Player player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GetComponent<Base_Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // 자식 오브젝트에서 SpriteRenderer 찾기
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 공격
        {
            Attack();
        }
    }

    void Attack()
    {
        Vector2 attackDirection = GetMouseDirection();
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0, attackDirection, attackRange, enemyLayer);

        if (hit.collider != null)
        {
            Monster_HP monsterHP = hit.collider.GetComponent<Monster_HP>();
            if (monsterHP != null)
            {
                monsterHP.TakeDamage(player.AttackPower);
            }
        }
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0; // 방향에 따라 flipX 설정
        }
        

        return direction;
    }
}
