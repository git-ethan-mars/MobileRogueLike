using UnityEngine;
using UnityEngine.EventSystems;

public class Mage : Enemy
{
    [SerializeField] private RangeAttack rangeAttack;
    [SerializeField] private float shootRange;
    [SerializeField] public GameObject[] lootPrefabs;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource shootAudioSource;

    private void Update()
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
        var forwardObjects = Physics2D.RaycastAll(transform.position,
            new Vector2(5f * (moveDirection == MoveDirection.Left ? -1 : 1), rigidBody.velocity.y));
        if (forwardObjects.Length < 2) return;
        if (forwardObjects[1].collider.name == "Player" && distance < shootRange) Shoot();
    }

    private void Shoot()
    {
        if (currentState == SHOOT) return;
        isForced = true;
        rigidBody.velocity = Vector2.zero;
        shootAudioSource.Play();
        ChangeAnimationState(SHOOT);
        StartCoroutine(Utils.DoActionAfterDelay(0.3f, () =>
        {
            var offset = 0.5f * (moveDirection == MoveDirection.Left ? -1 : 1);
            var position = transform.position + new Vector3(offset, 0.3f);
            WeaponManager.CreateRangeAttack(position,
                moveDirection, rangeAttack);
            StartCoroutine(Utils.DoActionAfterAnimationFinished(animator, SHOOT,
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