//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//#pragma warning disable CS0618 // Type or member is obsolete
//[RequireComponent(typeof(TempPlayer))]
//public class PlayerNetworkInteractions : NetworkBehaviour
//{
//    [Command]
//    public void CmdExample(NetworkInstanceId _netID)
//    {
//        RpcExample(_netID);
//    }
//    [ClientRpc]
//    void RpcExample(NetworkInstanceId _netID)
//    {
//        GameObject GO = ClientScene.FindLocalObject(_netID);
//    }

//}
//#pragma warning restore CS0618 // Type or member is obsolete
