using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(MapGenerator))]
public class MapGenerateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGenerator.GetAutoUpdate())
            {
                mapGenerator.DrawMapInEditor();
            }
        }
        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.DrawMapInEditor();
        }
 

    }
}
