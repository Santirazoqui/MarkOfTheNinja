using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using UnityEngine.AI;

public class Fireball : MonoBehaviour
{
    [SerializeField] float fireballLifespan = 5;
    Transform target;
    NavMeshAgent agent;
	private bool destroyItself = false;
    LevelManagerController levelManagerController;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var transform = player?.transform;
        if(transform == null)
        {
            Destroy(gameObject);
        }
        target = transform;
    }

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>();
        levelManagerController = FindObjectOfType<LevelManagerController>();
        levelManagerController.LevelWasReset += DestroyItself;
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
		//Hacerlo así evita que el transform rotations llame a un objeto que fue destruido
		if(destroyItself) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.Explode();
            DestroyItself();
        }
    }

    IEnumerator DestroyFireball(float lifespan)
    {
        yield return new WaitForSeconds(lifespan);
        DestroyItself();
    }

    private void DestroyItself()
    {
        destroyItself = true;
    }
}
