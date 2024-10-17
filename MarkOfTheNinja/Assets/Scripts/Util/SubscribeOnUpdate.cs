using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Util
{
    public class SubscribeOnUpdate:MonoBehaviour
    {
        private Action actions;

        private void Update()
        {
            actions?.Invoke();
        }

        /// <summary>
        /// Subscribe una funcion para que se ejecute en el update
        /// </summary>
        /// <param name="action">Funcion a encolarse</param>
        protected void SubscribeToOnUpdate(Action action)
        {
            actions += action;
        }
        /// <summary>
        /// Desencola una funcion para que no se vuelva a ejecutar en el update
        /// </summary>
        /// <param name="action">Funcion a desencolarse</param>
        protected void UnsubscribeOfOnUpdate(Action action)
        {
            actions -= action;
        }
    }
}
