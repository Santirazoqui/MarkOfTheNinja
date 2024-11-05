using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class GenerateAnimatorConnections:MonoBehaviour
    {
        public string path;

        public void CreateAnimatorConnections()
        {
            // Obtén el controlador de Animator que deseas modificar
            AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));

            if (controller == null)
            {
                Debug.LogError($"No se encontró ningun controlador de Animator en el path: {path}");
                return;
            }

            //Agrega los parametros de los 
            foreach (var layer in controller.layers)
            {
                foreach (var stateA in layer.stateMachine.states)
                {
                    controller.AddParameter(stateA.state.name, AnimatorControllerParameterType.Trigger);
                }
            }

            // Recorre todos los estados del controlador
            foreach (var layer in controller.layers)
            {
                foreach (var stateA in layer.stateMachine.states)
                {
                    foreach (var stateB in layer.stateMachine.states)
                    {
                        if (stateA.state != stateB.state)
                        {
                            AnimatorStateTransition transition = stateA.state.AddTransition(stateB.state);
                            transition.AddCondition(AnimatorConditionMode.If,0,stateB.state.name);
                            transition.hasExitTime = false; 
                            transition.duration = 0f;
                        }
                    }
                }
            }

            Debug.Log("Conexiones creadas correctamente.");
        }

        public void DeleteAnimatorConnections()
        {
            // Obtén el controlador de Animator que deseas modificar
            AnimatorController controller = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));

            if (controller == null)
            {
                Debug.LogError($"No se encontró ningun controlador de Animator en el path: {path}");
                return;
            }

            // Elimina todas las conexiones
            foreach (var layer in controller.layers)
            {
                foreach (var stateA in layer.stateMachine.states)
                {
                    foreach(var transition in stateA.state.transitions)
                    {
                        stateA.state.RemoveTransition(transition);
                    }
                }
            }

            foreach(var parameter in controller.parameters)
            {
                controller.RemoveParameter(parameter);
            }

            Debug.Log("Conexiones eliminadas correctamente.");
        }
    }

    [CustomEditor(typeof(GenerateAnimatorConnections))]
    public class ShadowCaster2DTileMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Create connections"))
            {
                var creator = (GenerateAnimatorConnections)target;
                creator.CreateAnimatorConnections();
            }

            if (GUILayout.Button("Delete connections"))
            {
                var creator = (GenerateAnimatorConnections)target;
                creator.DeleteAnimatorConnections();
            }
        }

    }
}
