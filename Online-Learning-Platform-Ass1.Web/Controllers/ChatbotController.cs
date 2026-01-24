using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

using Online_Learning_Platform_Ass1.Service.DTOs.Chatbot;

namespace Online_Learning_Platform_Ass1.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController(IChatbotService chatbotService) : ControllerBase
{
    private readonly IChatbotService _chatbotService = chatbotService;

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { error = "Question cannot be empty" });
        }

        var response = await _chatbotService.AskAsync(request.Question, request.History);
        return Ok(new { response });
    }
}

public class ChatRequest
{
    public string Question { get; set; } = string.Empty;
    public List<ChatHistoryItem> History { get; set; } = [];
}
