using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using System.Reflection;
#if UNITY_EDITOR

[RequireComponent(typeof(CompositeCollider2D))]
public class Light2DFreeformCreator : MonoBehaviour
{
    public float lightIntensity;
    public Color lightColor;
    [SerializeField] int[] m_ApplyToSortingLayers;
    private CompositeCollider2D tilemapCollider;

    private void Awake()
    {
        // Default target sorting layers to "All"
        if (m_ApplyToSortingLayers == null)
            m_ApplyToSortingLayers = SortingLayer.layers.Select(x => x.id).ToArray();
    }
    public void Create()
    {
        DestroyOldLights();

        var actualTilemapCollider = GetComponent<TilemapCollider2D>();
        var ogValue = actualTilemapCollider.usedByComposite;
        actualTilemapCollider.usedByComposite = true;
        tilemapCollider = GetComponent<CompositeCollider2D>();

        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            // Obtener los vértices del camino actual
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);

            // Crear una nueva Light2D
            GameObject lightObject = new GameObject("light2D_freeform_" + i);
            lightObject.transform.parent = gameObject.transform;
            lightObject.transform.position = gameObject.transform.position;

            Light2D light2DComponent = lightObject.AddComponent<Light2D>();
            light2DComponent.lightType = Light2D.LightType.Freeform;

            // Asignar los vértices a la Light2D
            Vector3[] lightVertices = pathVertices.Select(v => (Vector3)v).ToArray();
            light2DComponent.SetShapePath(lightVertices);

            // Configurar otros parámetros de la luz si es necesario
            light2DComponent.intensity = lightIntensity;
            light2DComponent.color = lightColor;
            light2DComponent.falloffIntensity = 0;
            light2DComponent.shapeLightFalloffSize = 0;
            Light2DLayersMaskAccessExtension.SetLayers(light2DComponent, m_ApplyToSortingLayers);
        }

        actualTilemapCollider.usedByComposite = ogValue;
    }

    public void DestroyOldLights()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            bool isLight2D = child.gameObject.GetComponent<Light2D>() != null;
            if (isLight2D)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    public void GetLayers()
    {
        GameObject lightObject = new GameObject("light2D_freeform_for_testing");
        Light2D light2DComponent = lightObject.AddComponent<Light2D>();
        var layer = Light2DLayersMaskAccessExtension.GetLayers(light2DComponent);
        Debug.Log("Layers:");
        foreach (int layerMask in layer)
        {
            Debug.Log(layerMask);
        }
        DestroyImmediate(lightObject);
    }

}

public static class Light2DLayersMaskAccessExtension
{
    public static int[] GetLayers(this Light2D light)
    {
        FieldInfo targetSortingLayersField = typeof(Light2D).GetField("m_ApplyToSortingLayers",
                                                                   BindingFlags.NonPublic |
                                                                   BindingFlags.Instance);
        int[] mask = targetSortingLayersField.GetValue(light) as int[];
        return mask;
    }
    public static void SetLayers(this Light2D light, int[] mask)
    {
        FieldInfo targetSortingLayersField = typeof(Light2D).GetField("m_ApplyToSortingLayers",
                                                                   BindingFlags.NonPublic |
                                                                   BindingFlags.Instance);
        targetSortingLayersField.SetValue(light, mask);
    }
}


[CustomEditor(typeof(Light2DFreeformCreator))]
public class Light2DFreeformTileMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Lights"))
        {
            var creator = (Light2DFreeformCreator)target;
            creator.Create();
        }

        if (GUILayout.Button("Remove Lights"))
        {
            var creator = (Light2DFreeformCreator)target;
            creator.DestroyOldLights();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Get sorting layers ids in console"))
        {
            var creator = (Light2DFreeformCreator)target;
            creator.GetLayers();
        }
    }
}

#endif
