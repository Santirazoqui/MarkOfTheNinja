using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Assets.Scripts.DataAccess
{
    public class FileDataAccess : IDataAccessManager
    {
        public GameData LoadData()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new();
                using FileStream stream = new(path, FileMode.Open);
                var data =  (GameData)formatter.Deserialize(stream);
                return data;
            }
            return new GameData(); 
        }

        public void SaveData(GameData data)
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(GetPath(), FileMode.Create);
            formatter.Serialize(stream, data);
        }

        private string GetPath()
        {
            return Path.Combine(Application.persistentDataPath, "gamedata.bin");
        }
    }
}
