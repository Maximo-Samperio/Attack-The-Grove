using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeState<T> : State<T>
{
    EnemyModel _model;
    List<Node> _waypoints;
    ObstacleAvoidanceV2 _obstacleAvoidance;
    int _currentWaypointIndex = 0;
    bool _isFinishedPath = false;
    float _reachThreshold = 1f; // Adjust this threshold as needed

    public EnemyFleeState(EnemyModel model, List<Node> waypoints, ObstacleAvoidanceV2 obstacleAvoidance)
    {
        _model = model;
        _waypoints = waypoints;
        _obstacleAvoidance = obstacleAvoidance;
    }

    public override void Execute()
    {
        base.Execute();

        if (_isFinishedPath)
        {
            FindNextWaypoint();
            _isFinishedPath = false;
        }

        Vector3 targetPosition = _waypoints[_currentWaypointIndex].transform.position;
        Vector3 direction = targetPosition - _model.transform.position;

        if (direction.magnitude < _reachThreshold) // Use a threshold to determine if reached
        {
            _isFinishedPath = true;
            return; // Stop moving when reaching the waypoint
        }

        Vector3 avoidanceDirection = _obstacleAvoidance.GetDir(direction, false);
        // Reduce speed to avoid erratic movement
        _model.Move(avoidanceDirection.normalized * 1f);
        _model.LookDir(avoidanceDirection);
    }

    private void FindNextWaypoint()
    {
        if (_waypoints.Count == 0) return;

        _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
    }
}
