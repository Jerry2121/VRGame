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

        List<byte[]> messageList = new List<byte[]>();

        void Start()
        {
            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
            if (m_Driver.Bind(new IPEndPoint(IPAddress.Loopback, 9000)) != 0)
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

                        //tell the client its ID
                        GameObject temp = Instantiate(NetworkingManager.Instance.playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        int ID = NetworkingManager.Instance.playerDictionary.Keys.Count;
                        NetworkingManager.Instance.playerDictionary.Add(ID, temp.GetComponent<TempPlayer>());

                        //Send Data

                        using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                        {
                            writer.Write(Encoding.Unicode.GetBytes(NetworkTranslater.CreateIDMessage(ID)));
                            m_Driver.Send(m_Connections[i], writer);
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
                        TranslateMessage(recievedMessage);

                        //Send Data

                        using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                        {
                            writer.Write(Encoding.Unicode.GetBytes("Recieved Data"));
                            m_Driver.Send(m_Connections[i], writer);
                        }
                    }
                    else if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        Debug.Log("NetworkServer -- Client disconnected from server");
                        m_Connections[i] = default(NetworkConnection);
                    }

                    //send the first message in the list
                    if (messageList.Count <= 0)
                        return;

                    using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                    {
                        writer.Write(messageList[0]);
                        m_Driver.Send(m_Connections[i], writer);
                    }
                }
            }
        }

        new void SendMessage(string message)
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
                    break;
                case NetworkMessageContent.ID:
                    break;
                case NetworkMessageContent.None:
                    break;
            }
        }

        void MoveMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateMoveMessage(recievedMessage, out int playerID, out float x, out float z);

            NetworkingManager.Instance.playerDictionary[playerID].RecieveMoveMessage(x, z);

        }

        void PositionMessage(string recievedMessage)
        {

        }

    }
}
