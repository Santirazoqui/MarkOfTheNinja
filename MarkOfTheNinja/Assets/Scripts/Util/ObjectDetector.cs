using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class ObjectDetector
    {
        private static readonly string _wallsLayer = "Walls";
        private static readonly string _groundLayer = "Ground";
        public static bool AnyObjectsBetween(GameObject you, GameObject them)
        {
            Vector2 origin = you.transform.position;
            Vector2 goal = them.transform.position;
            return AnyObjectsBetweenWithLayer(origin, goal, _wallsLayer) || AnyObjectsBetweenWithLayer(origin,goal,_groundLayer);
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
