using Client;
using UnityEngine;
using Leopotam.EcsLite;
using Statement;
using System;

public class BattlePanel : SourcePanel
{
    [SerializeField] FloatingJoystick movementJoystick; 
    [SerializeField] FloatingJoystick aimJoystick;

    EcsWorld world;
    EcsPool<InputMovementComponent> _movePool = default;
    EcsPool<InputAimComponent> _aimPool = default;
     
    public override void OnOpen(params Action[] onComplete)
    {
        base.OnOpen(onComplete);

        world = BattleState.Instance.EcsHandler.World;

        if (!BattleState.Instance.TryGetEntity("input", out int entity))
        {
            _movePool = world.GetPool<InputMovementComponent>();
            _aimPool = world.GetPool<InputAimComponent>();

            var inputEntity = world.NewEntity();

            world.GetPool<InputComponent>().Add(inputEntity);

            BattleState.Instance.AddEntity("input", inputEntity);

            movementJoystick.OnJoystickDown += OnInputDown;
            movementJoystick.OnJoystickUp += OnInputUp;
            aimJoystick.OnJoystickDown += OnInputDown;
            aimJoystick.OnJoystickUp += OnInputUp; 
        } 
    }

    public override void OnDipose()
    {
        base.OnDipose();

        movementJoystick.OnJoystickDown -= OnInputDown;
        movementJoystick.OnJoystickUp -= OnInputUp;
        aimJoystick.OnJoystickDown -= OnInputDown;
        aimJoystick.OnJoystickUp -= OnInputUp;
    }

    void OnInputDown(InputType type)
    {
        var filter = world.Filter<InputComponent>().End();

        foreach (var inputEntity in filter)
        {
            switch (type)
            {
                case InputType.move:
                    if (!_movePool.Has(inputEntity))
                    {
                        ref var inputMoveComp = ref _movePool.Add(inputEntity);
                        inputMoveComp.MovementJoystick = movementJoystick;
                    }
                    break;
                case InputType.aim:
                    if (!_aimPool.Has(inputEntity))
                    {
                        ref var inputAimComp = ref _aimPool.Add(inputEntity);
                        inputAimComp.AimJoystick = aimJoystick;
                    }
                    break;
            }
        }
    }

    void OnInputUp(InputType type)
    {
        var filter = world.Filter<InputComponent>().End();

        foreach (var inputEntity in filter)
        {
            switch (type)
            {
                case InputType.move:
                    if (_movePool.Has(inputEntity)) _movePool.Del(inputEntity);
                    break;
                case InputType.aim:
                    if (_aimPool.Has(inputEntity)) _aimPool.Del(inputEntity);
                    break;
            }
        }
    }
}
 
public enum InputType { move, aim }