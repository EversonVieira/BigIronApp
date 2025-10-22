using Core.Enums;

namespace Core.Wrappers
{
    public class Message
    {
        public string? Text { get; init;  }
        public MessageType? Type {  get; init; }
    }
}
