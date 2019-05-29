using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(SpacebrewClient))]
public sealed class SpacebrewClientEditor : Editor {

    const string DEFAULT_MESSAGE_TYPE = "<default>";


    SerializedProperty clientNameProp;
    SerializedProperty descriptionTextProp;

    SerializedProperty isAdminProp;

    SerializedProperty isServerProp;
    SerializedProperty useDefaultPortProp;
    SerializedProperty useConfigManagerProp;
    SerializedProperty serverAddressProp;
    SerializedProperty serverPortProp;

    SerializedProperty messageTypesProp;
    SerializedProperty publishersProp;
    SerializedProperty subscribersProp;


    private bool showMsgTypes = true;
    private ReorderableList messageTypesList;
    private bool showPublishers = true;
    private ReorderableList publishersList;
    private bool showSubscribers = true;
    private ReorderableList subscribersList;


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


        // Message Types
        messageTypesProp = serializedObject.FindProperty("messageTypes");
        messageTypesList = new ReorderableList(serializedObject, messageTypesProp) {
            draggable = true,
            displayAdd = true,
            displayRemove = true,

            drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Types List");
            },

            drawElementCallback = (rect, index, active, focused) => {
                var element = messageTypesProp.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            },

            elementHeight = EditorGUIUtility.singleLineHeight,

            drawNoneElementCallback = (rect) => {
                EditorGUI.LabelField(rect, "No types: Using DEFAULT for all messages");
            },

            onAddCallback = list => {
                var index = list.serializedProperty.arraySize;

                list.serializedProperty.arraySize++;
                list.index = index;
                var element = list.serializedProperty.GetArrayElementAtIndex(index);

                element.stringValue = "";
            }
        };


        // Publishers
        publishersProp = serializedObject.FindProperty("publishers");
        publishersList = new ReorderableList(serializedObject, publishersProp) {
            draggable = true,
            displayAdd = true,
            displayRemove = true,

            drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Publishers List");
            },

            drawElementCallback = (rect, index, active, focused) => {
                var element = publishersProp.GetArrayElementAtIndex(index);

                var name = element.FindPropertyRelative("name");
                var pubType = element.FindPropertyRelative("pubType");
                var msgType = element.FindPropertyRelative("msgType");
                //var defaultValue = element.FindPropertyRelative("defaultValue");

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), name);
                rect.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), pubType);

                if ((SpacebrewClient.DataType)pubType.intValue == SpacebrewClient.DataType.STRING) {
                    rect.y += EditorGUIUtility.singleLineHeight;

                    List<string> list = new List<string>() { DEFAULT_MESSAGE_TYPE };
                    for (int i=0; i<messageTypesProp.arraySize; ++i) {
                        list.Add(messageTypesProp.GetArrayElementAtIndex(i).stringValue);
                    }

                    int choice = list.IndexOf(msgType.stringValue);
                    if (choice == -1) {
                        choice = 0; // default
                    }
                    choice = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Msg type", choice, list.ToArray());
                    msgType.stringValue = list[choice];
                }
            },

            elementHeightCallback = index => {
                var element = publishersProp.GetArrayElementAtIndex(index);
                var pubType = element.FindPropertyRelative("pubType");

                switch ((SpacebrewClient.DataType)pubType.intValue) {
                    case SpacebrewClient.DataType.STRING:
                        return EditorGUIUtility.singleLineHeight * 3;

                    default:
                        return EditorGUIUtility.singleLineHeight * 2;
                }
            },

            onAddCallback = list => {
                var index = list.serializedProperty.arraySize;

                list.serializedProperty.arraySize++;
                list.index = index;
                var element = list.serializedProperty.GetArrayElementAtIndex(index);

                var name = element.FindPropertyRelative("name");
                var pubType = element.FindPropertyRelative("pubType");
                var msgType = element.FindPropertyRelative("msgType");
                //var defaultValue = element.FindPropertyRelative("defaultValue");

                name.stringValue = "pub_" + (index+1);
                pubType.intValue = (int)SpacebrewClient.DataType.STRING;
                msgType.stringValue = DEFAULT_MESSAGE_TYPE;
                //defaultValue.stringValue = "";
            }
        };


        // Subscribers
        subscribersProp = serializedObject.FindProperty("subscribers");
        subscribersList = new ReorderableList(serializedObject, subscribersProp) {
            draggable = true,
            displayAdd = true,
            displayRemove = true,

            drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Subscribers List");
            },

            drawElementCallback = (rect, index, active, focused) => {
                var element = subscribersProp.GetArrayElementAtIndex(index);

                var name = element.FindPropertyRelative("name");
                var subType = element.FindPropertyRelative("subType");
                var msgType = element.FindPropertyRelative("msgType");

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), name);
                rect.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), subType);

                if ((SpacebrewClient.DataType)subType.intValue == SpacebrewClient.DataType.STRING) {
                    rect.y += EditorGUIUtility.singleLineHeight;

                    List<string> list = new List<string>() { DEFAULT_MESSAGE_TYPE };
                    for (int i=0; i<messageTypesProp.arraySize; ++i) {
                        list.Add(messageTypesProp.GetArrayElementAtIndex(i).stringValue);
                    }

                    int choice = list.IndexOf(msgType.stringValue);
                    if (choice == -1) {
                        choice = 0; // default
                    }
                    choice = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Msg type", choice, list.ToArray());
                    msgType.stringValue = list[choice];
                }
            },

            elementHeightCallback = index => {
                var element = subscribersProp.GetArrayElementAtIndex(index);
                var pubType = element.FindPropertyRelative("subType");

                switch ((SpacebrewClient.DataType)pubType.intValue) {
                    case SpacebrewClient.DataType.STRING:
                        return EditorGUIUtility.singleLineHeight * 3;

                    default:
                        return EditorGUIUtility.singleLineHeight * 2;
                }
            },

            onAddCallback = list => {
                var index = list.serializedProperty.arraySize;

                list.serializedProperty.arraySize++;
                list.index = index;
                var element = list.serializedProperty.GetArrayElementAtIndex(index);

                var name = element.FindPropertyRelative("name");
                var subType = element.FindPropertyRelative("subType");
                var msgType = element.FindPropertyRelative("msgType");

                name.stringValue = "sub_" + (index+1);
                subType.intValue = (int)SpacebrewClient.DataType.STRING;
                msgType.stringValue = DEFAULT_MESSAGE_TYPE;
            }
        };

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


        showMsgTypes = EditorGUILayout.Foldout(showMsgTypes, "Messages Types");
        if (showMsgTypes) {
            EditorGUILayout.HelpBox("Messages types are only used if there is an 'Admin' entity, to automatically configure routes between clients", MessageType.Warning);
            messageTypesList.DoLayoutList();
        }

        showPublishers = EditorGUILayout.Foldout(showPublishers, "Publishers");
        if (showPublishers) {
            publishersList.DoLayoutList();
        }

        showSubscribers = EditorGUILayout.Foldout(showSubscribers, "Subscribers");
        if (showSubscribers) {
            subscribersList.DoLayoutList();
        }

        serializedObject.ApplyModifiedProperties();
    }

}
