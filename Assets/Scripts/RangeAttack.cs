using UnityEngine;

public class RangeAttack : Damaging
{
    [SerializeField] private RangeAttackInfo rangeAttackInfo;

    public override int Damage => rangeAttackInfo.Damage;
    public float Speed => rangeAttackInfo.Speed;

    public void IncreaseDamage(int value)
    {
        rangeAttackInfo.IncreaseDamage(value);
    }

    public void IncreaseSpeed(float value)
    {
        rangeAttackInfo.IncreaseSpeed(value);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        var entity = col.gameObject.GetComponent<Entity>();
        if (entity is Player && rangeAttackInfo.IsMadeByPlayer || entity is Enemy && !rangeAttackInfo.IsMadeByPlayer)
            return;
        if (entity)
        {
            entity.HealthSystem.GetDamageFromDamaging(this);
        }
        if (!rangeAttackInfo.IsPiercing)
        {
            Destroy(gameObject);
        }
    }
}