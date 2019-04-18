//using System;
//using System.Net;
//using System.Collections.Generic;
//using System.Text;
//using Unity.Collections;
//using UnityEngine;
//using Unity.Networking.Transport;
//using Unity.Jobs;

//using UdpCNetworkDriver = Unity.Networking.Transport.BasicNetworkDriver<Unity.Networking.Transport.IPv4UDPSocket>;

//namespace VRGame.Networking
//{
//    class NetworkClientJobs : NetworkingClient
//    {

//        TempPlayer m_player;

//        public UdpCNetworkDriver m_Driver;
//        public NativeArray<NetworkConnection> m_Connection;
//        public NativeArray<int> m_clientID;
//        public NativeArray<byte> m_Done;

//        public JobHandle m_ClientJobHandle;

//        List<string> m_messageList = new List<string>();


//        void Start()
//        {
//            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
//            m_Connection = new NativeArray<NetworkConnection>(1, Allocator.Persistent);
//            m_clientID = new NativeArray<int>(1, Allocator.Persistent);
//            m_Done = new NativeArray<byte>(1, Allocator.Persistent);

//            if (NetworkingManager.Instance.NetworkAddress() == null)
//            {
//                Debug.Log("NetworkClient -- Start: Invalid IP address");
//                Disconnect();
//                NetworkingManager.Instance.Disconnect();
//                return;
//            }

//            var endpoint = new IPEndPoint(NetworkingManager.Instance.NetworkAddress(), 9000);
//            m_Connection[0] = m_Driver.Connect(endpoint);

//            m_clientID[0] = -1;

//            if (Debug.isDebugBuild)
//                Debug.Log("NetworkClient -- Start: Client created");
//        }


//        void Update()
//        {
//            m_ClientJobHandle.Complete();

//            var job = new ClientUpdateJob
//            {
//                driver = m_Driver,
//                connection = m_Connection,
//                clientID = m_clientID,
//                done = m_Done
//            };

//            m_ClientJobHandle = m_Driver.ScheduleUpdate();
//            m_ClientJobHandle = job.Schedule(m_ClientJobHandle);

//            if (job.sendMessages[0] == 1)
//            {
//                //send the first message in the message list
//                try
//                {
//                    if (m_clientID[0] == -1)
//                    {
//                        string IDRequest = NetworkTranslater.CreateIDMessageFromClient();

//                        Debug.Log("NetworkClient -- Update: Sending ID Request");

//                        SendMessages(Encoding.UTF8.GetBytes(IDRequest));
//                        return;
//                    }
//                }
//                catch (InvalidOperationException)
//                {
//                    if (Debug.isDebugBuild)
//                        Debug.LogError("NetworkClient -- Update: InvalidOperationException caught. Did not send ID request");
//                    return;
//                }

//                if (m_messageList.Count <= 0)
//                {
//                    SendMessages(Encoding.UTF8.GetBytes(string.Empty));
//                }

//                else
//                {
//                    string allMessages = NetworkTranslater.CombineMessages(m_messageList);
//                    m_messageList.Clear();
//                    SendMessages(Encoding.UTF8.GetBytes(allMessages));
//                }

//            }



//            if (m_Done[0] == 1)
//            {
//                m_Connection[0].Disconnect(m_Driver);
//                m_Connection[0] = default(NetworkConnection);
//            }


//        }


//        public override void WriteMessage(string message)
//        {
//            m_messageList.Add(message);
//        }
        
//        void SendMessages(byte[] buffer)
//        {
//            using (var writer = new DataStreamWriter(1024, Allocator.Temp))
//            {
//                writer.Write(buffer);

//                //Debug.LogFormat("NetworkClient -- Sending message {0} to server", Encoding.UTF8.GetString(buffer));
//                //Debug.LogFormat("NetworkClient -- Message  is {0} in bytes", BitConverter.ToString(messageList[0]));

//                m_Connection[0].Send(m_Driver, writer);
//            }
//        }

//        public override void Disconnect()
//        {
//            m_Done[0] = 1;
//        }

//        public void AssignPlayer(TempPlayer player)
//        {
//            if (m_player == null)
//            {
//                m_player = player;
//            }
//            else
//            {
//                if (m_player == player)
//                    Debug.LogWarning("NetworkClient -- AssignPlayer: Tried to assign the local player twice");
//                else
//                    Debug.LogError("NetworkClient -- AssignPlayer: Tried to assign the local player to a different player! One of these players is incorrect");
//            }
//        }

//        public void OnDestroy()
//        {
//            m_ClientJobHandle.Complete();

//            m_Connection.Dispose();
//            m_Driver.Dispose();
//            m_clientID.Dispose();
//            m_Done.Dispose();
//        }

