using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine.Assertions;

using UdpCNetworkDriver = Unity.Networking.Transport.BasicNetworkDriver<Unity.Networking.Transport.IPv4UDPSocket>;

namespace VRGame.Networking
{

    public class NetworkServer : MonoBehaviour
    {
        public UdpCNetworkDriver m_Driver;
        private NativeList<NetworkConnection> m_Connections;

        List<int> m_PlayerIDs = new List<int>();
        List<string> m_MessageList = new List<string>();

        void Start()
        {
            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);

            if (m_Driver.Bind(new IPEndPoint(IPAddress.Any, 9000)) != 0)
                Debug.Log("NetworkServer -- Failed to bind to port 9000");
            else
                m_Driver.Listen();

            m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);

            if (Debug.isDebugBuild)
                Debug.Log("NetworkServer -- Start: Server created");
        }

        public void OnDestroy()
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }

        void Update()
        {
            m_Driver.ScheduleUpdate().Complete();

            string[] currentMessages = m_MessageList.ToArray();
            m_MessageList.Clear();

            // CleanUpConnections
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (!m_Connections[i].IsCreated)
                {
                    m_Connections.RemoveAtSwapBack(i);
                    --i;
                }
            }
            // AcceptNewConnections
            NetworkConnection c;
            while ((c = m_Driver.Accept()) != default(NetworkConnection))
            {
                m_Connections.Add(c);
                Debug.Log("NetworkServer -- Accepted a connection");
            }

            DataStreamReader stream;
            for (int i = 0; i < m_Connections.Length; i++)
            {
                if (!m_Connections[i].IsCreated)
                    Assert.IsTrue(true);

                NetworkEvent.Type cmd;
                while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) !=
                       NetworkEvent.Type.Empty)
                {
                    if(cmd == NetworkEvent.Type.Connect)
                    {
                        Debug.Log("NetworkServer -- Client has connected");

                        //Send Data

                        using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                        {
                            writer.Write(Encoding.Unicode.GetBytes(NetworkTranslater.CreateIDMessageFromServer(m_Connections[i].InternalId)));
                            m_Driver.Send(m_Connections[i], writer);
                            break;
                        }
                    }

                    if (cmd == NetworkEvent.Type.Data)
                    {
                        var readerCtx = default(DataStreamReader.Context);
                        byte[] messageBytes = new byte[stream.Length];
                        stream.ReadBytesIntoArray(ref readerCtx, ref messageBytes, stream.Length);
                        //Debug.LogFormat("NetworkServer -- message bytes is {0}", BitConverter.ToString(messageBytes));

                        string recievedMessage = Encoding.Unicode.GetString(messageBytes);
                        Debug.Log("NetworkServer -- Got " + recievedMessage + " from the Client.");

                        m_MessageList.Add(recievedMessage);

                        string[] splitMessages = NetworkTranslater.SplitMessages(recievedMessage);

                        foreach(var msg in splitMessages)
                        {
                            if (TranslateMessage(recievedMessage, i) == false)
                                break;
                        }

                    }
                    else if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        Debug.Log("NetworkServer -- Client disconnected from server");
                        m_Connections[i] = default(NetworkConnection);
                    }

                    //send the first message in the list
                    if (m_MessageList.Count <= 0)
                        return;

                    using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                    {
                        string allMessages = NetworkTranslater.CombineMessages(currentMessages);

                        SendMessages(Encoding.Unicode.GetBytes(allMessages), i);
                    }
                }
            }
        }

        void SendMessages(byte[] buffer, int i)
        {
            using (var writer = new DataStreamWriter(1024, Allocator.Temp))
            {
                writer.Write(buffer);

                Debug.LogFormat("NetworkServer -- Sending message {0} to Client", Encoding.Unicode.GetString(buffer));
                //Debug.LogFormat("NetworkServer -- Message  is {0} in bytes", BitConverter.ToString(messageList[0]));

                m_Driver.Send(m_Connections[i], writer);
            }
        }

        public void WriteMessage(string message)
        {
            m_MessageList.Add(message);
        }

        bool TranslateMessage(string recievedMessage, int i)
        {
            NetworkMessageContent msgContent = NetworkTranslater.GetMessageContentType(recievedMessage);

            switch (msgContent)
            {
                case NetworkMessageContent.Move:
                    MoveMessage(recievedMessage);
                    break;
                case NetworkMessageContent.Position:
                    break;
                case NetworkMessageContent.ID:
                    IDMessage(i);
                    return false;
                case NetworkMessageContent.None:
                    break;
            }

            return true;
        }

        void MoveMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateMoveMessage(recievedMessage, out int playerID, out float x, out float z);

            NetworkingManager.Instance.playerDictionary[playerID].RecieveMoveMessage(x, z);

        }

        void PositionMessage(string recievedMessage)
        {

        }

        void IDMessage(int i)
        {
            SendMessages(Encoding.Unicode.GetBytes(NetworkTranslater.CreateIDMessageFromServer(m_Connections[i].InternalId)), i);
        }

    }
}
