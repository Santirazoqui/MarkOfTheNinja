using Assets.Scripts.Enemy.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemy.States
{
    public class CatatonicState: State
    {
        protected override void EnterImplementation()
        {
            _lastRecivedContext.Pathfinder.AdjustPosition(0, 0);
        }
    }
}
