using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerController : MonoBehaviour
{
    public UnityEvent<float> OnHorizontalInput { get; set; }
    public UnityEvent OnAttackButtonClick { get; set; }
    public UnityEvent OnShootButtonClick { get; set; }
    public UnityEvent OnJumpButtonClick { get; set; }

    protected void Awake()
    {
        OnHorizontalInput = new UnityEvent<float>();
        OnAttackButtonClick = new UnityEvent();
        OnShootButtonClick = new UnityEvent();
        OnJumpButtonClick = new UnityEvent();
    }
}