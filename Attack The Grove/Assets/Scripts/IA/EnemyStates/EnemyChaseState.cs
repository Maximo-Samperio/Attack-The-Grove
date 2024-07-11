using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChaseState<T> : State<T>
{
    ISteering _pursuit;
    EnemyModel _model;
    ObstacleAvoidanceV2 _obs;

    public EnemyChaseState(EnemyModel model, ISteering pursuit, ObstacleAvoidanceV2 obs)
    {
        _pursuit = pursuit;
        _model = model;
        _obs = obs;
    }
    public override void Execute()
    {
        Debug.Log("CHASING");
        var dir = _obs.GetDir(_pursuit.GetDir(), false);
        _model.Move(_model.transform.forward);
        _model.LookDir(dir);
    }

}
