using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Background
{
    public class FollowPlayerScript:MonoBehaviour
    {
        // Referencia a la cámara que se va a seguir
        [SerializeField] public GameObject player;
        public bool followY = true;
        public bool mantainInitialDistance = true;
        private Vector2 initalPositionRespectFromPlayer;
        private Vector2 initialPosition;
        void Start()
        {
            initialPosition = transform.position;
            initalPositionRespectFromPlayer = mantainInitialDistance ? initialPosition - (Vector2)player.transform.position : Vector2.zero;
        }

        private void Update()
        {
            AjustToPlayerPosition();
        }

        void LateUpdate()
        {
            AjustToPlayerPosition();
        }

        private void FixedUpdate()
        {
            AjustToPlayerPosition();
        }

        private void AjustToPlayerPosition()
        {
            // Asignar la posición del fondo a la de la cámara (1:1)
            float y = followY ? player.transform.position.y : initialPosition.y;
            Vector2 pos = new(player.transform.position.x, y);
            transform.position = pos + initalPositionRespectFromPlayer;
        }
    }
}
