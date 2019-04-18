using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SpacebrewClient))]
public sealed class SpacebrewClientEditor : Editor {

    SerializedProperty clientNameProp;
    SerializedProperty descriptionTextProp;

    SerializedProperty isAdminProp;

    SerializedProperty isServerProp;
    SerializedProperty useDefaultPortProp;
    SerializedProperty useConfigManagerProp;
    SerializedProperty serverAddressProp;
    SerializedProperty serverPortProp;

    SerializedProperty publishersProp;
    SerializedProperty subscribersProp;


//TODO: should use "InitializeOnLoad"?
    private void OnEnable() {
        clientNameProp = serializedObject.FindProperty("clientName");
        descriptionTextProp = serializedObject.FindProperty("descriptionText");

        isAdminProp = serializedObject.FindProperty("isAdmin");

        isServerProp = serializedObject.FindProperty("isServer");
        useDefaultPortProp = serializedObject.FindProperty("useDefaultPort");
        useConfigManagerProp = serializedObject.FindProperty("useConfigManager");
        serverAddressProp = serializedObject.FindProperty("serverAddress");
        serverPortProp = serializedObject.FindProperty("serverPort");

        publishersProp = serializedObject.FindProperty("publishers");
        subscribersProp = serializedObject.FindProperty("subscribers");
//TODO: check that not null!
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();

        EditorGUILayout.PropertyField(clientNameProp, new GUIContent("Client Name"));
        EditorGUILayout.PropertyField(descriptionTextProp, new GUIContent("Description"));

        EditorGUILayout.PropertyField(isAdminProp, new GUIContent("Is Admin"));

        EditorGUILayout.PropertyField(isServerProp, new GUIContent("Is Server"));
        if (isServerProp.boolValue) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(useDefaultPortProp, new GUIContent("Use Default Port"));
            if (!useDefaultPortProp.boolValue) {
                EditorGUI.indentLevel++;
//TODO: "use default port" + text field
//=> if setting the port, need to add the argument to Node.js command line! (=> MODIFY NODEJS PROJECT? OR ACCESS FROM HERE?)
                EditorGUILayout.PropertyField(serverPortProp, new GUIContent("Server Port"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        else {
            EditorGUILayout.PropertyField(useConfigManagerProp, new GUIContent("Use Config Manager"));
            if (!useConfigManagerProp.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serverAddressProp, new GUIContent("Server Address"));
                EditorGUILayout.PropertyField(serverPortProp, new GUIContent("Server Port"));
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.PropertyField(publishersProp, true);
        EditorGUILayout.PropertyField(subscribersProp, true);

        serializedObject.ApplyModifiedProperties();
    }

}
