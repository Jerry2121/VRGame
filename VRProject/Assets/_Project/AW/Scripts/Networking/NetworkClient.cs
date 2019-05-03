using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;
using Unity.Networking.Transport;

using UdpCNetworkDriver = Unity.Networking.Transport.BasicNetworkDriver<Unity.Networking.Transport.IPv4UDPSocket>;
using System.Timers;

namespace VRGame.Networking
{
    class NetworkClient : MonoBehaviour
    {
        int m_ClientID = -1;

        NetworkPlayer m_Player;

        public UdpCNetworkDriver m_Driver;
        public NetworkConnection m_Connection;
        public bool m_Done;

        List<string> m_MessageList = new List<string>();

        Timer m_ConnectionTimer;

        void Start()
        {
            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
            m_Connection = default(NetworkConnection);

            if(NetworkingManager.s_Instance.NetworkAddress() == null)
            {
                Debug.Log("NetworkClient -- Start: Invalid IP address");
                Disconnect();
                NetworkingManager.s_Instance.Disconnect();
                return;
            }

            var endpoint = new IPEndPoint(NetworkingManager.s_Instance.NetworkAddress(), 9000);
            m_Connection = m_Driver.Connect(endpoint);

            WriteMessage("Connected");

            if (Debug.isDebugBuild)
                Debug.Log("NetworkClient -- Start: Client created");

            m_ConnectionTimer = new Timer(15000/2);
            m_ConnectionTimer.Elapsed += OnConnectionTimerDone;
            m_ConnectionTimer.Enabled = true;
        }

        public void OnDestroy()
        {
            m_Driver.Dispose();
            m_ConnectionTimer.Stop();
            m_ConnectionTimer.Dispose();
        }

        void Update()
        {
            m_Driver.ScheduleUpdate().Complete();

            if (m_Connection.IsCreated == false)
            {
                if (!m_Done && Debug.isDebugBuild)
                    Debug.Log("NetworkClient -- Update: Something went wrong during connect");
                return;
            }

            DataStreamReader stream;
            NetworkEvent.Type cmd;

            if (m_Done)
            {
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
                Destroy(this);
                return;
            }

            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) !=
                   NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    if (Debug.isDebugBuild)
                        Debug.Log("NetworkClient -- Update: We are now connected to the server");
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    var readerCtx = default(DataStreamReader.Context);

                    try
                    {
                        byte[] messageBytes = new byte[stream.Length];
                        stream.ReadBytesIntoArray(ref readerCtx, ref messageBytes, stream.Length);
                        string recievedMessage = Encoding.UTF8.GetString(messageBytes);

                        //Debug.Log("NetworkClient -- Got the value " + recievedMessage + " from the server");

                        string[] splitMessages = NetworkTranslater.SplitMessages(recievedMessage);

                        foreach (var msg in splitMessages)
                        {
                            TranslateMessage(msg);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        if(Debug.isDebugBuild)
                            Debug.LogError("NetworkClient -- Update: Caught Null Reference");
                    }
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("NetworkClient -- Update: Client got disconnected from server");
                    m_Connection = default(NetworkConnection);

                    NetworkingManager.s_Instance.Disconnect();
                }
            }

            //send the first message in the message list
            try
            {
                if (m_ClientID == -1)
                {
                    if (m_MessageList.Count > 0)
                        m_MessageList.Clear(); //none of our messages have the proper ID, so clear them
                    string IDRequest = NetworkTranslater.CreateIDMessageFromClient();

                    Debug.Log("NetworkClient -- Update: Sending ID Request");

                    SendMessages(Encoding.UTF8.GetBytes(IDRequest));
                    return;
                }
            }
            catch (InvalidOperationException) {
                if (Debug.isDebugBuild)
                    Debug.LogError("NetworkClient -- Update: InvalidOperationException caught. Did not send ID request");
                return;
            }

            if(m_MessageList.Count <= 0)
            {
                SendMessages(Encoding.UTF8.GetBytes(UnityEngine.Random.Range(0f, 10f).ToString()));
            }

            else {
                string allMessages = NetworkTranslater.CombineMessages(m_MessageList);
                m_MessageList.Clear();
                SendMessages(Encoding.UTF8.GetBytes(allMessages));
            }

            if (m_Done)
            {
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
        }

        void OnConnectionTimerDone(object source, ElapsedEventArgs e)
        {
            m_ConnectionTimer.Stop();
            m_ConnectionTimer.Dispose();

            if(m_ClientID == -1)
            {
                Debug.Log("NetworkClient -- Failed To Connect");
                NetworkingManager.s_Instance.Disconnect();
                //GetComponent<NetworkingManager>().Disconnect();
            }
        }

        void SendMessages(byte[] buffer)
        {
            using (var writer = new DataStreamWriter(1024, Allocator.Temp))
            {
                writer.Write(buffer);

                //Debug.LogFormat("NetworkClient -- Sending message {0} to server", Encoding.UTF8.GetString(buffer));
                //Debug.LogFormat("NetworkClient -- Message  is {0} in bytes", BitConverter.ToString(messageList[0]));

                m_Connection.Send(m_Driver, writer);
            }
        }

