using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class ObjectDetector
    {
        private static readonly string _wallsLayer = "Walls";
        private static readonly string _groundLayer = "Ground";
        public static bool AnyObjectsBetween(GameObject you, Collider2D collider)
        {
            Vector2 origin = you.transform.position;

            // Obtener los puntos importantes de la hitbox (superior, centro, e inferior)
            Vector2 top = collider.bounds.max; // Parte superior
            Vector2 bottom = collider.bounds.min; // Parte inferior
            Vector2 center = collider.bounds.center; // Centro

            // Hacer raycasts a los tres puntos
            return (AnyObjectsBetweenWithLayer(origin, top, _wallsLayer) &&
                   AnyObjectsBetweenWithLayer(origin, center, _wallsLayer) &&
                   AnyObjectsBetweenWithLayer(origin, bottom, _wallsLayer)) ||
                   (AnyObjectsBetweenWithLayer(origin, top, _groundLayer) &&
                   AnyObjectsBetweenWithLayer(origin, center, _groundLayer) &&
                   AnyObjectsBetweenWithLayer(origin, bottom, _groundLayer));
        }

        public static bool AnyObjectsBetween(GameObject you, Vector2 goal)
        {
            Vector2 origin = you.transform.position;
            return AnyObjectsBetweenWithLayer(origin, goal, _wallsLayer) || AnyObjectsBetweenWithLayer(origin, goal, _groundLayer);
        }

        private static bool AnyObjectsBetweenWithLayer(Vector2 origin, Vector2 goal, string layer)
        {
            Vector2 direction = goal - origin;
            float distance = direction.magnitude;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance: distance, layerMask: LayerMask.GetMask(layer));
            if (hit.collider == null) return false;
            bool weHitThem = hit.point == goal;
            bool weHitSomethingBehindThem = hit.distance >= distance;
            return !(weHitThem || weHitSomethingBehindThem);
        }


    }
}
