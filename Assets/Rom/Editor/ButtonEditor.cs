using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq; // Added for LINQ methods
using System.Collections.Generic; // Added for List

[CustomEditor(typeof(MonoBehaviour), true)] // Draw for all MonoBehaviour scripts
public class InspectorButtonDrawer : Editor // Renamed class
{
    public override void OnInspectorGUI()
    {
        // Don't draw the default inspector anymore
        // base.OnInspectorGUI();

        var monoBehaviour = target as MonoBehaviour;
        if (monoBehaviour == null)
            return;

        var type = target.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null)
                         .Cast<MemberInfo>(); // Cast to base MemberInfo for sorting

        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                          .Where(m => m.GetCustomAttribute<InspectorButtonAttribute>() != null)
                          .Cast<MemberInfo>(); // Cast to base MemberInfo for sorting

        // Combine and sort members by metadata token (approximates declaration order)
        var members = fields.Concat(methods).OrderBy(m => m.MetadataToken).ToList();

        serializedObject.Update(); // Update SerializedObject representation

        foreach (var member in members)
        {
            if (member is FieldInfo field)
            {
                // Draw serializable fields
                var property = serializedObject.FindProperty(field.Name);
                if (property != null)
                {
                    EditorGUILayout.PropertyField(property, true); // Draw field using PropertyField
                }
                // Optionally handle non-serialized public fields if needed, though standard inspector doesn't show them.
            }
            else if (member is MethodInfo method)
            {
                // Draw button for methods with the attribute
                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(monoBehaviour, null);
                }
            }
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes back to the object
    }
}