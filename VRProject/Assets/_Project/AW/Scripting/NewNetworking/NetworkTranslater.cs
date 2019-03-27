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
                case "Move":
                    return NetworkMessageContent.Move;
                case "Pos":
                    return NetworkMessageContent.Position;
                case "ID":
                    return NetworkMessageContent.ID;
                case "Ins":
                    return NetworkMessageContent.Instantiate;
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

        public static bool TranslateMoveMessage(string message, out int playerID, out float x, out float z)
        {
            string[] splitMessage = message.Split('|');

            if (GetMessageContentType(splitMessage) != NetworkMessageContent.Move)
            {
                x = z = playerID = 0;
                return false;
            }

            if (int.TryParse(splitMessage[0], out playerID) && float.TryParse(splitMessage[2], out x) && float.TryParse(splitMessage[3], out z))
            {
                return true;
            }

            x = z = playerID = 0;
            return false;
        }

        public static bool TranslateIDMessage(string message, out int clientID)
        {
            string[] splitMessage = message.Split('|');

            if (GetMessageContentType(message) != NetworkMessageContent.ID)
            {
                Debug.LogError("FOOOOOO");
                clientID = -1;
                return false;
            }

            if (int.TryParse(splitMessage[0], out clientID))
                return true;


            return false;
        }

        #endregion

        #region CreateMessage

        public static string CreateMoveMessage(int playerID, float x, float z)
        {
            return string.Format("{0}|Move|{1}|{2}", playerID, x, z);
        }

        public static string CreateIDMessageFromServer(int clientID)
        {
            return string.Format("{0}|ID", clientID);
        }
        public static string CreateIDMessageFromClient()
        {
            return string.Format("-1|ID");
        }

        #endregion

    }
}
