using UnityEngine;

public abstract class BaseAttackInfo : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private bool isMadeByPlayer;

    public int Damage => damage;
    public bool IsMadeByPlayer => isMadeByPlayer;

    public void IncreaseDamage(int value)
    {
        damage += value;
    }
}