using UnityEngine;

public class PlayerKeyboardController : PlayerController
{
    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        OnHorizontalInput.Invoke(horizontalInput);
        if (Input.GetKey(KeyCode.Space))
        {
            OnJumpButtonClick.Invoke();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            OnAttackButtonClick.Invoke();
        }

        if (Input.GetKey(KeyCode.E))
        {
            OnShootButtonClick.Invoke();
        }
    }
}