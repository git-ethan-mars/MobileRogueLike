using UnityEngine;

public class MeleeAttack : Damaging
{
    [SerializeField] private MeleeAttackInfo meleeAttackInfo;

    public override int Damage => meleeAttackInfo.Damage;

    public void IncreaseDamage(int value)
    {
        meleeAttackInfo.IncreaseDamage(value);
    }


    protected void OnTriggerEnter2D(Collider2D col)
    {
        var entity = col.gameObject.GetComponent<Entity>();
        if ((entity is Player && !meleeAttackInfo.IsMadeByPlayer) ||
            (entity is Enemy && meleeAttackInfo.IsMadeByPlayer))
        {
            entity.GetDamageFromDamaging(this);
        }
    }
}