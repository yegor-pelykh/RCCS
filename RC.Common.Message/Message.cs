using System.IO;

namespace RC.Common.Message
{
    public abstract class Message
    {
        public abstract void Serialize(BinaryWriter writer);

        public abstract void Deserialize(BinaryReader reader);

    }

}
