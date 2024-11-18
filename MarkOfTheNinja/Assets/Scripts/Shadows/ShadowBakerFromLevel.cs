using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shadows
{
    #if UNITY_EDITOR
    public class ShadowBakerFromLevel:MonoBehaviour
    {
        public string[] tilemapWithShadowBakerNames;
        private List<ShadowCaster2DCreator> shadowBakers;
        public void GenerateShadows()
        {
            SetUp();
            foreach (var shadowCaster in shadowBakers)
            {
                shadowCaster.Create();
            }
        }

        public void DeleteOldShadows()
        {
            SetUp();
            foreach (var shadowCaster in shadowBakers)
            {
                shadowCaster.DestroyOldShadowCasters();
            }
        }


        private void SetUp()
        {
            shadowBakers = new();
            foreach (var tilemapName in tilemapWithShadowBakerNames)
            {
                var tilemap = FindChildByName(transform, tilemapName) ?? throw new Exception($"There is no tilempap with name: {tilemapName}");
                var shadowBaker = tilemap.GetComponent<ShadowCaster2DCreator>() ?? throw new Exception($"Tilemap {tilemapName} has no component ShadowCaster2DCreator");
                shadowBakers.Add(shadowBaker);
            }
        }

        private GameObject FindChildByName(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child.gameObject;

                GameObject found = FindChildByName(child, name);
                if (found != null)
                    return found;
            }
            return null;
        }

    }

    [CustomEditor(typeof(ShadowBakerFromLevel))]
    public class ShadowCaster2DTileMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Delete old Shadows"))
            {
                var creator = (ShadowBakerFromLevel)target;
                creator.DeleteOldShadows();
            }
            if (GUILayout.Button("Create shadows"))
            {
                var creator = (ShadowBakerFromLevel)target;
                creator.GenerateShadows();
            }
        }

    }

#endif
}
