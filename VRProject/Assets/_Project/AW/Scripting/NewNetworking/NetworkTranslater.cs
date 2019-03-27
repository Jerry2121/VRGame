using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VRGame.Networking
{

    public enum NetworkMessageContent
    {
        None,
        Move,           // Mov
        Position,       // Pos
        ID,             // ID
        Instantiate     // Ins
    }

    public static class NetworkTranslater
    {

        //Messages go playerID|contentType|data|data|data... ect
        //Messages connected Message1-Message2

        public static NetworkMessageContent GetMessageContentType(string message)
        {
            string[] splitMessage = message.Split('|');

            if (splitMessage.Length <= 1)
                return NetworkMessageContent.None;

            switch (splitMessage[1])
            {
                case "Mov":
                    return NetworkMessageContent.Move;          // ID|Mov|xMove|zMove
                case "Pos":
                    return NetworkMessageContent.Position;      // ID|Pos|xPositon|yPosition|zPosition
                case "ID":
                    return NetworkMessageContent.ID;            // ID|ID
                case "Ins":
                    return NetworkMessageContent.Instantiate;   // ID|Ins|Object|xPosition|yPosition|zPosition
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
                sb.Append(message + ":");
            }

            return sb.ToString();
        }

        public static string CombineMessages(List<string> messages)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var message in messages)
            {
                sb.Append(message + ":");
            }

            return sb.ToString();
        }

        public static string[] SplitMessages(string recievedMessage)
        {
            return recievedMessage.Split(':');
        }

        #region TranslateMessages

        public static bool TranslateMoveMessage(string message, out int clientID, out float x, out float z)
        {
            x = z = 0;
            clientID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Move)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) && float.TryParse(splitMessage[2], out x) && float.TryParse(splitMessage[3], out z))
            {
                return true;
            }

            return false;
        }

        public static bool TranslatePositionMessage(string message, out int clientID, out float x, out float y, out float z)
        {
            x = y = z = 0;
            clientID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Position)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) && float.TryParse(splitMessage[2], out x) && float.TryParse(splitMessage[3], out y) && float.TryParse(splitMessage[4], out z))
            {
                return true;
            }

            return false;
        }

        public static bool TranslateIDMessage(string message, out int clientID)
        {
            if (GetMessageContentType(message) != NetworkMessageContent.ID)
            {
                clientID = -1;
                return false;
            }

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID))
                return true;


            return false;
        }

        public static bool TranslateInstantiateMessage(string message, out int clientID, out string objectName, out float x, out float y, out float z)
        {
            x = y = z = 0;
            objectName = string.Empty;
            clientID = -1;

            if (GetMessageContentType(message) != NetworkMessageContent.Instantiate)
                return false;

            string[] splitMessage = message.Split('|');

            if (int.TryParse(splitMessage[0], out clientID) && float.TryParse(splitMessage[3], out x) && float.TryParse(splitMessage[4], out y) && float.TryParse(splitMessage[5], out z))
            {
                objectName = splitMessage[2];
                return true;
            }

            return false;
        }

        #endregion

        #region CreateMessage

        public static string CreateMoveMessage(int playerID, float x, float z)
        {
            return string.Format("{0}|Mov|{1}|{2}", playerID, x, z);
        }

        public static string CreatePositionMessage(int playerID, float x, float y, float z)
        {
            return string.Format("{0}|Pos|{1}|{2}|{3}", playerID, x, y, z);
        }

        public static string CreatePositionMessage(int playerID, Vector3 position)
        {
            return string.Format("{0}|Pos|{1}|{2}|{3}", playerID, position.x, position.y, position.z);
        }

        public static string CreateIDMessageFromServer(int clientID)
        {
            return string.Format("{0}|ID", clientID);
        }
        public static string CreateIDMessageFromClient()
        {
            return string.Format("-1|ID");
        }

        public static string CreateInstantiateMessage(int playerID, string objectName, float x, float y, float z)
        {
            return string.Format("{0}|Ins|{1}|{2}|{3}|{4}", playerID, objectName, x, y, z);
        }

        #endregion

    }
}
