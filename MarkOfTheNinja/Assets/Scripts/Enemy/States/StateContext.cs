
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
                            IEnemyAnimationController animation,
                            VisionConeController visionCone,
                            GameObject fireballOrigin,
                            DetectionRateManager detectionRateManager,
                            InstaDetectionHitboxController[] instaDetectedHitboxes)
        {
            Parent = parent;
            Pathfinder = pathfinder;
            Player = player;
            LevelManagerController = levelManager;
            AnimationController = animation;
            VisionCone = visionCone;
            this.FireBallOrigin = fireballOrigin;
            DetectionRateManager = detectionRateManager;
            InstaDetectedHitboxes = instaDetectedHitboxes;
        }

        public VisionConeController VisionCone { get; set; }
        public EnemyController Parent {  get; set; }    
        public Vector2 SoundPosition {  get; set; } 
        public IPathfinder Pathfinder { get; set; }
        public GameObject Player { get; set; }  
        public ILevelManager LevelManagerController { get; set; }
        public IEnemyAnimationController AnimationController { get; set; }
        public float WaitTime { get; set; } 
        public EnemyStates PreviousState { get; set; }
        public float Speed { get; set; }
        public GameObject FireBallOrigin { get; set; }
        public DetectionRateManager DetectionRateManager { get; set; }
        public InstaDetectionHitboxController[] InstaDetectedHitboxes { get; set; }
    }
}
