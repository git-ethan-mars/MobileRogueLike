using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{
    public static readonly UnityEvent<GameObject> OnEnemyDied = new();
    public static readonly UnityEvent OnNextLevelRequest = new();
    public static void SendEnemyDeath(GameObject enemy)
    {
        OnEnemyDied.Invoke(enemy);        
    }

    public static void SendNextLevelRequest()
    {
        OnNextLevelRequest.Invoke();
    }
}