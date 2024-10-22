using System;

namespace Assets.Scripts.DataAccess
{
    [Serializable]

    public class Score
    {
        public int RawPoints { get; set; } 
        public int TimeBonus {  get; set; }

        public int Total {  get; set; }
    }

}
