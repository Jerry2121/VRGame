using System;
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
        Dictionary<int, ServerPuzzle> m_Puzzles = new Dictionary<int, ServerPuzzle>();

        void Start()
        {
            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);

            if (m_Driver.Bind(new IPEndPoint(IPAddress.Any, 9000)) != 0)
                Debug.LogError("NetworkServer -- Failed to bind to port 9000");
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
                        Debug.Log("NetworkServer -- A Client has connected");
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
                        Debug.Log("NetworkServer --A  Client has disconnected from server");
                        m_MessageList.Add(NetworkTranslater.CreateDisconnectedMessage(m_Connections[i].InternalId + 1));

                        ServerObject playerObject = m_Players[(m_Connections[i].InternalId + 1)];

                        m_Players.Remove(m_Connections[i].InternalId + 1);
                        m_NetworkedObjects.Remove(playerObject.m_ObjectID);

                        m_Connections[i] = default(NetworkConnection);
                    }

                    //don't send if there aren't any messages
                    if (currentMessages.Count <= 0)
                    {
                        SendMessages(Encoding.UTF8.GetBytes("null"), i);
                        continue;
                    }

                    string allMessages = NetworkTranslater.CombineMessages(currentMessages);
                    SendMessages(Encoding.UTF8.GetBytes(allMessages), i);
                }
            }
        }

        void SendMessages(byte[] buffer, int i)
        {
            using (var writer = new DataStreamWriter(1024 * 128, Allocator.Temp))
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
                case NetworkMessageContent.Rotation:
                    RotationMessage(recievedMessage);
                    break;
                case NetworkMessageContent.ClientID:
                    IDMessage(i);
                    return false;
                case NetworkMessageContent.Instantiate:
                    InstantiateMessage(recievedMessage);
                    break;
                case NetworkMessageContent.LoadedIn:
                    LoadedInMessage(recievedMessage, i);
                    break;
                case NetworkMessageContent.PuzzleStarted:
                    PuzzleStartedMessage(recievedMessage);
                    break;
                case NetworkMessageContent.PuzzleProgress:
                    PuzzleProgressMessage(recievedMessage);
                    break;
                case NetworkMessageContent.PuzzleFailed:
                    PuzzleFailedMessage(recievedMessage);
                    break;
                case NetworkMessageContent.PuzzleComplete:
                    PuzzleCompleteMessage(recievedMessage);
                    break;
                case NetworkMessageContent.None:
                    break;
                default:
                    WriteMessage(recievedMessage);
                    break;
            }

            return true;
        }

        void MoveMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateMoveMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out float x, out float z);

            WriteMessage(recievedMessage);
        }

        void PositionMessage(string recievedMessage)
        {
            //Debug.Log(string.Format("NetworkServer -- PositionMessage"));

            NetworkTranslater.TranslatePositionMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out float x, out float y, out float z);

            if (componentID < 10) //The root gameobject's component should always have ID of < 10
                m_NetworkedObjects[objectID].SetPosition(x, y, z);

            //m_Players[clientID].SetPosition(x, y, z);

            WriteMessage(recievedMessage);
        }

        void RotationMessage(string recievedMessage)
        {
            //Debug.Log(string.Format("NetworkServer -- RotationMessage"));

            NetworkTranslater.TranslateRotationMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out float x, out float y, out float z, out float w);

            if (componentID < 10) //The root gameobject's component should always have ID of < 10
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


            SendMessages(Encoding.UTF8.GetBytes(NetworkTranslater.CreateIDMessageFromServer(ID)), i);

            /*if (m_Players.ContainsKey(ID) == false)
            {
                //m_Players.Add(ID, new ServerObject("Player"));

                m_PlayerIDs.Add(ID);
            }*/
        }

        void LoadedInMessage(string recievedMessage, int i)
        {
            if (NetworkTranslater.TranslateLoadedInMessage(recievedMessage, out int clientID) == false)
                return;

            if (Debug.isDebugBuild)
                Debug.Log(string.Format("NetworkServer -- LoadedInMessage: Recieved from client {0}", clientID));

            if (m_NetworkedObjects.Count > 0)
            {
                List<string> messages = new List<string>();

                /*foreach (var playerID in m_Players.Keys)
                {
                    ServerObject player = m_Players[playerID];
                    messages.Add(NetworkTranslater.CreateInstantiateMessage(playerID, playerID, player.objectType, player.m_Position));
                }*/

                foreach (var objectID in m_NetworkedObjects.Keys)
                {
                    ServerObject networkedObject = m_NetworkedObjects[objectID];
                    messages.Add(NetworkTranslater.CreateInstantiateMessage(networkedObject.m_ClientID, objectID, networkedObject.m_ObjectType, networkedObject.m_Position, networkedObject.m_Rotation.value));
                    if (messages.Count > 10)
                    {
                        SendMessages(Encoding.UTF8.GetBytes(NetworkTranslater.CombineMessages(messages)), i);
                        messages.Clear();
                    }
                }

                foreach (var objectID in m_Puzzles.Keys)
                {
                    ServerPuzzle puzzle = m_Puzzles[objectID];
                    if (puzzle.m_Started)
                        messages.Add(NetworkTranslater.CreatePuzzleStartedMessage(0, objectID, puzzle.m_ComponentID));
                    if (puzzle.m_Progress != -1)
                    {
                        messages.Add(NetworkTranslater.CreatePuzzleProgressMessage(0, objectID, puzzle.m_ComponentID, puzzle.m_Progress));
                    }
                    if (puzzle.m_Complete)
                        messages.Add(NetworkTranslater.CreatePuzzleCompleteMessage(0, objectID, puzzle.m_ComponentID));
                }

                SendMessages(Encoding.UTF8.GetBytes(NetworkTranslater.CombineMessages(messages)), i);
            }
        }

        void InstantiateMessage(string recievedMessage)
        {
            NetworkTranslater.TranslateInstantiateMessage(recievedMessage, out int clientID, out int objectID, out string objectType, out float posX, out float posY, out float posZ, out float rotX, out float rotY, out float rotZ, out float rotW);

            //if (objectName == "Player")
            //return; //Players are setup when we get an ID message from the client

            if (objectID != -1)
                return;

            objectID = m_NetworkedObjects.Count /*+ 101*/;

            ServerObject serverObj = new ServerObject(clientID, objectID, objectType, posX, posY, posZ, rotX, rotY, rotZ, rotW);

            m_NetworkedObjects.Add(objectID, serverObj);

            if (objectType == "Player")
                if (m_Players.ContainsKey(clientID) == false)
                    m_Players.Add(clientID, serverObj);
                else
                    Debug.LogError("NetworkServer -- InstantiateMessage: Got an Instantiate Message for a player already in the dictionary");

            WriteMessage(NetworkTranslater.CreateInstantiateMessage(clientID, objectID, objectType, posX, posY, posZ, rotX, rotY, rotZ, rotW));
        }

        void PuzzleStartedMessage(string recievedMessage)
        {
            WriteMessage(recievedMessage);

            if (NetworkTranslater.TranslatePuzzleStartedMessage(recievedMessage, out int clientID, out int objectID, out int componentID))
            {
                if (m_Puzzles.ContainsKey(objectID) == false)
                {
                    ServerPuzzle puzzle = new ServerPuzzle(objectID, componentID);
                    puzzle.m_Started = true;
                    m_Puzzles.Add(objectID, puzzle);
                }
                else
                    m_Puzzles[objectID].m_Started = true;
            }

        }

        private void PuzzleProgressMessage(string recievedMessage)
        {
            WriteMessage(recievedMessage);

            if (NetworkTranslater.TranslatePuzzleProgressMessage(recievedMessage, out int clientID, out int objectID, out int componentID, out int numOne))
            {
                if (m_Puzzles.ContainsKey(objectID) == false)
                {
                    ServerPuzzle puzzle = new ServerPuzzle(objectID, componentID);
                    puzzle.m_Progress = numOne;
                    m_Puzzles.Add(objectID, puzzle);
                }
                else
                    m_Puzzles[objectID].m_Progress = numOne;
            }
        }

        void PuzzleFailedMessage(string recievedMessage)
        {
            WriteMessage(recievedMessage);
        }
        private void PuzzleCompleteMessage(string recievedMessage)
        {
            WriteMessage(recievedMessage);

            if (NetworkTranslater.TranslatePuzzleCompleteMessage(recievedMessage, out int clientID, out int objectID, out int componentID))
            {
                if (m_Puzzles.ContainsKey(objectID) == false)
                {
                    ServerPuzzle puzzle = new ServerPuzzle(objectID, componentID);
                    puzzle.m_Complete = true;
                    m_Puzzles.Add(objectID, puzzle);
                }
                else
                    m_Puzzles[objectID].m_Complete = true;
            }
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
