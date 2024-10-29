using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemy.States.Detected
{
    public class DetectedHunt:DetectedStateV2
    {
        public float persectutionSpeed = 400f;
        protected override void FixedDoImplementation()
        {
            base.FixedDoImplementation();
            if(!CanSeePlayer())
            {
                parent.ChangeStates(EnemyStates.DetectedPatrolling);
            }
            else
            {
                Hunt();
            }
        }

        private void Hunt()
        {
            pathfinder.SetDestination(player.transform.position, () => { });
            var speed = persectutionSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }
    }
}
