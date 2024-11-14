using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class ObjectDetector
    {
        private static readonly string _wallsLayer = "Walls";
        private static readonly string _groundLayer = "Ground";
        private static readonly string[] _defaultLayers = new string[] { _groundLayer };
        public static bool AnyObjectsBetween(GameObject you, Collider2D collider, string[] layers = null)
        {
            layers ??= _defaultLayers;
            Vector2 origin = you.transform.position;

            // Obtener los puntos importantes de la hitbox (superior, centro, e inferior)
            Vector2 top = collider.bounds.max; // Parte superior
            Vector2 bottom = collider.bounds.min; // Parte inferior
            Vector2 center = collider.bounds.center; // Centro

            // Hacer raycasts a los tres puntos
            return AnyObjectsBetween(origin, top, layers) &&
                    AnyObjectsBetween(origin, center, layers) &&
                    AnyObjectsBetween(origin, bottom, layers); 
        }

        public static bool AnyObjectsBetween(Vector2 origin, Vector2 goal, string[] layers=null)
        {
            layers ??= _defaultLayers;
            Vector2 direction = goal - origin;
            float distance = direction.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance: distance, layerMask: LayerMask.GetMask(layers));
            if (hit.collider == null) { return false; }

            bool weHitThem = hit.point == goal;
            bool weHitSomethingBehindThem = hit.distance >= distance;
            return !(weHitThem || weHitSomethingBehindThem);
        }


    }
}
