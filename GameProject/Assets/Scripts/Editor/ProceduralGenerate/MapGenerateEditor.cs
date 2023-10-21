using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(MapGenerator))]
public class MapGenerateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateMap();
        }
    }
}
