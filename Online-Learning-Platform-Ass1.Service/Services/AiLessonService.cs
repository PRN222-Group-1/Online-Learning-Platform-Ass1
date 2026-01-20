using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class AiLessonService(HttpClient httpClient, ITranscriptService transcriptService, ILessonService lessonService) : IAiLessonService
{
    private readonly HttpClient _http = httpClient;
    private readonly ITranscriptService _transcriptService = transcriptService;
    private readonly ILessonService _lessonService = lessonService;

    private const string _aiEndpoint = "https://api.groq.com/openai/v1/chat/completions";
    private string _groqApiKey = Environment.GetEnvironmentVariable("GroqAPIKey__Key") ?? "";

    public async Task<string> GenerateSummaryAsync(LessonDTO lesson)
    {
        if (lesson.AiSummaryStatus == AiSummaryStatus.Done)
            return lesson.AiSummary!;

        if (lesson.AiSummaryStatus == AiSummaryStatus.Processing)
            return "Đợi xíu, AI đang tóm tắt bài này...";

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

    public async Task<string> AskAsync(LessonDTO lesson, string question)
    {
        var context = !string.IsNullOrWhiteSpace(lesson.AiSummary)
            ? lesson.AiSummary!
            : await EnsureTranscriptAsync(lesson);

        return await CallAiAsk(context, question);
    }

    private async Task<string> EnsureTranscriptAsync(LessonDTO lesson)
    {
        if (!string.IsNullOrWhiteSpace(lesson.Transcript))
            return lesson.Transcript!;

        var transcript =
            await _transcriptService.GenerateTranscriptFromVideoAsync(
                lesson.VideoUrl
            );

        lesson.Transcript = transcript;
        await _lessonService.UpdateAsync(lesson);

        return transcript;
    }

    private async Task<string> CallAiAsk(string context, string question)
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
                        "You are a teacher. Answer ONLY using the provided content. " +
                        "If the content does not contain the answer, say you do not know."
                },
                new
                {
                    role = "user",
                    content = $"Content:\n{context}\n\nQuestion:\n{question}"
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
                        "ONLY summarize the lesson using the provided transcript. " +
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

    private async Task<string> SendToAi(object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        using var content =
            new StringContent(json, Encoding.UTF8, "application/json");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _groqApiKey);

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
