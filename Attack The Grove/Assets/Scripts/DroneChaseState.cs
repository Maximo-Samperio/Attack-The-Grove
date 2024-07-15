using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneChaseState<T> : State<T>
{
    DroneModel _model;
    Transform _target;
    ISteering _pursuit;
    ObstacleAvoidanceV2 _obstacleAvoidanceV2;
    public DroneChaseState(DroneModel model, Transform target, ISteering pursuit, ObstacleAvoidanceV2 obstacleAvoidanceV2)
    {
        _model = model;
        _target = target;
        _pursuit = pursuit;
        _obstacleAvoidanceV2 = obstacleAvoidanceV2;
    }

    public override void Execute()
    {
        Debug.Log("MOVIENDOMEDRON");
        base.Execute();
        Vector3 dir = _obstacleAvoidanceV2.GetDir(_pursuit.GetDir(), false);
        _model.Move(_model.transform.forward);
        _model.LookDir(dir);
    }

}
