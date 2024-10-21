using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DataAccess
{
    [Serializable]
    public class GameData
    {
        public int? Score { get; set; }
        public int? HighScore { get; set; }

        public int? GameSceneIndex { get; set; }
    }
}
