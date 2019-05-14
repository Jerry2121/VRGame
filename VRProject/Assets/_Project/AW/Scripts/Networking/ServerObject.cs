using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace VRGame.Networking
{

    public class ServerObject
    {
        public float3 m_Position;
        public quaternion m_Rotation;

        public string m_ObjectType;

        public int m_ObjectID;
        public int m_ClientID = 0; //Really only matters for player objects

        public void SetPosition(float x, float y, float z)
        {
            m_Position.x = x;
            m_Position.y = y;
            m_Position.z = z;
        }

        public void SetRotation(float x, float y, float z, float w)
        {
            m_Rotation.value.x = x;
            m_Rotation.value.y = y;
            m_Rotation.value.z = z;
            m_Rotation.value.w = w;
        }

        public ServerObject(string objectType)
        {
            m_ObjectType = objectType;
        }

        public ServerObject(int clientID, int objectID, string objectType, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float rotW)
        {
            m_ObjectType = objectType;
            m_ClientID = clientID;
            m_ObjectID = objectID;

            m_Position.x = posX;
            m_Position.y = posY;
            m_Position.z = posZ;

            m_Rotation.value.x = rotX;
            m_Rotation.value.y = rotY;
            m_Rotation.value.z = rotZ;
            m_Rotation.value.w = rotW;
        }
    }

}
