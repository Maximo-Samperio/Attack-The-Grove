using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbours;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var neighbour in neighbours)
        {
            Gizmos.DrawLine(transform.position, neighbour.transform.position);
        }
    }

}
