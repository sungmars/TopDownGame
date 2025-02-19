using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private bool isDead = false; // 사망 상태 확인

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    public void UpdateAnimation(Vector2 movement)
    {
        if (isDead) return; // 사망 상태면 애니메이션 업데이트 중지

        bool isAttacking = Input.GetMouseButton(0); // 마우스 왼쪽 버튼 공격

        ResetAnimations(); // 기존 애니메이션 초기화

        // 이동 애니메이션 처리
        if (movement != Vector2.zero)
        {
            if (movement.y > 0)
                animator.SetBool("IsBack_Walk", true);
            else if (movement.y < 0)
                animator.SetBool("IsFore_Walk", true);
            else
            {
                animator.SetBool("IsRight_Walk", true);
                spriteRenderer.flipX = movement.x < 0; // 왼쪽 이동 시 flipX 적용
            }
        }

        // 마우스 방향을 기준으로 공격 방향 설정
        if (isAttacking)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = transform.position;
            Vector2 direction = (mousePosition - playerPosition).normalized;

            if (direction.y > 0.5f) // 위쪽 공격
                animator.SetBool("IsBack_Attack", true);
            else if (direction.y < -0.5f) // 아래쪽 공격
                animator.SetBool("IsFore_Attack", true);
            else // 좌우 공격
            {
                animator.SetBool("IsRight_Attack", true);
                spriteRenderer.flipX = direction.x < 0; // 마우스가 왼쪽이면 flipX 활성화
            }
        }
    }

    private void ResetAnimations()
    {
        animator.SetBool("IsFore_Walk", false);
        animator.SetBool("IsRight_Walk", false);
        animator.SetBool("IsBack_Walk", false);
        animator.SetBool("IsFore_Attack", false);
        animator.SetBool("IsBack_Attack", false);
        animator.SetBool("IsRight_Attack", false);
    }

    public void Dead()
    {
        if (isDead) return; // 이미 사망 상태면 실행 안 함

        isDead = true;
        ResetAnimations(); // 모든 애니메이션 초기화
        animator.SetTrigger("IsDie"); // 트리거 실행
        Debug.Log("플레이어 사망 애니메이션 실행");
    }
}
