using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    [SerializeField] float fireballLifespan = 5;
    Transform target;
    NavMeshAgent agent;

    LevelManagerController levelManagerController;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(!player)
        {
            Destroy(gameObject);
        }
        target = player.transform;
    }

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
        levelManagerController = FindObjectOfType<LevelManagerController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {

        agent.SetDestination(target.position);
        StartCoroutine(DestroyFireball(this.fireballLifespan));
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            levelManagerController.PublishEnemyStateChange(EnemyStates.Chilling);
            levelManagerController.PlayerWasCaught();
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyFireball(float lifespan)
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
