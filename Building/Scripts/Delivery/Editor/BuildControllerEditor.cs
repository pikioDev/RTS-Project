using UnityEditor;
using UnityEngine;

namespace Xenocode.Features.Building.Scripts.Delivery.Editor
{
    [CustomEditor(typeof(BuildingView))]
    public class BuildControllerEditor : UnityEditor.Editor
    {
        // public override void OnInspectorGUI()
        // {
        //     // Draw the default inspector fields first.
        //     DrawDefaultInspector();
        //
        //     EditorGUILayout.Space();
        //
        //     // Get a reference to the script instance.
        //     var buildController = (BuildingNetView)target;
        //
        //     // Add a button that, when clicked, calls the editor-specific method.
        //     if (GUILayout.Button("Spawn"))
        //     {
        //         buildController.Editor_SpawnUnit();
        //     }
        // }
    }
}