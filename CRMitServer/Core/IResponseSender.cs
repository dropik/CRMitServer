namespace CRMitServer.Core
{
    public interface IResponseSender
    {
        void SendToClient(Client client);
    }
}
