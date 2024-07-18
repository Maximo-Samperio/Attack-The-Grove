using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState<T> : State<T>, IPoints
{
    EnemyModel _model;
    List<Vector3> _waypoints;
    int _nextPoint = 0;
    bool _isFinishPath = true;
    AgentController _controller;
    public EnemyPatrolState(EnemyModel model, AgentController agentController)
    {
        _model = model;
        _controller = agentController;
    }
    public override void Enter()
    {
        base.Enter();
        SetPath();
    }
    public override void Execute()
    {
        Debug.Log("PATROLING");
        base.Execute();
        Run();
    }
    public override void Sleep()
    {
        base.Sleep();
    }
    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        SetWayPoints(list);
    }
    public void SetWayPoints(List<Vector3> newPoints)
    {
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _model.transform.position.y;
        _model.SetPosition(pos);
        _isFinishPath = false;
    }
    void Run()
    {
        if (IsFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _model.transform.position.y;
        Vector3 dir = posPoint - _model.transform.position;
        if (dir.magnitude < 0.2f)
        {
            Debug.Log("aaa");
            if (_nextPoint + 1 < _waypoints.Count)
                _nextPoint++;
            else
            {
                _isFinishPath = true;
                SetPath();
                return;
            }
        }
        _model.Move(dir.normalized);
        _model.LookDir(dir);
    }
    public void SetPath()
    {
        SetNewPathWithRoulette();
    }
    public bool IsFinishPath => _isFinishPath;

    private void SetNewPathWithRoulette()
    {
        List<float> probabilities = CalculateProbabilities();
        int selectedIndex = RouletteWheelSelection(probabilities);

        if (selectedIndex != -1)
        {
            Debug.Log("Selected new waypoint: " + selectedIndex);
            Vector3 newDestination = _waypoints[selectedIndex];
            var path = AStarPathfinding(_model.transform.position, newDestination);
            SetWayPoints(path);
        }
        else
        {
            Debug.LogError("No waypoints available for selection");
        }
    }

    private List<float> CalculateProbabilities()
    {
        List<float> distances = new List<float>();

        // Calculate distances from the agent to each waypoint
        foreach (Vector3 waypoint in _waypoints)
        {
            float distance = Vector3.Distance(_model.transform.position, waypoint);
            distances.Add(distance);
        }

        List<float> probabilities = new List<float>();
        float totalDistance = distances.Sum();

        foreach (float distance in distances)
        {
            float probability = 1f - (distance / totalDistance);
            probabilities.Add(probability);
        }

        return probabilities;
    }

    private int RouletteWheelSelection(List<float> probabilities)
    {
        float totalProbability = probabilities.Sum();
        float randomValue = UnityEngine.Random.value * totalProbability; // Generate a random number between 0 and the sum of probabilities

        // Make selection based on probabilities
        float cumulativeProbability = 0f;
        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        // If no waypoint can be selected (should not happen if probabilities are correctly calculated)
        Debug.LogError("No waypoints available for selection");
        return -1;
    }

    private List<Node> AStarPathfinding(Vector3 start, Vector3 goal)
    {
        return _controller.RunAStar2(start, goal);
    }

    //private List<Vector3> GetConnections(Vector3 node)
    //{
    //    return _waypoints;
    //}
}
