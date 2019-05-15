using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace VRGame.Networking
{

    public enum NetworkMessageContent
    {
        None,
        Move,           // Mov
        Position,       // Pos
        ClientID,       // ID
        Instantiate,    // Ins
        Rotation,       // Rot
        LoadedIn,       // LIN
        PuzzleStarted,  // PuS
        PuzzleProgress, // PuP
        PuzzleComplete, // PuC
        Disconnected,   // Dco
    }

    public static class NetworkTranslater
    {

        //Messages go clientID|ObjectID|contentType|data|data|data... ect
        //Messages connected Message1-Message2

        //ID of 0 is from the server
        //IDs 1-100 are clients/players

        public static NetworkMessageContent GetMessageContentType(string message)
        {
            string[] splitMessage = message.Split('|');

            if (splitMessage.Length <= 1)
                return NetworkMessageContent.None;

            switch (splitMessage[2])
            {
                case "Mov":
                    return NetworkMessageContent.Move;          // ClientID|ObjectID|Mov|ComponentID|xMove|zMove

                case "Pos":
                    return NetworkMessageContent.Position;      // ClientID|ObjectID|Pos|ComponentID|xPositon|yPosition|zPosition

                case "ID":
                    return NetworkMessageContent.ClientID;      // ID||ID    This is used for setting clientIDs

                case "Ins":
                    return NetworkMessageContent.Instantiate;   // ClientID|ObjectID|Ins|ObjectName|xPosition|yPosition|zPosition

                case "Rot":
                    return NetworkMessageContent.Rotation;      // ClientID|ObjectID|Rot|ComponentID|xRotation|yRotation|zRotation

                case "LIN":
                    return NetworkMessageContent.LoadedIn;      // ClientID||LIN

                case "PuS":
                    return NetworkMessageContent.PuzzleStarted; //ClientID|ObjectID|PuS

                case "PuP":
                    return NetworkMessageContent.PuzzleProgress;//ClientID|ObjectID|PuP|ComponentID|NumOne

                case "PuC":                                     
                    return NetworkMessageContent.PuzzleComplete;// ClientID|ObjectID|PuC

                case "Dco":                                     
                    return NetworkMessageContent.Disconnected;  // ClientID||Dco

                default:
                    return NetworkMessageContent.None;
            }
        }

        public static NetworkMessageContent GetMessageContentType(string[] message)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var msg in message)
            {
                sb.Append(msg);
            }
            return GetMessageContentType(sb.ToString());
        }


        public static string CombineMessages(string[] messages)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var message in messages)
            {
                if (message.Equals(string.Empty))
                    continue;
                if(message == messages[messages.Length - 1])
                    sb.Append(message);
                else
                    sb.Append(message + ":");
            }

            return sb.ToString();
        }

        public static string CombineMessages(List<string> messages)
        {
            return CombineMessages(messages.ToArray());

            //StringBuilder sb = new StringBuilder();

            //foreach (var message in messages)
            //{
            //    if (message.Equals(string.Empty))
            //        continue;
            //    sb.Append(message + ":");
            //}

            //return sb.ToString();
        }

        public static string[] SplitMessages(string recievedMessage)
        {
            return recievedMessage.Split(':');
        }

        public static bool GetIDsFromMessage(string recievedMessage, out int clientID, out int objectID)
        {
            clientID = objectID = -1;

            if (GetMessageContentType(recievedMessage) == NetworkMessageContent.ClientID)
                return false;

            string[] splitMessage = recievedMessage.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) &&
                int.TryParse(splitMessage[1], out objectID))
            {
                return true;
            }

            else return false;
        }

        public static bool GetIDsFromMessage(string recievedMessage, out int clientID, out int objectID, out int componentID)
        {
            clientID = objectID = componentID = -1;

            if (GetMessageContentType(recievedMessage) == NetworkMessageContent.ClientID)
                return false;

            string[] splitMessage = recievedMessage.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) && 
                int.TryParse(splitMessage[1], out objectID) && 
                int.TryParse(splitMessage[3], out componentID))
            {
                return true;
            }

            else return false;
        }

        #region TranslateMessages

        public static bool TranslateMoveMessage(string message, out int clientID, out int objectID, out int componentID, out float x, out float z)
        {
            x = z = 0;
            clientID = objectID  =componentID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Move)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) && 
                int.TryParse(splitMessage[1], out objectID) &&
                int.TryParse(splitMessage[3], out componentID)&&
                float.TryParse(splitMessage[4], out x) && 
                float.TryParse(splitMessage[5], out z))
            {
                return true;
            }

            return false;
        }

        public static bool TranslatePositionMessage(string message, out int clientID, out int objectID, out int componentID, out float x, out float y, out float z)
        {
            x = y = z = 0;
            clientID = objectID = componentID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Position)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) && 
                int.TryParse(splitMessage[1], out objectID) &&
                int.TryParse(splitMessage[3], out componentID) &&
                float.TryParse(splitMessage[4], out x) && 
                float.TryParse(splitMessage[5], out y) && 
                float.TryParse(splitMessage[6], out z))
            {
                return true;
            }

            return false;
        }

        public static bool TranslateRotationMessage(string message, out int clientID, out int objectID, out int componentID, out float x, out float y, out float z, out float w)
        {
            x = y = z = w = 0;
            clientID = objectID = componentID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Rotation)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) &&
                int.TryParse(splitMessage[1], out objectID) &&
                int.TryParse(splitMessage[3], out componentID) &&
                float.TryParse(splitMessage[4], out x) &&
                float.TryParse(splitMessage[5], out y) &&
                float.TryParse(splitMessage[6], out z) &&
                float.TryParse(splitMessage[7], out w))
            {
                return true;
            }

            return false;
        }

        public static bool TranslateIDMessage(string message, out int clientID)
        {
            if (GetMessageContentType(message) != NetworkMessageContent.ClientID)
            {
                Debug.LogError("NOOOOOOO");
                clientID = -1;
                return false;
            }

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID))
                return true;


            return false;
        }

        public static bool TranslateLoadedInMessage(string message, out int clientID)
        {
            if (GetMessageContentType(message) != NetworkMessageContent.LoadedIn)
            {
                Debug.LogError("NOOOOOOO");
                clientID = -1;
                return false;
            }

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID))
                return true;


            return false;
        }

        public static bool TranslateInstantiateMessage(string message, out int clientID, out int objectID, out string objectName, out float posX, out float posY, out float posZ, out float rotX, out float rotY, out float rotZ, out float rotW)
        {
            posX = posY = posZ = rotX = rotY = rotZ = rotW = 0;
            objectName = string.Empty;
            clientID = objectID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Instantiate)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) &&
                int.TryParse(splitMessage[1], out objectID) &&
                float.TryParse(splitMessage[4], out posX) &&
                float.TryParse(splitMessage[5], out posY) &&
                float.TryParse(splitMessage[6], out posZ) &&
                float.TryParse(splitMessage[7], out rotX) &&
                float.TryParse(splitMessage[8], out rotY) &&
                float.TryParse(splitMessage[9], out rotZ) &&
                float.TryParse(splitMessage[10], out rotW))
            {
                objectName = splitMessage[3];
                return true;
            }

            return false;
        }

        public static bool TranslateDisconnectedMessage(string message, out int clientID)
        {
            if (GetMessageContentType(message) != NetworkMessageContent.Disconnected)
            {
                Debug.LogError("NOOOOOOO");
                clientID = -1;
                return false;
            }

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID))
                return true;


            return false;
        }

        public static bool TranslatePuzzleStartedMessage(string message, out int clientID, out int objectID, out int componentID)
        {
            clientID = objectID = componentID = -1;
            if(GetMessageContentType(message) != NetworkMessageContent.PuzzleStarted)
            {
                Debug.LogError("NOOOOOOO");
                return false;
            }

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) &&
                int.TryParse(splitMessage[1], out objectID) &&
                int.TryParse(splitMessage[3], out componentID))
            {
                return true;
            }

            return false;
        }

        public static bool TranslatePuzzleProgressMessage(string message, out int clientID, out int objectID, out int componentID, out int numOne)
        {
            clientID = objectID = componentID = numOne = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.PuzzleProgress)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) &&
                int.TryParse(splitMessage[1], out objectID) &&
                int.TryParse(splitMessage[3], out componentID) &&
                int.TryParse(splitMessage[4], out numOne))
            {
                return true;
            }

            return false;
        }

        public static bool TranslatePuzzleCompleteMessage(string message, out int clientID, out int objectID, out int componentID)
        {
            clientID = objectID = componentID = -1;
            if (GetMessageContentType(message) != NetworkMessageContent.PuzzleComplete)
            {
                Debug.LogError("NOOOOOOO");
                return false;
            }

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) &&
                int.TryParse(splitMessage[1], out objectID) &&
                int.TryParse(splitMessage[3], out componentID))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region CreateMessage

        public static string CreateMoveMessage(int clientID, int objectID, int componentID, float x, float z)
        {
            return string.Format("{0}|{1}|Mov|{2}|{3}|{4}", clientID, objectID, componentID, x, z);
        }

        public static string CreatePositionMessage(int clientID, int objectID, int componentID, float x, float y, float z)
        {
            return string.Format("{0}|{1}|Pos|{2}|{3}|{4}|{5}", clientID, objectID, componentID, x, y, z);
        }

        public static string CreatePositionMessage(int clientID, int objectID, int componentID, float3 position)
        {
            return CreatePositionMessage(clientID, objectID, componentID, position.x, position.y, position.z);
        }

        public static string CreateRotationMessage(int clientID, int objectID, int componentID, float x, float y, float z, float w)
        {
            return string.Format("{0}|{1}|Rot|{2}|{3}|{4}|{5}|{6}", clientID, objectID, componentID, x, y, z, w);
        }

        public static string CreateRotationMessage(int clientID, int objectID, int componentID, float4 rotation)
        {
            return CreateRotationMessage(clientID, objectID, componentID, rotation.x, rotation.y, rotation.z, rotation.w);
        }

        public static string CreateIDMessageFromServer(int clientID)
        {
            return string.Format("{0}||ID", clientID);
        }

        public static string CreateIDMessageFromClient()
        {
            return string.Format("-1||ID");
        }

        public static string CreateInstantiateMessage(int clientID, int objectID, string objectName, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float rotW)
        {
            return string.Format("{0}|{1}|Ins|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", clientID, objectID, objectName, posX, posY, posZ, rotX, rotY, rotZ, rotW);
        }

        public static string CreateInstantiateMessage(int clientID, int objectID, string objectName, float3 position, float4 rotation)
        {
            return CreateInstantiateMessage(clientID, objectID, objectName, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, rotation.w);
        }

        public static string CreateLoadedInMessage(int clientID)
        {
            return string.Format("{0}||LIN", clientID);
        }

        public static string CreateDisconnectedMessage(int clientID)
        {
            return string.Format("{0}||Dco", clientID);
        }

        public static string CreatePuzzleStartedMessage(int clientID, int objectID, int componentID)
        {
            return string.Format("{0}|{1}|PuS|{2}", clientID, objectID, componentID);
        }

        public static string CreatePuzzleProgressMessage(int clientID, int objectID, int componentID, int numOne)
        {
            return string.Format("{0}|{1}|PuP|{2}|{3}", clientID, objectID, componentID, numOne);
        }
        public static string CreatePuzzleCompleteMessage(int v, int objectID, int m_ComponentID)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
