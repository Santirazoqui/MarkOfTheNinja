#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.DataAccess
{
    [Serializable]
    public class GameData
    {
        public Score? Score { get; set; }
        public Dictionary<string, int>? HighScores { get; set; }

        public int? GameSceneIndex { get; set; }

        public float? TimeSpentInLevel { get; set; }

        public string? PreviousLevelName { get; set; }

        internal GameDataInternal ToSerializableGameData()
        {
            var res =  new GameDataInternal()
            {
                Score = Score,
                HighScores = HighScores,
                GameSceneIndex = GameSceneIndex,
                TimeSpentInLevel = TimeSpentInLevel,
                PreviousLevelName = PreviousLevelName,
            };

            return res;
        }
    }

    [Serializable]
    internal class GameDataInternal
    {
        public Score? Score { get; set; }
        public Dictionary<string, int>? HighScores { get; set; }

        public int? GameSceneIndex { get; set; }

        public float? TimeSpentInLevel { get; set; }
        public string? PreviousLevelName { get; set; }
        public SerialializableVector? SpawnPoint { get; set; }


        internal GameData ToGameData()
        {
            var res = new GameData()
            {
                Score = Score,
                HighScores = HighScores,
                GameSceneIndex = GameSceneIndex,
                TimeSpentInLevel = TimeSpentInLevel,
                PreviousLevelName = PreviousLevelName,
            };

            return res;
        }
    }

    [Serializable]
    internal class SerialializableVector
    {
        public float X;
        public float Y;
    }
}
