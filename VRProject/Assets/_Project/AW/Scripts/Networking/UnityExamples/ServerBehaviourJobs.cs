﻿//using System.Net;
//using UnityEngine;

//using Unity.Networking.Transport;
//using Unity.Collections;
//using UnityEngine.Assertions;
//using Unity.Jobs;

//using UdpCNetworkDriver = Unity.Networking.Transport.BasicNetworkDriver<Unity.Networking.Transport.IPv4UDPSocket>;

//namespace UnityExamples
//{

//    public class ServerBehaviourJobs : MonoBehaviour
//    {
//        public UdpCNetworkDriver m_Driver;
//        public NativeList<NetworkConnection> m_Connections;
//        private JobHandle ServerJobHandle;


//        void Start()
//        {
//            m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
//            if (m_Driver.Bind(new IPEndPoint(IPAddress.Any, 9000)) != 0)
//                Debug.Log("Failed to bind to port 9000");
//            else
//                m_Driver.Listen();

//            m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
//        }

//        void Update()
//        {
//            ServerJobHandle.Complete();

//            var connectionJob = new ServerUpdateConnectionsJob
//            {
//                driver = m_Driver,
//                connections = m_Connections
//            };

//            var serverUpdateJob = new ServerUpdateJob
//            {
//                driver = m_Driver.ToConcurrent(),
//                connections = m_Connections.ToDeferredJobArray()
//            };

//            ServerJobHandle = m_Driver.ScheduleUpdate();
//            ServerJobHandle = connectionJob.Schedule(ServerJobHandle);
//            ServerJobHandle = serverUpdateJob.Schedule(m_Connections, 1, ServerJobHandle);
//        }

//        public void OnDestroy()
//        {
//            ServerJobHandle.Complete();

//            m_Driver.Dispose();
//            m_Connections.Dispose();
//        }
//    }

//    struct ServerUpdateConnectionsJob : IJob
//    {
//        public UdpCNetworkDriver driver;
//        public NativeList<NetworkConnection> connections;

//        public void Execute()
//        {
//            //Clean up connections
//            for (int i = 0; i < connections.Length; i++)
//            {
//                if (connections[i].IsCreated == false)
//                {
//                    connections.RemoveAtSwapBack(i);
//                    --i;
//                }
//            }

//            //Accept new connections
//            NetworkConnection c;
//            while ((c = driver.Accept()) != default(NetworkConnection))
//            {
//                connections.Add(c);
//                Debug.Log("Accepted a connection");
//            }
//        }
//    }

//    struct ServerUpdateJob : IJobParallelFor
//    {
//        public UdpCNetworkDriver.Concurrent driver;
//        public NativeArray<NetworkConnection> connections;


//        public void Execute(int index)
//        {
//            DataStreamReader stream;
//            if (connections[index].IsCreated == false)
//                Assert.IsTrue(true);

//            NetworkEvent.Type cmd;
//            while ((cmd = driver.PopEventForConnection(connections[index], out stream)) != NetworkEvent.Type.Empty)
//            {
//                if (cmd == NetworkEvent.Type.Data)
//                {
//                    var readerCtx = default(DataStreamReader.Context);
//                    uint number = stream.ReadUInt(ref readerCtx);

//                    Debug.Log("Got " + number + " from the client. Addit + 2 to it");
//                    number += 2;

//                    using (var writer = new DataStreamWriter(4, Allocator.Temp))
//                    {
//                        writer.Write(number);
//                        driver.Send(connections[index], writer);
//                    }
//                }
//                else if (cmd == NetworkEvent.Type.Disconnect)
//                {
//                    Debug.Log("Client Disconnected");
//                    connections[index] = default(NetworkConnection);
//                }
//            }

//        }
//    }

//}