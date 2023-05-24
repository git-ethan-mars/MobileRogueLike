using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Swordsman : Enemy
{
    [SerializeField] private MeleeAttack meleeAttack;
    [SerializeField] private float attackRange;
    [SerializeField] public GameObject[] lootPrefabs;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource attackAudioSource;

    public void Update()
    {
        if (IsDead)
        {
            deathAudioSource.Play();
            Instantiate(lootPrefabs[Random.Range(0, lootPrefabs.Length)], transform.position, transform.rotation);
            boxCollider.enabled = false;
            rigidBody.bodyType = RigidbodyType2D.Static;
            enabled = false;
        }
        if (player.IsDead || IsDead || isForced) return;
        Move();
        var distance = GetDistanceToPlayer();
        if (distance < attackRange &&
            ((moveDirection == MoveDirection.Left && player.transform.position.x < transform.position.x) ||
             (moveDirection == MoveDirection.Right && player.transform.position.x > transform.position.x)))
            Attack();
    }

    private void Attack()
    {
        if (currentState == ATTACK) return;
        isForced = true;
        rigidBody.velocity = Vector2.zero;
        attackAudioSource.Play();
        ChangeAnimationState(ATTACK);
        StartCoroutine(Utils.DoActionAfterDelay(0.3f, () =>
        {
            var offset = 0.5f * (moveDirection == MoveDirection.Left ? -1 : 1);
            var position = transform.position + new Vector3(offset, 0.3f);
            WeaponManager.CreateMeleeAttack(position, transform.rotation, meleeAttack);
            StartCoroutine(Utils.DoActionAfterAnimationFinished(animator, ATTACK,
                () =>
                {
                    if (IsDead) return;
                    ChangeAnimationState(WALK);
                    isForced = false;
                }
            ));
        }));
    }
}