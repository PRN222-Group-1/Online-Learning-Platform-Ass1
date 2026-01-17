using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
public record LessonDTO(int Id, int ModuleId, string Title, string Content, string VideoUrl, string Duration, int OrderIndex, DateTime CreatedAt);
