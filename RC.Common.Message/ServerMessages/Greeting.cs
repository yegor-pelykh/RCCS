using System.IO;

namespace RC.Common.Message.ServerMessages
{
    public class Greeting : ServerMessage
    {
        public override ServerMessageType MessageType { get; } = ServerMessageType.Greeting;

        #region Overriden Methods

        public override void Serialize(BinaryWriter writer)
        {
        }

        public override void Deserialize(BinaryReader reader)
        {
        }

        #endregion

    }

}
