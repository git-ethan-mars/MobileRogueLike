using UnityEngine;

[CreateAssetMenu(fileName = "New range attack", menuName = "Attack/Range attack")]
public class RangeAttackInfo : BaseAttackInfo
{
    [SerializeField] private bool isPiercing;
    public bool IsPiercing => isPiercing;

    [SerializeField] private float speed;

    public float Speed => speed;
    
    public void IncreaseSpeed(float value)
    {
        speed += value;
    }
}