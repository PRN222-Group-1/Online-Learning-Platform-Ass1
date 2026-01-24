
namespace Online_Learning_Platform_Ass1.Service.DTOs.Chatbot;

public class ChatHistoryItem
{
    public string Role { get; set; } = string.Empty; // "user" or "assistant"
    public string Content { get; set; } = string.Empty;
}
