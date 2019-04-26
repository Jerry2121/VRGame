using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public class MoveCube : MonoBehaviour
{
    static bool m_Move;
    static bool m_Left;

    [SerializeField] float distance;

    Vector3 startPosition;
    NetworkObject netObj;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        netObj = GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //if ((startPosition - transform.position).magnitude >= distance)
            //return;


        if (m_Move)
        {
            if (netObj != null)
                netObj.SetPlayerInteracting(true);

            float movement = 0.5f;
            if (m_Left)
                movement *= -1;

            transform.Translate(movement, 0, 0);
        }
        else
        {
            if (netObj != null)
                netObj.SetPlayerInteracting(false);
        }
    }

    public static void Move(bool move, bool left)
    {
        m_Move = move;
        m_Left = left;
    }

}
