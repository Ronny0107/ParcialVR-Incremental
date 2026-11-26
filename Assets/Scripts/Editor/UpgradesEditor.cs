using UnityEditor;
using UnityEngine;

// Muestra el campo "Unlocks" solo cuando el tipo de nodo es NewObject.
[CustomEditor(typeof(Upgrades))]
public class UpgradesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "Unlocks");

        var type = serializedObject.FindProperty("Type");
        if (type != null && type.enumValueIndex == (int)UpgradeType.NewObject)
        {
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty("Unlocks"),
                new GUIContent("Prefab a desbloquear"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
