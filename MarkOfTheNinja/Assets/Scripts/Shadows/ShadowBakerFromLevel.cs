using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Shadows
{
    #if UNITY_EDITOR
    public class ShadowBakerFromLevel:MonoBehaviour
    {
        public string platformTilemapName = "";
        public string floorTilemapName = "";
        private ShadowCaster2DCreator platformShadowBaker;
        private ShadowCaster2DCreator floorShadowBaker;
        public void GenerateShadows()
        {
            SetUp();

            platformShadowBaker.Create();
            floorShadowBaker.Create();
        }

        public void DeleteOldShadows()
        {
            SetUp();
            platformShadowBaker.DestroyOldShadowCasters();
            floorShadowBaker.DestroyOldShadowCasters();
        }


        private void SetUp()
        {
            var platform = FindChildByName(transform, platformTilemapName) ?? throw new Exception("Incorrect platform tilemap name");
            var floor = FindChildByName(transform, floorTilemapName) ?? throw new Exception("Incorrect floor tilemap name");
            platformShadowBaker = platform.GetComponent<ShadowCaster2DCreator>();
            floorShadowBaker = floor.GetComponent<ShadowCaster2DCreator>();
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
