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
        public GameData LoadData()
        {
            var data =  new GameData() { 
                HighScore = PlayerPrefs.GetInt(HighScoreKey,-1),
                Score = PlayerPrefs.GetInt(ScoreKey,-1),
                GameSceneIndex = PlayerPrefs.GetInt(GameIndexKey,-1),
            };
            data.HighScore = data.HighScore == -1 ? null: data.HighScore;
            data.Score = data.Score == -1 ? null : data.Score;
            data.GameSceneIndex = data.GameSceneIndex == -1 ? null : data.GameSceneIndex;
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
            PlayerPrefs.Save();
        }
    }
}
