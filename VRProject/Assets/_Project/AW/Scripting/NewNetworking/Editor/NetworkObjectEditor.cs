using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRGame.Networking
{

    [CustomEditor(typeof(NetworkObject))]
    public class NetworkObjectEditor : Editor
    {

        SerializedProperty m_objectName;
        int m_objectID;

        private void OnEnable()
        {
            m_objectName = serializedObject.FindProperty("objectName");
            m_objectID = serializedObject.FindProperty("objectID").intValue;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_objectName);
            EditorGUILayout.LabelField(string.Format("Object Network ID: {0} ", m_objectID.ToString()), EditorStyles.boldLabel);

            serializedObject.ApplyModifiedProperties();
        }

    }
}
