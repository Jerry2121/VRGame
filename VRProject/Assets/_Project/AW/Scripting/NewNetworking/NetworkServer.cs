﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine.Assertions;

using UdpCNetworkDriver = Unity.Networking.Transport.BasicNetworkDriver<Unity.Networking.Transport.IPv4UDPSocket>;
using System.Linq;
using System.Net.Sockets;

namespace VRGame.Networking
{

    public class NetworkServer : MonoBehaviour
    {
        public UdpCNetworkDriver m_Driver;
        private NativeList<NetworkConnection> m_Connections;

        List<int> m_PlayerIDs = new List<int>();
        List<string> m_MessageList = new List<string>();

        Dictionary<int, ServerObject> m_Players = new Dictionary<int, ServerObject>();
        Dictionary<int, ServerObject> m_NetworkedObjects = new Dictionary<int, ServerObject>();

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

            List<string> currentMessages = new List<string>(m_MessageList);
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
                if (m_Connections[i].IsCreated == false)
                    continue; //Assert.IsTrue(true);

                NetworkEvent.Type cmd;
                while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) !=
                       NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Connect)
                    {
                        Debug.Log("NetworkServer -- Client has connected");
                    }

                    if (cmd == NetworkEvent.Type.Data)
                    {
                        try
                        {
                            var readerCtx = default(DataStreamReader.Context);
                            int length = stream.Length;
                            //byte[] messageBytes = new byte[stream.Length];
                            //stream.ReadBytesIntoArray(ref readerCtx, ref messageBytes, stream.Length);

                            byte[] messageBytes = stream.ReadBytesAsArray(ref readerCtx, length);
                            
                            //Debug.LogFormat("NetworkServer -- message bytes is {0}", BitConverter.ToString(messageBytes));

                            string recievedMessage = Encoding.UTF8.GetString(messageBytes);

                            //Debug.Log("NetworkServer -- Got " + recievedMessage + " from a Client.");

                            //m_MessageList.Add(recievedMessage);

                            string[] splitMessages = NetworkTranslater.SplitMessages(recievedMessage);

                            foreach (var msg in splitMessages)
                            {
                                if (TranslateMessage(msg, i) == false)
                                    break;
                            }
                        }
                        catch (NullReferenceException)
                        {
                            if (Debug.isDebugBuild)
                                Debug.LogError("NetworkSever -- Update: Caught Null Reference");
                        }

                    }
                    else if (cmd == NetworkEvent.Type.Disconnect)
                    {
                        Debug.Log("NetworkServer -- Client disconnected from server");
                        m_Connections[i] = default(NetworkConnection);
                    }

                    //don't send if there aren't any messages
                    if (currentMessages.Count <= 0)
                        continue;

                    using (var writer = new DataStreamWriter(1024, Allocator.Temp))
                    {
                        string allMessages = NetworkTranslater.CombineMessages(currentMessages);
                        SendMessages(Encoding.UTF8.GetBytes(allMessages), i);
                    }
                }
            }
        }

        void SendMessages(byte[] buffer, int i)
        {
            using (var writer = new DataStreamWriter(1024, Allocator.Temp))
            {
                writer.Write(buffer);

                //Debug.LogFormat("NetworkServer -- Sending message {0} to Client", Encoding.UTF8.GetString(buffer));
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
                    PositionMessage(recievedMessage);
                    break;
                case NetworkMessageContent.ClientID:
                    IDMessage(i);
                    return false;
                case NetworkMessageContent.Instantiate:
                    InstantiateMessage(recievedMessage);
                    break;
                case NetworkMessageContent.Rotation:
                    RotationMessage(recievedMessage);
                    break;
                case NetworkMessageContent.None:
                    break;
                default:
                    break;
            }

            return true;
        }

        void MoveMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateMoveMessage(recievedMessage, out int clientID, out int objectID, out float x, out float z);

            //if (m_Players == null)
            //return;

            //NetworkingManager.Instance.playerDictionary[playerID].RecieveMoveMessage(x, z);

            WriteMessage(recievedMessage);
        }

        void PositionMessage(string recievedMessage)
        {
            Debug.Log(string.Format("NetworkServer -- PositionMessage"));

            NetworkTranslater.TranslatePositionMessage(recievedMessage, out int clientID, out int objectID, out float x, out float y, out float z);

            m_NetworkedObjects[objectID].SetPosition(x, y, z);
            
            //m_Players[clientID].SetPosition(x, y, z);

            WriteMessage(recievedMessage);
        }

        void RotationMessage(string recievedMessage)
        {
            Debug.Log(string.Format("NetworkServer -- RotationMessage"));

            NetworkTranslater.TranslateRotationMessage(recievedMessage, out int clientID, out int objectID, out float x, out float y, out float z, out float w);

            m_NetworkedObjects[objectID].SetRotation(x, y, z, w);

            //m_Players[clientID].SetPosition(x, y, z);

            WriteMessage(recievedMessage);
        }

        void IDMessage(int i)
        {
            Debug.Log(string.Format("NetworkServer -- IDMessage: Got ID Message from connection {0}", i));

            int ID = -1;

            if (m_Connections[i] != null && m_Connections[i] != default(NetworkConnection))
                ID = m_Connections[i].InternalId + 1;

            if (m_NetworkedObjects.Count > 0)
            {
                List<string> messages = new List<string>();

                messages.Add(NetworkTranslater.CreateIDMessageFromServer(ID));

                /*foreach (var playerID in m_Players.Keys)
                {
                    ServerObject player = m_Players[playerID];
                    messages.Add(NetworkTranslater.CreateInstantiateMessage(playerID, playerID, player.objectType, player.m_Position));
                }*/

                foreach (var objectID in m_NetworkedObjects.Keys)
                {
                    ServerObject networkedObject = m_NetworkedObjects[objectID];
                    messages.Add(NetworkTranslater.CreateInstantiateMessage(networkedObject.m_clientID, objectID, networkedObject.m_objectType, networkedObject.m_Position));
                }

                SendMessages(Encoding.UTF8.GetBytes(NetworkTranslater.CombineMessages(messages)), i);
            }
            else
                SendMessages(Encoding.UTF8.GetBytes(NetworkTranslater.CreateIDMessageFromServer(ID)), i);

            /*if (m_Players.ContainsKey(ID) == false)
            {
                //m_Players.Add(ID, new ServerObject("Player"));

                m_PlayerIDs.Add(ID);
            }*/
        }

        void InstantiateMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateInstantiateMessage(recievedMessage, out int clientID, out int objectID, out string objectType,  out float x, out float y, out float z);
            
            //if (objectName == "Player")
            //return; //Players are setup when we get an ID message from the client

            if (objectID != -1)
                return;

            objectID = m_NetworkedObjects.Count /*+ 101*/;

            m_NetworkedObjects.Add(objectID, new ServerObject(clientID, objectType, x, y, z));
            
            
            WriteMessage(NetworkTranslater.CreateInstantiateMessage(clientID, objectID, objectType, x, y, z));
        }

        public string ServerIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

    }
}
