using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemy.Pathfinding;

namespace Assets.Scripts.Enemy.States
{
    public class StateContext
    {
        public StateContext(EnemyController parent)
        {
            Parent = parent;
        }

        public EnemyController Parent {  get; set; }    
        public Vector2 SoundPosition {  get; set; } 
        public Pathfinder Pathfinder { get; set; }
    }
}
