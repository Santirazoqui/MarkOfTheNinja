
using UnityEngine;
using Assets.Scripts.Enemy.Pathfinding;

namespace Assets.Scripts.Enemy.States
{
    public class StateContext
    {
        public StateContext(EnemyController parent, 
                            Pathfinder pathfinder, 
                            GameObject player,
                            LevelManagerController levelManager,
                            EnemyAnimationController animation,
                            VisionConeController visionCone)
        {
            Parent = parent;
            Pathfinder = pathfinder;
            Player = player;
            LevelManagerController = levelManager;
            AnimationController = animation;
            VisionCone = visionCone;
        }

        public VisionConeController VisionCone { get; set; }
        public EnemyController Parent {  get; set; }    
        public Vector2 SoundPosition {  get; set; } 
        public IPathfinder Pathfinder { get; set; }
        public GameObject Player { get; set; }  
        public ILevelManager LevelManagerController { get; set; }
        public EnemyAnimationController AnimationController { get; set; }
        public (float, EnemyStates) WaitTime { get; set; } 
        public float Speed { get; set; }
    }
}
