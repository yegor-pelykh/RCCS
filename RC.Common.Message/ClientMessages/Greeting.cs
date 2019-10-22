using System;
using System.IO;
using RC.Common.Helpers;

namespace RC.Common.Message.ClientMessages
{
    public class Greeting : ClientMessage
    {
        public override ClientMessageType MessageType { get; } = ClientMessageType.Greeting;

        #region Properties

        public Guid Id { get; set; }

        #endregion

        #region Overriden Methods

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteStringEx(Id.ToString());
        }

        public override void Deserialize(BinaryReader reader)
        {
            Id = Guid.Parse(reader.ReadStringEx());
        }
        
        #endregion

    }

}
