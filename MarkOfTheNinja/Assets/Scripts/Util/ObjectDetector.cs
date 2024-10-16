using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class ObjectDetector
    {
        private static readonly string _wallsLayer = "Walls";
        private static readonly string _groundLayer = "Ground";
        public static bool AnyObjectsBetween(GameObject you, GameObject them)
        {
            return AnyObjectsBetweenWithLayer(you, them, _wallsLayer) || AnyObjectsBetweenWithLayer(you,them,_groundLayer);
        }

        private static bool AnyObjectsBetweenWithLayer(GameObject you, GameObject them, string layer)
        {
            Vector2 origin = you.transform.position;
            Vector2 goal = them.transform.position;
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
