using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class SoundInstanceController:MonoBehaviour
    {
        public float maxRadius=10f;
        public float lifeExpentancy = 2f;
        public int amountOfIncrements = 200;
        public bool Moving { get; set; } = false;
        public Action DestructionCallback { get; set; }
        private float _radius;
        public float Radius 
        { 
            get => this._radius; 
            set 
            {
                UpdateRadius(value);
            } 
        }

        private CircleCollider2D circleCollider;
        public int segments = 100;       
        private LineRenderer lineRenderer;
        private void Start()
        {
            InitiateGlobals();
            StartCoroutine(SoundWave());
        }

        private void UpdateRadius(float value)
        {
            InitiateGlobals();
            _radius = value;
            circleCollider.radius = _radius;
            DrawCircleShape();
            
        }

        private void InitiateGlobals()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = segments + 1; // Configurar el número de puntos
            lineRenderer.useWorldSpace = false;
            _radius = circleCollider.radius;
        }

        private void DrawCircleShape()
        {
            float angle = 0f;
            for (int i = 0; i < segments + 1; i++)
            {
                float x = Mathf.Cos(angle) * Radius;
                float y = Mathf.Sin(angle) * Radius;
                lineRenderer.SetPosition(i, new Vector2(x, y));
                angle += 2 * Mathf.PI / segments;
            }
        }

        private IEnumerator SoundWave()
        {
            float waitTime = lifeExpentancy / amountOfIncrements;
            float increments = maxRadius / amountOfIncrements;
            for (int i = 1; Radius <= maxRadius; i++)
            {
                Radius = increments * i;
                yield return new WaitForSeconds(waitTime);
            }
            while(Moving)
            {
                yield return new WaitForEndOfFrame();
            }
            DestructionCallback?.Invoke();
            Destroy(gameObject);
            yield break;
        }
    }
}
