  using System.Collections;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Enemy.ChildObjects
{
    public class HeadHitboxScript : MonoBehaviour
    {
        public float secondsOfSlideHitbox;
        private bool playerIsOnTop;
        private EnemyController controller;
        private BoxCollider2D boxCollider;
        private GameObject BouncePlayerOff;
        private readonly string bouncePlayerOffPrefabName = "BounceOffMeHitbox";
        // Use this for initialization
        void Start()
        {
            controller = GetComponentInParent<EnemyController>();
            boxCollider = GetComponent<BoxCollider2D>();
            BouncePlayerOff = transform.Find(bouncePlayerOffPrefabName).gameObject;
            BouncePlayerOff.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Util.Util.CollidedWithPlayer(collision) || !boxCollider.enabled) return;
            StopAllCoroutines();
            controller.ChangeStates(EnemyStates.Stunned);
            StartCoroutine(SlideOff());
        }

        private IEnumerator SlideOff()
        {
            boxCollider.enabled = false;
            BouncePlayerOff.SetActive(true);
            yield return new WaitForSeconds(secondsOfSlideHitbox);
            boxCollider.enabled = true;
            BouncePlayerOff.SetActive(false);
        }
    }
}