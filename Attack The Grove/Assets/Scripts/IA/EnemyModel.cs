using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    [SerializeField] private float speed;
    Rigidbody _rb;

    public Node[] waypoints; // Array to hold the patrol waypoints
    public Node currentWayPoint;
    public int currentWaypointIndex = 0; // Index of the current waypoint
    public int index;
    public GameObject Jagger;
    public float health;
    public DroneModel[] drones;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        // Ensure waypoints are assigned
        GameObject nodesParent = GameObject.Find("Nodes");
        if (nodesParent != null)
        {
            waypoints = nodesParent.GetComponentsInChildren<Node>();
        }
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;

        transform.forward = dir;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Attack()
    {
        StartCoroutine(Cooldown());
    }

    public void Death()
    {
        Debug.Log("death");
        Destroy(gameObject);
    }

    IEnumerator Cooldown()
    {
        Jagger.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Jagger.SetActive(false);
    }
}