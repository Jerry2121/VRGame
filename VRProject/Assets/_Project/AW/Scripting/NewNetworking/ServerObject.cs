using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    public class ServerObject
    {
        public Vector3 m_Position;

        public string objectType;

        public void SetPosition(float x, float y, float z)
        {
            m_Position.x = x;
            m_Position.y = y;
            m_Position.z = z;
        }

        public ServerObject(string objectType)
        {
            this.objectType = objectType;
        }

        public ServerObject(string objectType, float x, float y, float z)
        {
            this.objectType = objectType;

            m_Position.x = x;
            m_Position.y = y;
            m_Position.z = z;
        }

    }

}
