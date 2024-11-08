using Assets.Scripts.Enemy;
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
            text.text = $"Time: {Util.Util.ChangeCommaToPoint(time + "")}s";
        }


    }
}
