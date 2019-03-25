using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.Networking
{

    public enum NetworkMessageContent
    {
        None,
        Move,
        Position,
        ID
    }

    public static class NetworkTranslater
    {

        //Messages go playerID|contentType|data|data|data... ect

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
                default:
                    return NetworkMessageContent.None;
            }
        }

        public static NetworkMessageContent MessageContentType(string[] message)
        {
            switch (message[0])
            {
                case "Move":
                    return NetworkMessageContent.Move;
                default:
                    return NetworkMessageContent.None;
            }
        }

        #region TranslateMessages

        public static bool TranslateMoveMessage(string message, out int playerID, out float x, out float z)
        {
            string[] splitMessage = message.Split('|');

            if (MessageContentType(splitMessage) != NetworkMessageContent.Move)
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

            if (MessageContentType(splitMessage) != NetworkMessageContent.ID)
            {
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

        public static string CreateIDMessage(int clientID)
        {
            return string.Format("{0}|ID", clientID);
        }

        #endregion

    }
}
