using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPuzzle
{
    public bool m_Started;

    public int m_Progress = -1;

    public bool m_Complete;

    public int m_ObjectID = -1;

    public int m_ComponentID = -1;

    public ServerPuzzle(int objectID, int componentID)
    {
        m_ObjectID = objectID;
        m_ComponentID = componentID;
    }

}
