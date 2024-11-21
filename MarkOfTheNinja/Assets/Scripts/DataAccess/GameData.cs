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
        public int? HighScore { get; set; }

        public int? GameSceneIndex { get; set; }

        public float? TimeSpentInLevel { get; set; }
        public Vector2? SpawnPoint { get; set; }

        internal GameDataInternal ToSerializableGameData()
        {
            var res =  new GameDataInternal()
            {
                Score = Score,
                HighScore = HighScore,
                GameSceneIndex = GameSceneIndex,
                TimeSpentInLevel = TimeSpentInLevel,
            };
            if (SpawnPoint.HasValue)
            {
                var value = SpawnPoint.Value;
                res.SpawnPoint = new SerialializableVector() { X = value.x, Y = value.y };
            }

            return res;
        }
    }

    [Serializable]
    internal class GameDataInternal
    {
        public Score? Score { get; set; }
        public int? HighScore { get; set; }

        public int? GameSceneIndex { get; set; }

        public float? TimeSpentInLevel { get; set; }
        public SerialializableVector? SpawnPoint { get; set; }

        internal GameData ToGameData()
        {
            var res = new GameData()
            {
                Score = Score,
                HighScore = HighScore,
                GameSceneIndex = GameSceneIndex,
                TimeSpentInLevel = TimeSpentInLevel,
            };
            if (SpawnPoint!=null)
            {
                res.SpawnPoint = new Vector2(SpawnPoint.X, SpawnPoint.Y);
            }

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
