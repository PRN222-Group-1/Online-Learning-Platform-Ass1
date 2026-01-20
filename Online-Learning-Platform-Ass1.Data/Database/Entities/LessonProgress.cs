using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;
public class LessonProgress
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public int LessonId { get; set; }

    public int WatchedPosition { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime UpdatedAt { get; set; }
}
