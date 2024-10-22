using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DataAccess
{
    public interface IDataAccessManager
    {
        GameData LoadData();
        void SaveData(GameData data);   
    }
}
