using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    internal static class Util
    {
        private static readonly string _soundTag = "Sound";
        private static readonly string _playerTag = "Player";
        public static bool CollidedWithSound(GameObject you, Collider2D collision)
        {
            bool collidedWithASound = collision.gameObject.CompareTag(_soundTag);
            if (!collidedWithASound) return false;
            if (ObjectDetector.AnyObjectsBetween(you, collision.gameObject)) return false;
            return true;
        }

        public static bool CollidedWithPlayer(Collider2D collision)
        {
            return collision.gameObject.CompareTag(_playerTag);
        }
    }
}
