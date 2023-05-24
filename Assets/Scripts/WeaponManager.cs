using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponManager : MonoBehaviour
{
    public static void CreateMeleeAttack(Vector3 position, Quaternion rotation, MeleeAttack meleeAttack)
    {
        var instantiate = Instantiate(meleeAttack.gameObject, position, rotation);
        Destroy(instantiate, 0.1f);
    }


    public static void CreateRangeAttack(Vector3 position, MoveDirection direction, RangeAttack rangeAttack)
    {
        var instantiatedArrow = Instantiate(rangeAttack, position, rangeAttack.transform.rotation);
        if (direction == MoveDirection.Left)
            instantiatedArrow.transform.Rotate(0, 0, 180);
        instantiatedArrow.GetComponent<Rigidbody2D>().velocity =
            rangeAttack.Speed * (direction == MoveDirection.Left ? -1 : 1) * Vector2.right;
    }
}