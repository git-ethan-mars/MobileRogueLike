using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [HideInInspector] public UnityEvent<GameObject> onDamageTaken;
    [HideInInspector] public UnityEvent onDied;
    [SerializeField] public int health;
    public int maxHealth;

    private void Awake()
    {
        onDied = new UnityEvent();
        onDamageTaken = new UnityEvent<GameObject>();
    }

    public void GetDamageFromDamaging(Damaging damaging)
    {
        if (name == "Player" && GetComponent<Player>().IsEvasionLearned
                             && Random.Range(0, 3) == 0)
            return;
        if (name == "Player")
            GetComponent<Player>().playerTakeDamageAudioSource.Play();
        health -= damaging.Damage;
        if (health <= 0)
        {
            health = 0;
            onDied.Invoke();
        }
        else
        {
            onDamageTaken.Invoke(damaging.gameObject);
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void IncreaseMaxHealth(int healthAmount)
    {
        maxHealth += healthAmount;
    }
}