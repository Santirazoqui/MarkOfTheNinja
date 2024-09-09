using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class SoundInstanceController:MonoBehaviour
    {
        private readonly string _spriteName = "Circle";
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
        private Transform sprite;
        private float offset;
        private void Start()
        {
            InitiateGlobals();
        }

        private void UpdateRadius(float value)
        {
            InitiateGlobals();
            _radius = value;
            circleCollider.radius = _radius;
            sprite.localScale = _radius * offset * Vector2.one;
        }

        private void InitiateGlobals()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            sprite = transform.Find(_spriteName);
            _radius = circleCollider.radius;
            offset = sprite.localScale.x / _radius;
        }
    }
}
