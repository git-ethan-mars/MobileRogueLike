using UnityEngine;

public class Spike : Damaging
{
    [SerializeField]
    private int damage;

    public override int Damage => damage;
}

