using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class PointGeneratorScript:MonoBehaviour
    {
        public GameObject pointPrefab;

        public void DrawPoint(Vector2 position, Color color)
        {
            var point = Instantiate(pointPrefab, position, Quaternion.identity);
            point.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
