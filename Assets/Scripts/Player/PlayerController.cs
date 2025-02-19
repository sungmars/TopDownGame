using UnityEngine;

public class PlayerController : Base_Player
{
    private Player_Animation playerAnimation;

    public float interactionRange = 1.5f; // NPC 감지 범위

    protected override void Start()
    {
        base.Start();
        playerAnimation = GetComponentInChildren<Player_Animation>();
    }

    protected override void Update()
    {
        base.Update();
        playerAnimation.UpdateAnimation(movement);
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InteractWithNPC();
        }
    }

    private void InteractWithNPC()
    {
        // 플레이어 주변에 있는 NPC 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("NPC")) // NPC 태그 확인
            {
                NPC_Talk npcTalk = collider.GetComponentInChildren<NPC_Talk>();
                if (npcTalk != null)
                {
                    npcTalk.Talk(); // NPC의 Talk() 호출
                    break; // 하나의 NPC만 상호작용
                }
            }
            if(collider.CompareTag("Potal"))
            {
                Succle succle = collider.GetComponentInChildren<Succle>();
                if (succle != null)
                {
                    succle.Teleport();
                    break;
                }
                
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 상호작용 범위 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
