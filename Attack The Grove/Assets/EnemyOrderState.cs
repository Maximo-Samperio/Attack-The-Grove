using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyOrderState<T> : State<T>
{
    DroneModel[] _drones;

    public EnemyOrderState(DroneModel[] drones)
    {
        _drones = drones;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("AAAAAAAA");
        for (var i = 0; i < _drones.Length; i++)
        {
            _drones[i].AttackOrder();
        }
    }
    public override void Execute()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        base.Execute();
    }
}
