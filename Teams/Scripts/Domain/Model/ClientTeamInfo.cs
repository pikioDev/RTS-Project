using Unity.Netcode;

namespace Xenocode.Features.Teams.Scripts.Domain.Model
{
    public struct ClientTeamInfo : INetworkSerializable
    {
        public ulong ClientId;
        public Team PlayerTeam;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref PlayerTeam);
        }
    }
}