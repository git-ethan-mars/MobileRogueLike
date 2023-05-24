using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable InconsistentNaming

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthSystem))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int speed;
    public int Speed => speed;
    protected internal bool IsDead { get; private set; }
    protected Rigidbody2D rigidBody { get; set; }
    protected BoxCollider2D boxCollider { get; set; }
    protected MoveDirection moveDirection { get; set; }
    protected SpriteRenderer spriteRenderer { get; set; }
    protected bool isForced { get; set; }
    public HealthSystem HealthSystem { get; set; }
    protected string currentState { get; private set; }
    protected Animator animator;
    protected const string IDLE = "Idle";
    protected const string JUMP = "Jump";
    protected const string WALK = "Walk";
    protected const string SHOOT = "Shoot";
    protected const string ATTACK = "Attack";
    private const string TAKE_DAMAGE = "TakeDamage";
    private const string DEATH = "Death";

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        HealthSystem = GetComponent<HealthSystem>();
    }


    protected void Start()
    {
        HealthSystem.onDamageTaken.AddListener(TakeDamageEventHandler);
        HealthSystem.onDied.AddListener(DeathEventHandler);
    }


    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Spike"))
        {
            var spike = col.gameObject.GetComponent<Spike>();
            HealthSystem.GetDamageFromDamaging(spike);
        }
    }

    public void GetDamageFromDamaging(Damaging damaging)
    {
        HealthSystem.GetDamageFromDamaging(damaging);
    }

    private void TakeDamageEventHandler(GameObject damaging)
    {
        const int forceStrength = 5;
        const float delay = 0.15f;
        isForced = true;
        var direction = (transform.position - damaging.transform.position).normalized;
        rigidBody.AddForce(direction * forceStrength, ForceMode2D.Impulse);
        StartCoroutine(Utils.DoActionAfterDelay(delay, () => { isForced = false; }));
        ChangeAnimationState(TAKE_DAMAGE);
    }

    private void DeathEventHandler()
    {
        IsDead = true;
        rigidBody.velocity = Vector2.zero;
        ChangeAnimationState(DEATH);
    }

    protected void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;
        currentState = newState;
        animator.Play(currentState);    
    }
}