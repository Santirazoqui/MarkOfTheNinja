using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(target.position);
        //transform.LookAt(target, Vector3.forward);

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Vector3 relativePos = target.position - transform.position;
        // transform.rotation = Quaternion.LookRotation(relativePos, Vector2.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
