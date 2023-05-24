using UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSensorController : PlayerController
{
    [SerializeField] private MovementJoystick movementJoystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button shootButton;

    public new void Awake()
    {
        base.Awake();
        movementJoystick.InitializeEvent();
        movementJoystick.OnHorizontalJoystickInput.AddListener(input=>OnHorizontalInput.Invoke(input));
        jumpButton.onClick.AddListener(()=>OnJumpButtonClick.Invoke());
        attackButton.onClick.AddListener(()=>OnAttackButtonClick.Invoke());
        shootButton.onClick.AddListener(()=>OnShootButtonClick.Invoke());
    }
}
