using UnityEngine;

public class Experience : MonoBehaviour
{
    public ExperienceTypes type;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<Player>();
        if (player)
        {
            player.ExperienceSystem.AddExperienceByType(type, 1);
            Destroy(gameObject);
        }
    }
}
