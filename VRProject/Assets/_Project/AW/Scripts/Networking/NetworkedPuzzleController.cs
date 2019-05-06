using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGame.Networking;

public abstract class NetworkedPuzzleController : MonoBehaviour, INetworkMessageReciever
{
    public abstract int ID { get; protected set; }

    public abstract void RecieveNetworkMessage(string recievedMessage);

    public abstract void SendNetworkMessage(string messageToSend);

    public abstract void SetID(int newID);
}
