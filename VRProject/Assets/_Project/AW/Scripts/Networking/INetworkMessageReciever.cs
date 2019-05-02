namespace VRGame.Networking
{
    public interface INetworkMessageReciever
    {
        void RecieveNetworkMessage(string recievedMessage);

        void SendNetworkMessage(string messageToSend);
    }
}
