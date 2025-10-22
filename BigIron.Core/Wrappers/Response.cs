using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wrappers
{
    public class Response<T>:Response
    {
        public T? Result { get; set; }

        
        public Response()
        {

        }

        public Response(T? result, List<Message> messages):base(messages)
        {
            Result = result;
        }
    }

    public class Response
    {
        public List<Message> Messages { get; set; } = [];
        public bool HasError => Messages.Any(x => x.Type >= MessageType.VALIDATION);

        public void AddItem(Message message)
        {
            Messages.Add(message);
        }

        public void AddRange(List<Message> messages)
        {
            Messages.AddRange(messages);
        }
        
        public Response()
        {

        }

        public Response(List<Message> messages)
        {
            Messages = messages;
        }
    }
}
