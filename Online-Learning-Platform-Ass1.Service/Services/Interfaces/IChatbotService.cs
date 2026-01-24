
using Online_Learning_Platform_Ass1.Service.DTOs.Chatbot;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface IChatbotService
{
    Task<string> AskAsync(string question, List<ChatHistoryItem> history);
}
