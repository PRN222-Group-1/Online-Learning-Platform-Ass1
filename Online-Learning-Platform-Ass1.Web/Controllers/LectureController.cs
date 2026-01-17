using Microsoft.AspNetCore.Mvc;

namespace Online_Learning_Platform_Ass1.Data.Controllers;
public class LectureController : Controller
{
    public IActionResult Index()
    {
        return View("~/Views/Course/Lecture.cshtml");
    }
}
