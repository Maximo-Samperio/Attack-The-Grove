using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState<T>: State<T>
{
    EnemyModel _model;

    public EnemyDeathState(EnemyModel model)
    {
        _model = model;
    }

    public void Enter()
    {
        base.Enter();
        _model.Death();
    }



}
