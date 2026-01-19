using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Learning_Platform_Ass1.Service.DTOs.Course;
public record class CourseDTO(int Id, string Title, string Description, string? ImageUrl, DateTime CreatedAt);
