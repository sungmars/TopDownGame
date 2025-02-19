using UnityEngine;

public class Base_Player : MonoBehaviour
{
    public float speed = 5f;
    protected Rigidbody2D rb;
    protected Vector2 movement;
    public int AttackPower { get; private set; } = 15; // 공격력 15
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }
}
