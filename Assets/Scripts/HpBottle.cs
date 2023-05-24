using UnityEngine;

public class HpBottle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<Player>();
        if (player)
        {
            player.HealthSystem.Heal(20);
            Destroy(gameObject);
        }
    }
}
