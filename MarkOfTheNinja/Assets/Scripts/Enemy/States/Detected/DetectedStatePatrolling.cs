using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States.Detected
{
    public class DetectedStatePatrolling:State
    {

        [SerializeField] GameObject fireball;

        public float patrollingSpeed = 300f;
        public float persectutionSpeed = 400f;
        public float searchingRadius = 500f;
        public float minHeightDifferenceToThrowFireballs = 10f;
        public string playerTag = "Player";
        public string animationEventForKillingPlayer = "playerKilled";
        public string animationEventForThrowingFireball = "thowingFireballEnded";
        private readonly string floorsLayers = "Ground";
        private readonly string platformsLayer = "Walls";
        private readonly string enemyWallLayer = "EnemyWall";
        private readonly string camaraTag = "MainCamera";

        private bool cantMove = false;
        private bool killing = false;
        private Vector2 initialPosition;
        private Rigidbody2D rb;
        private readonly float[] searchingLimits = new float[2];
        private int searchingIndex = 0;

        private bool initiated = false;

        // Si en vez de poner el codigo en esta funcion, se pone en el start, tira null pointer exceptions 
        private void FakeStart()
        {
            if (initiated) return;

            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;

            rb = parent.GetComponent<Rigidbody2D>();
            initialPosition = rb.position;
            initiated = true;
            animationController = _lastRecivedContext.AnimationController;
        }

        protected override void EnterImplementation()
        {
            DeactivateVisionCone();
            PlayDetectedAnimation();
            FakeStart();
            UpdateSearchRadius(_lastRecivedContext);
            StartSearch();
        }

        protected override void DoImplementation()
        {
            UpdateSearchRadius(_lastRecivedContext);
        }

        protected override void FixedDoImplementation()
        {
            if (cantMove)
            {
                pathfinder.AdjustPosition(0, 0);
                return;
            }
            if (CanSeePlayer())
            {
                if(CantReachPlayer())
                {
                    ThrowFireballs();
                }
                else
                {
                    Hunt();
                }
            }
            else
            {
                StartSearch();
                Patroll();
            }
        }

        public override void CollitionEnter(Collision2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }

        public override void AnimationEventFired(string eventDescription)
        {
            if (eventDescription == animationEventForKillingPlayer)
            {
                PostKilling();
            }
            else if (eventDescription == animationEventForThrowingFireball)
            {
                cantMove = false;
                PlayDetectedAnimation();
            }

        }

        private void ThrowFireballs()
        {
            animationController.ThrowFireball();
            Instantiate(fireball, rb.position, Quaternion.identity);
            StartCoroutine(WaitForFireball());
        }

        IEnumerator WaitForFireball()
        {
            cantMove = true;
            yield return new WaitForSeconds(5); //Encapsule variable
            cantMove = false;
        }



        private void Hunt()
        {
            pathfinder.SetDestination(player.transform.position, () => { });
            var speed = persectutionSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }


        private void Patroll()
        {
            var speed = patrollingSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }


        private void UpdateSearchRadius(StateContext context)
        {
            var x = initialPosition.x;
            searchingLimits[0] = x - searchingRadius;
            searchingLimits[1] = x + searchingRadius;

        }

        private void StartSearch()
        {
            var target = new Vector2(searchingLimits[searchingIndex], rb.position.y);
            pathfinder.SetDestination(target, SwitchTargets);
        }

        private void SwitchTargets()
        {
            if (searchingLimits.Length - 1 == searchingIndex)
            {
                searchingIndex = 0;
            }
            else
            {
                searchingIndex++;
            }
            StartSearch();
        }

        //AVISO:
        //No es solo por coherencia visual. Si caes de cierta manera de arriba al guardia y te detecta al instante, pasaba un bug que despues de
        //matado al personaje el guardia se quedaba atascado en el estado confused porque, de alguna manera, el cono de vision detectaba al pibe
        //pos mortem
        private void DeactivateVisionCone()
        {
            visionCone.gameObject.SetActive(false);
        }


        private void HandlePlayerCollition(GameObject player)
        {
            bool collidedWithPlayer = player.CompareTag(playerTag);
            if (!collidedWithPlayer) return;
            KillPlayer(player);
        }

        private void KillPlayer(GameObject player)
        {
            if (killing) return;
            killing = true;
            Debug.Log("Player was killed");
            animationController.Killing();
            cantMove = true;
            player.SetActive(false);
            levelManagerController.PlayerWasCaught();
        }



        private void PostKilling()
        {
            Debug.Log("Post killing fired");
            levelManagerController.PublishEnemyStateChange(EnemyStates.Chilling);
        }


        private void PlayDetectedAnimation()
        {
            animationController.Walking();
        }


        private bool CanSeePlayer()
        {
            var camara = GameObject.FindGameObjectWithTag(camaraTag).GetComponent<Camera>();
            Vector2 viewportPosition = camara.WorldToViewportPoint(transform.position);
            bool isVisible = viewportPosition.x > 0 && viewportPosition.x < 1 && // Dentro del eje X del viewport
                            viewportPosition.y > 0 && viewportPosition.y < 1;   // Dentro del eje Y del viewport
            if (!isVisible) return false; // para no hacer raytracing si esta fuera de camara
            var collider = player.GetComponent<Collider2D>();
            bool areInTheSameFloor = !ObjectDetector.AnyObjectsBetween(parent.gameObject, collider, new string[] { floorsLayers });
            return areInTheSameFloor;
        }

        private bool CantReachPlayer()
        {
            var collider = player.GetComponent<Collider2D>();
            bool inAPlatformOrOutOfBounds = ObjectDetector.AnyObjectsBetween(parent.gameObject, collider, new string[] { platformsLayer, enemyWallLayer });
            return inAPlatformOrOutOfBounds;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }


    }
}
