using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

using Online_Learning_Platform_Ass1.Service.DTOs.Chatbot;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class ChatbotService(HttpClient httpClient, ICourseRepository courseRepository, IConfiguration configuration) : IChatbotService
{
    private readonly HttpClient _http = httpClient;
    private readonly ICourseRepository _courseRepository = courseRepository;
    private const string _aiEndpoint = "https://api.groq.com/openai/v1/chat/completions";
    private readonly string _groqApiKey = configuration["GroqAPIKey:Key"] ?? "";

    public async Task<string> AskAsync(string question, List<ChatHistoryItem> history)
    {
        if (string.IsNullOrEmpty(_groqApiKey))
        {
            return "Xin lỗi, chức năng AI chưa được cấu hình (Thiếu API Key).";
        }

        var courses = await _courseRepository.GetAllAsync();
        var courseList = courses.ToList();
        var totalCourses = courseList.Count;
        var courseData = string.Join("\n", courseList.Select(c => $"- {c.Title} (Price: {c.Price:C}): {c.Description}"));

        var messages = new List<object>
        {
            new
            {
                role = "system",
                content = "Bạn là một tư vấn viên khóa học chuyên nghiệp cho nền tảng học trực tuyến. " +
                          $"Hiện tại hệ thống đang có tổng cộng {totalCourses} khóa học. " +
                          "Dưới đây là danh sách chi tiết các khóa học:\n" +
                          courseData + "\n\n" +
                          "NHIỆM VỤ:\n" +
                          "1. Chỉ trả lời các câu hỏi liên quan đến học tập, giáo dục và thông tin về các khóa học trong danh sách trên.\n" +
                          "2. Nếu người dùng hỏi về các chủ đề không liên quan (chính trị, tôn giáo, giải trí, tư vấn tình cảm, y tế, pháp luật, v.v.) hoặc các nội dung nhạy cảm, hãy từ chối trả lời một cách lịch sự và hướng họ quay lại chủ đề học tập.\n" +
                          "   Ví dụ: 'Xin lỗi, tôi chỉ có thể hỗ trợ bạn các vấn đề liên quan đến khóa học và học tập. Bạn cần tư vấn khóa học nào không?'\n" +
                          "3. Đề xuất các khóa học phù hợp từ danh sách dựa trên nhu cầu của người dùng.\n" +
                          "4. Trả lời ngắn gọn, thân thiện, chuyên nghiệp bằng tiếng Việt."
            }
        };

        // Append history
        if (history != null && history.Any())
        {
            messages.AddRange(history.Select(h => new { role = h.Role, content = h.Content }));
        }

        // Append current question
        messages.Add(new { role = "user", content = question });

        var payload = new
        {
            model = "llama-3.3-70b-versatile",
            messages = messages,
            temperature = 0.5
        };

        return await SendToAi(payload);
    }

    private async Task<string> SendToAi(object payload)
    {
        var json = JsonSerializer.Serialize(payload);

        using var request = new HttpRequestMessage(HttpMethod.Post, _aiEndpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _groqApiKey);

        try 
        {
            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()!;
        }
        catch (Exception ex)
        {
            return $"Xin lỗi, có lỗi xảy ra khi liên hệ với AI server: {ex.Message}";
        }
    }
}
