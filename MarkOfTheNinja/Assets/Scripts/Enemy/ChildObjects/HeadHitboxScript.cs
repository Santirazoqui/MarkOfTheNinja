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
        private GameObject slidePlayerOff;
        private readonly string slidePlayerOffPrefabName = "SlideOffMeHitbox";
        // Use this for initialization
        void Start()
        {
            controller = GetComponentInParent<EnemyController>();
            boxCollider = GetComponent<BoxCollider2D>();
            slidePlayerOff = transform.Find(slidePlayerOffPrefabName).gameObject;
            slidePlayerOff.SetActive(false);
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
            slidePlayerOff.SetActive(true);
            yield return new WaitForSeconds(secondsOfSlideHitbox);
            boxCollider.enabled = true;
            slidePlayerOff.SetActive(false);
        }
    }
}