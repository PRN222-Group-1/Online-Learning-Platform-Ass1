using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface IAiLessonService
{
    Task<string> AskAsync(Lesson lesson, string question);

    Task<string> GenerateSummaryAsync(Lesson lesson);
}
