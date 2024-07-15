using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DroneModel : MonoBehaviour, IBoid
{
    [SerializeField] private float speed;
    [SerializeField] private float speedRot;
    Rigidbody _rb;

    public bool fight;

    public GameObject Jagger;
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;

    public float health;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = _rb.velocity.y;
        _rb.velocity = dir;
    }

    public void Attack()
    {
        StartCoroutine(Cooldown());
    }
    IEnumerator Cooldown()
    {
        Jagger.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Jagger.SetActive(false);

    }
    public void Death()
    {
        Destroy(gameObject);
    }

    public void AttackOrder()
    {
        fight = true;
        Debug.Log("orderDrone");
    }

    public void LookDir(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == 0) return;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * speedRot);

    }
}
