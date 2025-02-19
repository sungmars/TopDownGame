using UnityEngine;

public class PlayerController : Base_Player
{

    private Player_Animation playerAnimation;

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
            Debug.Log("점프 또는 상호작용");
        }
    }
}
