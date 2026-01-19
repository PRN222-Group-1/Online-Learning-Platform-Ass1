using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class AiLessonService(HttpClient httpClient, ITranscriptService transcriptService, ILessonService lessonService) : IAiLessonService
{
    private readonly HttpClient _http = httpClient;
    private readonly ITranscriptService _transcriptService = transcriptService;
    private readonly ILessonService _lessonService = lessonService;

    private const string _aiEndpoint = "https://api.groq.com/openai/v1/chat/completions";
    private const string _groqApiKey = "";

    public async Task<string> GenerateSummaryAsync(Lesson lesson)
    {
        if (lesson.AiSummaryStatus == AiSummaryStatus.Done)
            return lesson.AiSummary!;

        if (lesson.AiSummaryStatus == AiSummaryStatus.Processing)
            return "Đang tạo tóm tắt, vui lòng đợi xíu...";

        lesson.AiSummaryStatus = AiSummaryStatus.Processing;
        await _lessonService.UpdateAsync(lesson);

        try
        {
            var transcript = await EnsureTranscriptAsync(lesson);
            var summary = await CallAiSummary(transcript);

            lesson.AiSummary = summary;
            lesson.AiSummaryStatus = AiSummaryStatus.Done;
            await _lessonService.UpdateAsync(lesson);

            return summary;
        }
        catch
        {
            lesson.AiSummaryStatus = AiSummaryStatus.Failed;
            await _lessonService.UpdateAsync(lesson);
            throw;
        }
    }

    public async Task<string> AskAsync(Lesson lesson, string question)
    {
        if (!string.IsNullOrWhiteSpace(lesson.AiSummary))
        {
            return await CallAiAsk(lesson.AiSummary, question);
        }

        var transcript = await EnsureTranscriptAsync(lesson);

        return await CallAiAsk(transcript, question);
    }
    private async Task<string> EnsureTranscriptAsync(Lesson lesson)
    {
        if (!string.IsNullOrWhiteSpace(lesson.Transcript))
            return lesson.Transcript;

        var transcript =
            await _transcriptService.GenerateTranscriptFromVideoAsync(
                lesson.VideoUrl
            );

        lesson.Transcript = transcript;
        await _lessonService.UpdateAsync(lesson);

        return transcript;
    }
    private async Task<string> CallAiAsk(string summary, string question)
    {
        var payload = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                        "You are a teacher. Answer ONLY using the summary, you can fix some content to match. " +
                        "If the summary does not contain the answer, say you do not know."
                },
                new
                {
                    role = "user",
                    content =
                        $"Transcript:\n{summary}\n\nQuestion:\n{question}"
                }
            },
            temperature = 0.3
        };

        return await SendToAi(payload);
    }

    private async Task<string> CallAiSummary(string transcript)
    {
        var payload = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                    "You are a Vietnamese teacher. " +
                    "ONLY summarize the lesson using the provided transcript, because it is from a video so some may wrong, please adjust if possible. " +
                    "Write in clear Vietnamese. " +
                    "If the transcript is unclear or empty, respond exactly: 'Không thể tóm tắt vì nội dung không rõ ràng.'"
                },
                new
                {
                    role = "user",
                    content = transcript
                }
            },
            temperature = 0.3
        };

        return await SendToAi(payload);
    }

    //Gui cho Ai
    private async Task<string> SendToAi(object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        using var content =
            new StringContent(json, Encoding.UTF8, "application/json");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _groqApiKey);

        _http.Timeout = TimeSpan.FromMinutes(2);

        var response = await _http.PostAsync(_aiEndpoint, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}
