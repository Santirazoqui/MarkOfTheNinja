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
        public StateContext(EnemyController parent, 
                            Pathfinder pathfinder, 
                            GameObject player,
                            ChillingSettings chillingSettings,
                            SearchingSettings searchingSettings,
                            DetectedSettings detectedSettings)
        {
            Parent = parent;
            Pathfinder = pathfinder;
            Player = player;
            this.chillingSettings = chillingSettings;
            this.searchingSettings = searchingSettings;
            this.detectedSettings = detectedSettings;
        }

        public ChillingSettings chillingSettings { get; set; }
        public SearchingSettings searchingSettings { get; set; }
        public DetectedSettings detectedSettings { get; set; }
        public EnemyController Parent {  get; set; }    
        public Vector2 SoundPosition {  get; set; } 
        public Pathfinder Pathfinder { get; set; }
        public GameObject Player { get; set; }  
    }
}
