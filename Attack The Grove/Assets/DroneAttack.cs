using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackState<T> : State<T>
{
    DroneModel _model;

    public DroneAttackState(DroneModel model)
    {
        _model = model;
    }

    public override void Execute()
    {
        base.Execute();
        _model.Attack();
    }
}
