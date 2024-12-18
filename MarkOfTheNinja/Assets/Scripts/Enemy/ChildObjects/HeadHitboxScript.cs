  using System.Collections;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Enemy.ChildObjects
{
    public class HeadHitboxScript : MonoBehaviour
    {
        private bool playerIsOnTop;
        private EnemyController controller;
        // Use this for initialization
        void Start()
        {
            controller = GetComponentInParent<EnemyController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Util.Util.CollidedWithPlayer(collision)) return;
            controller.ChangeStates(EnemyStates.Stunned);
        }
    }
}