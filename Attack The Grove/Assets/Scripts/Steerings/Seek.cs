using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    Transform _entity;
    Transform _target;

    public Rigidbody rb;

    public Seek(Transform entity, Transform target)
    {
        _entity = entity;
        _target = target;
    }
    public Seek(Transform entity)
    {
        _entity = entity;
    }
    public Seek() { }

    public Vector3 GetDir()
    {
        //a: entity;
        //b: _target;
        if(_target == null) return Vector3.zero;
        return (_target.position - _entity.transform.position).normalized;
    }
 
    public Transform Target { set { _target = value; } }
}
