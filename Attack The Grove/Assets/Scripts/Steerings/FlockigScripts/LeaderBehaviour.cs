using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier;
    public Transform target;
    public bool isActive = true;
    Seek _seek;
    Pursuit _pursuit;
    ISteering _steering;

    private void Awake()
    {
        _seek = new Seek();
        _pursuit = new Pursuit();
    }
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        //if (isActive)
        //    return (target.position - self.Position).normalized * multiplier;
        //return Vector3.zero;
        return _steering.GetDir() * multiplier;

    }
    public void SetTarget(Transform target)
    {
        this.target = target;
        var rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {

            _steering = _pursuit;
        }
        else
        {
            _seek.Target = target;
            _steering = _seek;
        }
    }
}
