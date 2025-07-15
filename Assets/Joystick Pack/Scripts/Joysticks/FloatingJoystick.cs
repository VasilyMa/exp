using System;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public event Action<InputType> OnJoystickDown;
    public event Action<InputType> OnJoystickUp;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnJoystickDown?.Invoke(type);
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnJoystickUp?.Invoke(type);
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    private void OnDestroy()
    {
        OnJoystickDown = null;
        OnJoystickUp = null;
    }
}