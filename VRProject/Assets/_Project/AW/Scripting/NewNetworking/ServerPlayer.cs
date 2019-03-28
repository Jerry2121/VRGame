using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    public class ServerPlayer
    {
        public Vector3 m_Position;

        public string playerType = "Player";

        public void SetPosition(float x, float y, float z)
        {
            m_Position.x = x;
            m_Position.y = y;
            m_Position.z = z;
        }

    }

}
