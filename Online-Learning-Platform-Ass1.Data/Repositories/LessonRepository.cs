using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class LessonRepository : ILessonRepository
{
    List<Lesson> _lessons = new List<Lesson>();

    public LessonRepository()
    {
        _lessons.Add(new Lesson { Id = 1, Title = "Lesson 1", Content = "Content for Lesson 1", ModuleId = 1, OrderIndex = 1, CreatedAt = DateTime.Now });
        _lessons.Add(new Lesson { Id = 2, Title = "Lesson 2", Content = "Content for Lesson 2", ModuleId = 1, OrderIndex = 2, CreatedAt = DateTime.Now });
        _lessons.Add(new Lesson { Id = 3, Title = "Lesson 3", Content = "Content for Lesson 3", ModuleId = 1, OrderIndex = 3, CreatedAt = DateTime.Now });
    }

    public List<Lesson> GetLessonsByModuleId(int moduleId)
    {
        return _lessons.Where(l => l.ModuleId == moduleId).ToList();
    }
}