//        public override int ClientID()
//        {
//            return m_clientID[0];
//        }

//    }

//    struct ClientUpdateJob : IJob
//    {
//        public UdpCNetworkDriver driver;
//        public NativeArray<NetworkConnection> connection;
//        public NativeArray<int> clientID;
//        public NativeArray<byte> done;
//        public NativeArray<byte> sendMessages;

//        public void Execute()
//        {
//            if (connection[0].IsCreated == false)
//            {
//                if (done[0] != 1 && Debug.isDebugBuild)
//                    Debug.Log("NetworkClient -- Update: Something went wrong during connect");
//                return;
//            }

//            DataStreamReader stream;
//            NetworkEvent.Type cmd;

//            sendMessages[0] = 1;

//            while ((cmd = connection[0].PopEvent(driver, out stream)) !=
//                   NetworkEvent.Type.Empty)
//            {
//                if (cmd == NetworkEvent.Type.Connect)
//                {
//                    if (Debug.isDebugBuild)
//                        Debug.Log("NetworkClient -- Update: We are now connected to the server");
//                }
//                else if (cmd == NetworkEvent.Type.Data)
//                {
//                    var readerCtx = default(DataStreamReader.Context);

//                    try
//                    {
//                        byte[] messageBytes = new byte[stream.Length];
//                        stream.ReadBytesIntoArray(ref readerCtx, ref messageBytes, stream.Length);
//                        string recievedMessage = Encoding.UTF8.GetString(messageBytes);

//                        //Debug.Log("NetworkClient -- Got the value " + recievedMessage + " from the server");

//                        string[] splitMessages = NetworkTranslater.SplitMessages(recievedMessage);

//                        foreach (var msg in splitMessages)
//                        {
//                            TranslateMessage(msg);
//                        }
//                    }
//                    catch (NullReferenceException)
//                    {
//                        if (Debug.isDebugBuild)
//                            Debug.LogError("NetworkClient -- Update: Caught Null Reference");
//                    }
//                }
//                else if (cmd == NetworkEvent.Type.Disconnect)
//                {
//                    Debug.Log("NetworkClient -- Update: Client got disconnected from server");
//                    connection[0] = default(NetworkConnection);

//                    NetworkingManager.Instance.Disconnect();
//                }
//            }
            
//        }

        
//        void TranslateMessage(string recievedMessage)
//        {
//            NetworkMessageContent msgContent = NetworkTranslater.GetMessageContentType(recievedMessage);

//            switch (msgContent)
//            {
//                case NetworkMessageContent.Move:
//                    MoveMessage(recievedMessage);
//                    break;
//                case NetworkMessageContent.Position:
//                    PositionMessage(recievedMessage);
//                    break;
//                case NetworkMessageContent.ClientID:
//                    IDMessage(recievedMessage);
//                    break;
//                case NetworkMessageContent.Instantiate:
//                    InstantiateMessage(recievedMessage);
//                    break;
//                case NetworkMessageContent.None:
//                    break;
//            }
//        }

//        void MoveMessage(string recievedMessage)
//        {
//            if (NetworkTranslater.TranslateMoveMessage(recievedMessage, out int clientID, out int objectID, out float x, out float z) == false)
//                return;

//            throw new NotImplementedException();
//        }

//        void PositionMessage(string recievedMessage)
//        {
//            if (NetworkTranslater.GetIDsFromMessage(recievedMessage, out int originClientID, out int objectID))
//                return;

//            if (originClientID != clientID[0]) //Make sure we aren't getting out own positions back
//            {
//                NetworkingManager.Instance.networkedObjectDictionary[objectID].RecieveMessage(recievedMessage, NetworkMessageContent.Position);
//            }
//        }

//        void IDMessage(string recievedMessage)
//        {
//            if (NetworkTranslater.TranslateIDMessage(recievedMessage, out int newClientID) == false)
//                return;

//            if (NetworkingManager.Instance.playerDictionary.ContainsKey(newClientID) || newClientID == -1)
//                return;

//            if (clientID[0] != -1) //The message is for someone else
//            {
//                //NetworkingManager.Instance.playerDictionary.Add(newClientID, null);
//                return;
//            }

//            NetworkingManager.Instance.playerDictionary.Add(newClientID, null);

//            clientID[0] = newClientID;

//            Debug.Log(string.Format("NetworkClient -- IDMessage: Recieved ID of {0} from the server", newClientID));

//            NetworkingManager.Instance.InstantiateOverNetwork("Player", Vector3.zero);
//        }

//        void InstantiateMessage(string recievedMessage)
//        {
//            NetworkingManager.Instance.RecieveInstantiateMessage(recievedMessage);
//        }
//    }
//}