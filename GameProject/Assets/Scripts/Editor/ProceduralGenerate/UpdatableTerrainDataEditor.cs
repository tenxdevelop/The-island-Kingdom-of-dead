using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpdatableTerrainData), true)]
public class UpdatableTerrainDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UpdatableTerrainData data = (UpdatableTerrainData)target;
        if (GUILayout.Button("Generate Map"))
        {
            data.NotifyOfUpdatedValues();
        }
    }
}
