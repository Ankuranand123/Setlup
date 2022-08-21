using Setlup.Models;

namespace Setlup.Services
{
    public interface IMessageService
    {
        string InsertTextMessage(string UserId,MessageText ObjMessageText);

        MessageTextList GetChat(string UserId, string CombinationId, int PageIndex);

        Details GetRequiredIds(string UserId, string CombinationId);
    }
}
