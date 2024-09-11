
using UnityEngine;
using Assets.Scripts.Enemy.Pathfinding;

namespace Assets.Scripts.Enemy.States
{
    public class StateContext
    {
        public StateContext(EnemyController parent, 
                            Pathfinder pathfinder, 
                            GameObject player)
        {
            Parent = parent;
            Pathfinder = pathfinder;
            Player = player;
        }

        public EnemyController Parent {  get; set; }    
        public Vector2 SoundPosition {  get; set; } 
        public Pathfinder Pathfinder { get; set; }
        public GameObject Player { get; set; }  
    }
}
