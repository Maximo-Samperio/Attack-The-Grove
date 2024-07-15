using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDeathState<T>: State<T>
{
    DroneModel _model;

    public DroneDeathState(DroneModel model)
    {
        _model = model;
    }

    public override void Enter()
    {
        base.Enter();
        _model.Death();
    }



}
