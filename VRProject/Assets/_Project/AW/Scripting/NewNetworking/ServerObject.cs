using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    public class ServerObject
    {
        public Vector3 m_Position;

        public string m_objectType;

        public int m_clientID = 0; //Really only matters for player objects

        public void SetPosition(float x, float y, float z)
        {
            m_Position.x = x;
            m_Position.y = y;
            m_Position.z = z;
        }

        public ServerObject(string objectType)
        {
            m_objectType = objectType;
        }

        public ServerObject(int clientID, string objectType, float x, float y, float z)
        {
            m_objectType = objectType;
            m_clientID = clientID;

            m_Position.x = x;
            m_Position.y = y;
            m_Position.z = z;
        }

    }

}