        public void WriteMessage(string message)
        {
            m_MessageList.Add(message);
        }


        void TranslateMessage(string recievedMessage)
        {
            NetworkMessageContent msgContent = NetworkTranslater.GetMessageContentType(recievedMessage);

            switch (msgContent)
            {
                case NetworkMessageContent.Move:
                    MoveMessage(recievedMessage);
                    break;
                case NetworkMessageContent.Position:
                    PositionMessage(recievedMessage);
                    break;
                case NetworkMessageContent.ClientID:
                    IDMessage(recievedMessage);
                    break;
                case NetworkMessageContent.Instantiate:
                    InstantiateMessage(recievedMessage);
                    break;
                case NetworkMessageContent.Rotation:
                    RotationMessage(recievedMessage);
                    break;
                case NetworkMessageContent.Disconnected:
                    DisconnectedMessage(recievedMessage);
                    break;
                case NetworkMessageContent.None:
                    break;
            }
        }

        void MoveMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslateMoveMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out float x, out float z) == false)
                return;

            throw new NotImplementedException();
        }

        void PositionMessage(string recievedMessage)
        {
            //if(Debug.isDebugBuild)
                //Debug.Log("NetworkClient -- PositionMessage");

            if (NetworkTranslater.GetIDsFromMessage(recievedMessage, out int clientID, out int objectID, out int componentID) == false)
                return;

            if(clientID != m_ClientID) //Make sure we aren't getting out own positions back
            {
                //NetworkingManager.Instance.playerDictionary[clientID].RecievePositionMessage(xPos, yPos, zPos);
                NetworkingManager.s_Instance.PassNetworkMessageToReciever(recievedMessage, objectID, componentID);
            }
        }

        void RotationMessage(string recievedMessage)
        {
            //if (Debug.isDebugBuild)
                //Debug.Log("NetworkClient -- RotationMessage");

            if (NetworkTranslater.GetIDsFromMessage(recievedMessage, out int clientID, out int objectID, out int componentID) == false)
                return;

            if (clientID != m_ClientID) //Make sure we aren't getting out own rotations back
            {
                //NetworkingManager.Instance.playerDictionary[clientID].RecievePositionMessage(xPos, yPos, zPos);
                NetworkingManager.s_Instance.PassNetworkMessageToReciever(recievedMessage, objectID, componentID);
            }
        }

        void IDMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslateIDMessage(recievedMessage, out int clientID) == false)
                return;

            //if (NetworkingManager.s_Instance.m_PlayerDictionary.ContainsKey(clientID) || clientID == -1)
                //return;

            //NetworkingManager.s_Instance.m_PlayerDictionary.Add(clientID, null);

            if (m_ClientID != -1) //The message is for someone else
            {
                return;
            }

            m_ClientID = clientID;

            Debug.Log(string.Format("NetworkClient -- IDMessage: Recieved ID of {0} from the server", clientID));

            //TempPlayer player = Instantiate(NetworkingManager.Instance.playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TempPlayer>();
            //player.SetIsLocalPlayer();
            //player.SetPlayerID(clientID);

            //NetworkingManager.Instance.playerDictionary[clientID] = player;

            //WriteMessage(NetworkTranslater.CreateInstantiateMessage(m_playerID, m_playerID, "Player", player.transform.position));

            //WriteMessage(NetworkTranslater.CreateInstantiateMessage(m_clientID, -1, "Player", Vector3.zero);

            //NetworkingManager.Instance.InstantiateOverNetwork("Player", Vector3.zero);
            NetworkingManager.s_Instance.SwitchToOnlineScene();
        }

        void InstantiateMessage(string recievedMessage)
        {
            NetworkingManager.s_Instance.RecieveInstantiateMessage(recievedMessage);
        }

        void DisconnectedMessage(string recievedMessage)
        {
            if(Debug.isDebugBuild)
                Debug.Log("NetworkClient -- Disconnected");
            if (NetworkTranslater.TranslateDisconnectedMessage(recievedMessage, out int clientID) == false)
                return;

            NetworkingManager.s_Instance.DestroyPlayer(clientID);
        }

        void PuzzleStartedMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslatePuzzleStartedMessage(recievedMessage, out int clientID, out int puzzleID) == false)
                return;

            GameManager.s_Instance.PassPuzzleNetworkMessage(recievedMessage, puzzleID);
        }

        public void Disconnect()
        {
            m_Done = true;
        }

        //public void AssignPlayer(NetworkPlayer player)
        //{
        //    if(m_player == null)
        //    {
        //        m_player = player;
        //    }
        //    else
        //    {
        //        if(m_player == player)
        //            Debug.LogWarning("NetworkClient -- AssignPlayer: Tried to assign the local player twice");
        //        else
        //            Debug.LogError("NetworkClient -- AssignPlayer: Tried to assign the local player to a different player! One of these players is incorrect");
        //    }
        //}
        
        public int ClientID()
        {
            return m_ClientID;
        }

    }
}

