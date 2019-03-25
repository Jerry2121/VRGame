using System;
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

        int m_playerID = -1;

        TempPlayer m_player;

        public int PlayerID { get => m_playerID;}
        
        public UdpCNetworkDriver m_Driver;
        public NetworkConnection m_Connection;
        public bool m_Done;

        List<byte[]> messageList = new List<byte[]>();


        void Start()
        {
            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
            m_Connection = default(NetworkConnection);

            var endpoint = new IPEndPoint(IPAddress.Loopback, 9000);
            m_Connection = m_Driver.Connect(endpoint);

            SendMessage("Connected");

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
                    
                    using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                    {
                        if (messageList.Count < 1)
                            break;
                        writer.Write(messageList[0]);
                        Debug.LogFormat("NetworkClient -- Sending message {0} to server", Encoding.Unicode.GetString(messageList[0]));
                        //Debug.LogFormat("NetworkClient -- Message  is {0} in bytes", BitConverter.ToString(messageList[0]));
                        messageList.RemoveAt(0);

                        m_Connection.Send(m_Driver, writer);
                    }
                }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    var readerCtx = default(DataStreamReader.Context);

                    byte[] messageBytes = new byte[stream.Length];
                    stream.ReadBytesIntoArray(ref readerCtx, ref messageBytes, stream.Length);
                    string recievedMessage = Encoding.Unicode.GetString(messageBytes);
                    Debug.Log("NetworkClient -- Got the value " + recievedMessage + " back from the server");
                    TranslateMessage(recievedMessage);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("NetworkClient -- Client got disconnected from server");
                    m_Connection = default(NetworkConnection);

                    NetworkingManager.Instance.ClientDisconnect();
                }
            }

            //send the first message in the message list

            if (m_playerID == -1 || messageList.Count <= 0)
                return;

            using (var writer = new DataStreamWriter(1024, Allocator.Temp))
            {
                if (messageList.Count < 1)
                    return;
                writer.Write(messageList[0]);
                Debug.LogFormat("NetworkClient -- Sending message {0} to server", Encoding.Unicode.GetString(messageList[0]));
                //Debug.LogFormat("NetworkClient -- Message  is {0} in bytes", BitConverter.ToString(messageList[0]));
                messageList.RemoveAt(0);

                m_Connection.Send(m_Driver, writer);
            }

            if (m_Done)
            {
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
        }

        new public void SendMessage(string message)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            messageList.Add(buffer);
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
                case NetworkMessageContent.ID:
                    IDMessage(recievedMessage);
                    break;
                case NetworkMessageContent.None:
                    break;
            }
        }

        void MoveMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateMoveMessage(recievedMessage, out int playerID, out float x, out float z);
        }

        void PositionMessage(string recievedMessage)
        {
            throw new System.NotImplementedException();
        }

        void IDMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateIDMessage(recievedMessage, out int clientID);
            m_playerID = clientID;
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
                if(m_player = player)
                    Debug.LogWarning("NetworkClient -- AssignPlayer: Tried to assign the local player twice");
                else
                    Debug.LogError("NetworkClient -- AssignPlayer: Tried to assign the local player to a different player! One of these player is incorrect");
            }
        }

    }
}

