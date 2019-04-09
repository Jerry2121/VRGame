﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;
using Unity.Networking.Transport;

using UdpCNetworkDriver = Unity.Networking.Transport.BasicNetworkDriver<Unity.Networking.Transport.IPv4UDPSocket>;

namespace VRGame.Networking
{
    public class NetworkClient : MonoBehaviour
    {

        //#region singleton
        //private static readonly Lazy<NetworkClient> lazy =
        //    new Lazy<NetworkClient>(() => new NetworkClient());

        //public static NetworkClient Instance { get { return lazy.Value; } }

        //private NetworkClient()
        //{
        //    Init();
        //}
        //#endregion

        int m_clientID = -1;

        TempPlayer m_player;

        public int ClientID { get => m_clientID;}
        
        public UdpCNetworkDriver m_Driver;
        public NetworkConnection m_Connection;
        public bool m_Done;

        List<string> messageList = new List<string>();


        void Start()
        {
            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
            m_Connection = default(NetworkConnection);

            if(NetworkingManager.Instance.NetworkAddress() == null)
            {
                Debug.Log("NetworkClient -- Start: Invalid IP address");
                Disconnect();
                NetworkingManager.Instance.ClientDisconnect();
                return;
            }

            var endpoint = new IPEndPoint(NetworkingManager.Instance.NetworkAddress(), 9000);
            m_Connection = m_Driver.Connect(endpoint);

            WriteMessage("Connected");

            if (Debug.isDebugBuild)
                Debug.Log("NetworkClient -- Start: Client created");
        }

        public void OnDestroy()
        {
            m_Driver.Dispose();
        }

        void Update()
        {
            //The main part of this Update code is from Unity's multiplayer repo

            m_Driver.ScheduleUpdate().Complete();

            if (!m_Connection.IsCreated)
            {
                if (!m_Done)
                    Debug.Log("NetworkClient -- Something went wrong during connect");
                return;
            }

            DataStreamReader stream;
            NetworkEvent.Type cmd;

            while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) !=
                   NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("NetworkClient -- We are now connected to the server");
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    var readerCtx = default(DataStreamReader.Context);

                    try
                    {
                        byte[] messageBytes = new byte[stream.Length];
                        stream.ReadBytesIntoArray(ref readerCtx, ref messageBytes, stream.Length);
                        string recievedMessage = Encoding.Unicode.GetString(messageBytes);

                        //Debug.Log("NetworkClient -- Got the value " + recievedMessage + " from the server");

                        string[] splitMessages = NetworkTranslater.SplitMessages(recievedMessage);

                        foreach (var msg in splitMessages)
                        {
                            TranslateMessage(msg);
                        }
                    }
                    catch (NullReferenceException) { }
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("NetworkClient -- Client got disconnected from server");
                    m_Connection = default(NetworkConnection);

                    NetworkingManager.Instance.ClientDisconnect();
                }
            }

            //send the first message in the message list
            try
            {
                if (m_clientID == -1)
                {
                    if (messageList.Count > 0)
                        messageList.Clear(); //none of our messages have the proper ID
                    string IDRequest = NetworkTranslater.CreateIDMessageFromClient();
                    SendMessages(Encoding.Unicode.GetBytes(IDRequest));
                    return;
                }
            }
            catch (InvalidOperationException) {
                return;
            }

            if(messageList.Count <= 0)
            {
                SendMessages(Encoding.Unicode.GetBytes(string.Empty));
            }

            else {
                string allMessages = NetworkTranslater.CombineMessages(messageList);
                messageList.Clear();
                SendMessages(Encoding.Unicode.GetBytes(allMessages));
            }

            if (m_Done)
            {
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
        }

        void SendMessages(byte[] buffer)
        {
            using (var writer = new DataStreamWriter(1024, Allocator.Temp))
            {
                writer.Write(buffer);

                //Debug.LogFormat("NetworkClient -- Sending message {0} to server", Encoding.Unicode.GetString(buffer));
                //Debug.LogFormat("NetworkClient -- Message  is {0} in bytes", BitConverter.ToString(messageList[0]));

                m_Connection.Send(m_Driver, writer);
            }
        }

        public void WriteMessage(string message)
        {
            messageList.Add(message);
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
                case NetworkMessageContent.None:
                    break;
            }
        }

        void MoveMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslateMoveMessage(recievedMessage, out int clientID, out int objectID, out float x, out float z) == false)
                return;

            throw new NotImplementedException();
        }

        void PositionMessage(string recievedMessage)
        {
            if (NetworkTranslater.GetIDsFromMessage(recievedMessage, out int clientID, out int objectID))
                return;

            if(clientID != m_clientID) //Make sure we aren't getting out own positions back
            {
                //NetworkingManager.Instance.playerDictionary[clientID].RecievePositionMessage(xPos, yPos, zPos);
                NetworkingManager.Instance.networkedObjectDictionary[objectID].RecieveMessage(recievedMessage, NetworkMessageContent.Position);
            }
        }

        void IDMessage(string recievedMessage)
        {
            if (NetworkTranslater.TranslateIDMessage(recievedMessage, out int clientID) == false)
                return;

            if (NetworkingManager.Instance.playerDictionary.ContainsKey(clientID) || clientID == -1)
                return;

            if (m_clientID != -1) //The message is for someone else
            {
                NetworkingManager.Instance.playerDictionary.Add(clientID, null);
            }

            NetworkingManager.Instance.playerDictionary.Add(clientID, null);
            m_clientID = clientID;

            //TempPlayer player = Instantiate(NetworkingManager.Instance.playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TempPlayer>();
            //player.SetIsLocalPlayer();
            //player.SetPlayerID(clientID);

            //NetworkingManager.Instance.playerDictionary[clientID] = player;

            //WriteMessage(NetworkTranslater.CreateInstantiateMessage(m_playerID, m_playerID, "Player", player.transform.position));

            //WriteMessage(NetworkTranslater.CreateInstantiateMessage(m_clientID, -1, "Player", Vector3.zero);

            NetworkingManager.Instance.InstantiateOverNetwork("Player", Vector3.zero);
        }

        void InstantiateMessage(string recievedMessage)
        {
            NetworkingManager.Instance.RecieveInstantiateMessage(recievedMessage);
        }

        public void Disconnect()
        {
            m_Done = true;
        }

        public void AssignPlayer(TempPlayer player)
        {
            if(m_player == null)
            {
                m_player = player;
            }
            else
            {
                if(m_player == player)
                    Debug.LogWarning("NetworkClient -- AssignPlayer: Tried to assign the local player twice");
                else
                    Debug.LogError("NetworkClient -- AssignPlayer: Tried to assign the local player to a different player! One of these players is incorrect");
            }
        }

    }
}

