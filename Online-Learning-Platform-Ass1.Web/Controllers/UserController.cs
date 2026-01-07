using Online_Learning_Platform_Ass1.Service.DTOs.User;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Controllers;

public class UserController(IUserService userService) : Controller
{
    // GET: User/Login
    [HttpGet]
    public IActionResult Login() => View();

    // POST: User/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid) return View(userLoginDto);

        var result = await userService.LoginAsync(userLoginDto);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Login successful!";
            return RedirectToAction(nameof(List));
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Login failed");
        return View(userLoginDto);
    }

    // GET: User/Register
    [HttpGet]
    public IActionResult Register() => View();

    // POST: User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid) return View(userRegisterDto);

        var result = await userService.RegisterAsync(userRegisterDto);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction(nameof(Login));
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed");
        return View(userRegisterDto);
    }

    // GET: User/Logout
    [HttpGet]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public IActionResult Logout()
    {
        TempData["SuccessMessage"] = "You have been logged out";
        return RedirectToAction("Index", "Home");
    }

    // GET: User/List
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var users = await userService.GetAllUsersAsync();
        return View(users);
    }
}
