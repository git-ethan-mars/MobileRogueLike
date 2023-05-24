using System.Linq;
using UnityEngine;

public class Cup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        GameObject.Find("UpperCanvas").GetComponentsInChildren<Transform>(true)
            .First(child => child.name == "RestartButton").gameObject.SetActive(true);
        GameObject.Find("UpperCanvas").GetComponentsInChildren<Transform>(true)
            .First(child => child.name == "WinLabel").gameObject.SetActive(true);
    }
}
