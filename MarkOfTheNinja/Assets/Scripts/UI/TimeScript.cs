using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TimeScript:MonoBehaviour
    {
        private LevelManagerController levelManagerController;
        private TextMeshProUGUI text;
        void Start()
        {
            levelManagerController = FindAnyObjectByType<LevelManagerController>();
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            var time = Math.Round(levelManagerController.TimeSpentInLevel, 2);
            text.text = $"Time: {ChangeCommaToPoint(time + "")}s";
        }

        private string ChangeCommaToPoint(string time)
        {
            var res = "";
            for(int i=0; i<time.Length; i++)
            {
                if (time[i] == ',')
                {
                    res += '.';
                }
                else
                {
                    res += time[i];
                }
            }
            return res;
        }
    }
}
