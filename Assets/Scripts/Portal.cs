using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
            GlobalEvents.SendNextLevelRequest();
    }
}
