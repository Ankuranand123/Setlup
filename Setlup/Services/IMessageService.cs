using Setlup.Models;

namespace Setlup.Services
{
    public interface IMessageService
    {
        void InsertTextMessage(string UserId,MessageText ObjMessageText);
    }
}
