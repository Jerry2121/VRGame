using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRGame.Networking
{

    [CustomEditor(typeof(NetworkObject))]
    public class NetworkObjectEditor : Editor
    {

        SerializedProperty m_ObjectName;
        int m_ObjectID;
        SerializedProperty m_LocalAuthority;
        SerializedProperty m_ServerAuthority;

        private void OnEnable()
        {
            m_ObjectName = serializedObject.FindProperty("m_ObjectName");
            m_ObjectID = serializedObject.FindProperty("m_ObjectID").intValue;
            m_LocalAuthority = serializedObject.FindProperty("m_LocalAuthority");
            m_ServerAuthority = serializedObject.FindProperty("m_ServerAuthority");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_ObjectName);
            EditorGUILayout.LabelField(string.Format("Object Network ID: {0} ", m_ObjectID.ToString()), EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_LocalAuthority);
            EditorGUILayout.PropertyField(m_ServerAuthority);

            unsafe
            {
                if (GUILayout.Button("Register Network Component IDs"))
                {
                    NetworkObject netScript = ((NetworkObject)target);
                    int ID = 0;
                    netScript.CheckForNetworkComponents(netScript.gameObject, &ID);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
