using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{
    [SerializeField] private int jumpPower;
    [Header("Attacks")] [SerializeField] private MeleeAttack meleeAttack;
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private RangeAttack piercingRangeAttack;
    private bool _isGrounded = true;
    public ExperienceSystem ExperienceSystem { get; set; }
    public bool IsEvasionLearned { get; set; }
    private PlayerController PlayerController { get; set; }
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource attackAudioSource;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] public AudioSource playerTakeDamageAudioSource;

    protected new void Awake()
    {
        base.Awake();
        ExperienceSystem = GetComponent<ExperienceSystem>();
        PlayerController = GetComponent<PlayerController>();
    }

    private new void Start()
    {
        base.Start();
        PlayerController.OnHorizontalInput.AddListener(Move);
        PlayerController.OnAttackButtonClick.AddListener(Attack);
        PlayerController.OnShootButtonClick.AddListener(()=>Shoot());
        PlayerController.OnJumpButtonClick.AddListener(Jump);
    }

    private void Update()
    {
        if (IsDead)
        {
            deathAudioSource.Play();
            GameObject.Find("UpperCanvas").GetComponentsInChildren<Transform>(true)
                .First(child => child.name == "RestartButton").gameObject.SetActive(true);
            GameObject.Find("UpperCanvas").GetComponentsInChildren<Transform>(true)
                .First(child => child.name == "LoseLabel").gameObject.SetActive(true);
            enabled = false;

        }
        else
            spriteRenderer.flipX = moveDirection == MoveDirection.Left;
    }


    private void Move(float direction)
    {
        if (IsDead || isForced)
            return;
        rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);
        if (direction < 0) moveDirection = MoveDirection.Left;
        if (direction > 0) moveDirection = MoveDirection.Right;
        if (_isGrounded && currentState != SHOOT && currentState != ATTACK)
        {
            ChangeAnimationState(direction == 0 ? IDLE : WALK);
        }
    }

    private void Jump()
    {
        if (IsDead || !_isGrounded) return;
        _isGrounded = false;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
        jumpAudioSource.Play();
        if (currentState != SHOOT && currentState != ATTACK)
            ChangeAnimationState(JUMP);
    }


    public void Shoot(bool isPiercing = false)
    {
        if (IsDead || currentState is SHOOT or ATTACK) return;
        shootAudioSource.Play();
        ChangeAnimationState(SHOOT);
        StartCoroutine(Utils.DoActionAfterDelay(0.45f,
            () =>
            {
                if (isPiercing)
                    WeaponManager.CreateRangeAttack(transform.position + new Vector3(0, 0.3f),
                        moveDirection, piercingRangeAttack);

                else
                    WeaponManager.CreateRangeAttack(transform.position + new Vector3(0, 0.3f),
                        moveDirection, rangeAttack);
                StartCoroutine(Utils.DoActionAfterAnimationFinished(animator, SHOOT,
                    () => ChangeAnimationState(_isGrounded ? IDLE : JUMP)));
            }));
    }

    private void Attack()
    {
        if (IsDead || currentState is ATTACK or SHOOT) return;
        attackAudioSource.Play();
        ChangeAnimationState(ATTACK);
        StartCoroutine(Utils.DoActionAfterDelay(0.35f, () =>
        {
            var offset = moveDirection == MoveDirection.Left ? -0.8f : 0.8f;
            var position = transform.position + new Vector3(offset, 0.3f);
            WeaponManager.CreateMeleeAttack(position, transform.rotation, meleeAttack);
            StartCoroutine(Utils.DoActionAfterAnimationFinished(animator, ATTACK,
                () => ChangeAnimationState(_isGrounded ? IDLE : JUMP)));
        }));
    }

    public void ChangeRangeAttack(int damage)
    {
        rangeAttack.IncreaseDamage(damage);
    }


    public void ChangeMeleeAttack(int damage)
    {
        meleeAttack.IncreaseDamage(damage);
    }
    
    public void ChangeSpeedValue(int changingValue)
    {
        speed += changingValue;
    }

    public void IncreaseRangeAttacksSpeed(float value)
    {
        rangeAttack.IncreaseSpeed(value);
        piercingRangeAttack.IncreaseSpeed(value);
    }

    public new void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);
        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("OneWayPlatform")
                                                 || col.gameObject.CompareTag("Chest")) &&
            transform.position.y - boxCollider.bounds.extents.y > col.GetContact(0).point.y)
            _isGrounded = true;
    }
}