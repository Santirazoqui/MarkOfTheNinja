using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DataAccess
{
    public class DataAccessManager : IDataAccessManager
    {
        private readonly string HighScoreKey = "HighScore";
        private readonly string ScoreKey = "CurrentScore";
        private readonly string GameIndexKey = "GameIndex";
        private readonly string TimeSpentKey = "Time spent";
        public GameData LoadData()
        {
            var data =  new GameData() { 
                HighScore = PlayerPrefs.GetInt(HighScoreKey,-1),
                Score = PlayerPrefs.GetInt(ScoreKey,-1),
                GameSceneIndex = PlayerPrefs.GetInt(GameIndexKey,-1),
                TimeSpentInLevel = PlayerPrefs.GetFloat(TimeSpentKey,-1),
            };
            data.HighScore = data.HighScore == -1 ? null: data.HighScore;
            data.Score = data.Score == -1 ? null : data.Score;
            data.GameSceneIndex = data.GameSceneIndex == -1 ? null : data.GameSceneIndex;
            data.TimeSpentInLevel = data.TimeSpentInLevel == -1 ? null : data.TimeSpentInLevel;
            return data;
        }

        public void SaveData(GameData data)
        {
            var previusGameData = LoadData();
            if (data.Score != null)
            {
                if (previusGameData.HighScore < data.Score) PlayerPrefs.SetInt(HighScoreKey, (int)data.Score);
                PlayerPrefs.SetInt(ScoreKey, (int)data.Score);
            }
            if (data.GameSceneIndex != null) PlayerPrefs.SetInt(GameIndexKey, (int)data.GameSceneIndex);
            if (data.TimeSpentInLevel != null) PlayerPrefs.SetFloat(TimeSpentKey, (float)data.TimeSpentInLevel);
            PlayerPrefs.Save();
        }
    }
}
