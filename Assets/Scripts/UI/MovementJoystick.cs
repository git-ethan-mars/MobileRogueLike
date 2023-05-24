using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class MovementJoystick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private GameObject joystick;
        [SerializeField] private GameObject joystickBackground;
        public UnityEvent<float> OnHorizontalJoystickInput { get; private set; }

        private Vector2 JoystickCenterPos { get; set; }
        private float JoystickRadius { get; set; }
        private RectTransform RectTransform { get; set; }
        private bool IsJoystickDragging { get; set; }
        private Vector2 DragPosition { get; set; }
        
        public void InitializeEvent()
        {
            OnHorizontalJoystickInput = new UnityEvent<float>();
        }

        private void Start()
        {
            JoystickCenterPos = joystickBackground.transform.position;
            RectTransform = joystickBackground.GetComponent<RectTransform>();
            JoystickRadius = Utils.GetScreenCoordinates(RectTransform).size.x / 2;
        }
        
        private void Update()
        {
            JoystickCenterPos = joystickBackground.transform.position;
            JoystickRadius = JoystickRadius = Utils.GetScreenCoordinates(RectTransform).size.x / 2;
            if (IsJoystickDragging)
            {
                joystick.transform.position = Vector2.Lerp(JoystickCenterPos, DragPosition,
                    JoystickRadius / Vector2.Distance(JoystickCenterPos, DragPosition));
                var horizontalInput = GetHorizontalJoystickSensitivity();
                if (Math.Abs(horizontalInput) > 0.2f)
                    OnHorizontalJoystickInput?.Invoke(Math.Sign(horizontalInput));
            }
            else
            {
                joystick.transform.position = JoystickCenterPos;
                OnHorizontalJoystickInput?.Invoke(0);
            }
        }

        public void OnDrag(PointerEventData pointerEventData)
        {
            DragPosition = pointerEventData.position;
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsJoystickDragging = false;
        }
        


        public void OnPointerDown(PointerEventData eventData)
        {
            IsJoystickDragging = true;
        }

        private float GetHorizontalJoystickSensitivity()
        {
            return (joystick.transform.position.x - JoystickCenterPos.x) / JoystickRadius;
        }
    }
}