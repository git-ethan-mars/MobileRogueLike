using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

// ReSharper disable Unity.InefficientPropertyAccess

// ReSharper disable Unity.PreferNonAllocApi

public abstract class Enemy : Entity
{
    protected Player player;
    private const float RayDistance = 1f;

    protected new void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }

    private new void Start()
    {
        base.Start();
        HealthSystem.onDied.AddListener(()=>GlobalEvents.SendEnemyDeath(gameObject));
    }


    protected float GetDistanceToPlayer()
    {
        return Vector2.Distance(player.transform.position, transform.position);
    }


    protected void Move()
    {
        if (isForced)
            return;
        var raycastHit = Physics2D.RaycastAll(transform.position - new Vector3(0, 0.3f),
            new Vector2(moveDirection == MoveDirection.Left ? -1 : 1, 0), RayDistance);
        if (raycastHit.Any(s => s.collider.CompareTag("Spike") || s.collider.CompareTag("Chest")))
        {
            moveDirection = moveDirection == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
        }

        if (!HasRightGround())
        {
            moveDirection = MoveDirection.Left;
        }

        if (!HasLeftGround())
        {
            moveDirection = MoveDirection.Right;
        }

        var newVelocity = new Vector2(speed * (moveDirection == MoveDirection.Left ? -1 : 1), rigidBody.velocity.y);
        rigidBody.velocity = newVelocity;
        if (currentState != SHOOT && currentState != ATTACK && newVelocity != Vector2.zero)
            ChangeAnimationState(WALK);
        spriteRenderer.flipX = moveDirection == MoveDirection.Left;
    }

    public new void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        if (col.gameObject.CompareTag("Wall"))
        {
            moveDirection = moveDirection == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
        }
    }


    private bool HasRightGround()
    {
        var hit = Physics2D.Raycast(transform.position, new Vector2(1, -1), RayDistance, LayerMask.GetMask("Default"));
        return hit.collider;
    }

    private bool HasLeftGround()
    {
        var hit = Physics2D.Raycast(transform.position, new Vector2(-1, -1), RayDistance, LayerMask.GetMask("Default"));
        return hit.collider;
    }
}