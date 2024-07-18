using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AgentController : MonoBehaviour
{
    public EnemyController _enemy;
    public float radius = 3;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    public Box box;
    public Node target;

    public void RunAStar()
    {
        var start = GetNearNode(_enemy.transform.position);
        if (start == null) return;
        List<Node> path = AStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic);
        _enemy.GetStateWaypoints.SetWayPoints(path);
        box.SetWayPoints(path);
    }
    public List<Node> RunAStar2(Vector3 startPos, Vector3 targetPos)
    {
        var start = GetNearNode(startPos);
        var end = GetNearNode(targetPos);
        if (start == null || end == null) return new List<Node>();
        return AStar.Run(start, GetConnections, (x)=>IsSatiesfies(x,end), GetCost, (x)=> Heuristic(x,end));
    }
    public List<Node> RunAStar2(Vector3 startPos, Func<Node, bool> isSatisfies, Func<Node, float> heuristic)
    {
        var start = GetNearNode(startPos);
        if (start == null) return new List<Node>();
        return AStar.Run(start, GetConnections, isSatisfies, GetCost, heuristic);
    }
    float Heuristic(Node current)
    {
        float heuristic = 0;
        float multiplierDistance = 1;
        heuristic += Vector3.Distance(current.transform.position, target.transform.position) * multiplierDistance;
        return heuristic;
    }
    float Heuristic(Node current, Node end)
    {
        float heuristic = 0;
        float multiplierDistance = 1;
        heuristic += Vector3.Distance(current.transform.position, end.transform.position) * multiplierDistance;
        return heuristic;
    }
    float GetCost(Node parent, Node child)
    {
        float cost = 0;
        float multiplierDistance = 1;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * multiplierDistance;
        return cost;
    }
    Node GetNearNode(Vector3 pos)
    {
        var nodes = Physics.OverlapSphere(pos, radius, maskNodes);
        Node nearNode = null;
        float nearDistance = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            var currentNode = nodes[i];
            var dir = currentNode.transform.position - pos;
            float currentDistance = dir.magnitude;
            if (nearNode == null || currentDistance < nearDistance)
            {
                if (!Physics.Raycast(pos, dir.normalized, currentDistance, maskObs))
                {
                    nearNode = currentNode.GetComponent<Node>();
                    nearDistance = currentDistance;
                }
            }
        }
        return nearNode;
    }
    List<Node> GetConnections(Node current)
    {
  
        return current.neighbours;
    }
    bool IsSatiesfies(Node current)
    {
        //overlap sphere 
        // 5
        //
        return current == target;
    }
    bool IsSatiesfies(Node current, Node end)
    {
        return current == end;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(_enemy.transform.position, radius);
    }

    public Node FindFurthestNode(Vector3 startPos)
    {
        Node startNode = GetNearNode(startPos);
        if (startNode == null) return null;

        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, int> distances = new Dictionary<Node, int>();
        Node furthestNode = startNode;
        int maxDistance = 0;

        queue.Enqueue(startNode);
        distances[startNode] = 0;

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            int currentDistance = distances[current];

            foreach (Node neighbor in current.neighbours)
            {
                if (!distances.ContainsKey(neighbor))
                {
                    distances[neighbor] = currentDistance + 1;
                    queue.Enqueue(neighbor);

                    if (distances[neighbor] > maxDistance)
                    {
                        maxDistance = distances[neighbor];
                        furthestNode = neighbor;
                    }
                }
            }
        }

        return furthestNode;
    }

}
