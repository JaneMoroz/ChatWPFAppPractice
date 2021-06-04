using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Web.Server
{
    /// <summary>
    /// Manages the standard web server pages
    /// </summary>
    public class HomeController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected ApplicationDbContext _context;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The manager for handling signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> _signInManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        /// <summary>
        /// Basic welcome page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // Make sure we have the database
            _context.Database.EnsureCreated();

            // If we have no settings already...
            if (!_context.Settings.Any())
            {
                // Add a new setting
                _context.Settings.Add(new SettingsDataModel
                {
                    Name = "BackgroundColor",
                    Value = "Red"
                });

                // Check to show the new setting is currently only local and not in the database
                var settingsLocally = _context.Settings.Local.Count();
                var settingsDatabase = _context.Settings.Count();
                var firstLocal = _context.Settings.Local.FirstOrDefault();
                var firstDatabase = _context.Settings.FirstOrDefault();

                // Commit setting to database
                _context.SaveChanges();

                // Recheck to show its now in local and the actual database
                settingsLocally = _context.Settings.Local.Count();
                settingsDatabase = _context.Settings.Count();
                firstLocal = _context.Settings.Local.FirstOrDefault();
                firstDatabase = _context.Settings.FirstOrDefault();
            }

            return View();
        }

        /// <summary>
        /// Creates our single user for now
        /// </summary>
        /// <returns></returns>
        [Route("create")]
        public async Task<IActionResult> CreateUserAsync()
        {
            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "moroz",
                Email = "contact@moroz.com"
            }, "password");

            if (result.Succeeded)
                return Content("User was created", "text/html");

            return Content("User creation failed", "text/html");
        }

        /// <summary>
        /// Private area. No peeking
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("private")]
        public IActionResult Private()
        {
            return Content($"This is a private area. Welcome {HttpContext.User.Identity.Name}", "text/html");
        }

        /// <summary>
        /// Log the user out
        /// </summary>
        /// <returns></returns>
        [Route("logout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Content("done");
        }

        /// <summary>
        /// An auto-login page for testing
        /// </summary>
        /// <param name="returnUrl">The url to return to if successfully logged in</param>
        /// <returns></returns>
        [Route("login")]
        public async Task<IActionResult> LoginAsync(string returnUrl)
        {
            // Sign out any previous sessions
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // Sign user in with the valid credentials
            var result = await _signInManager.PasswordSignInAsync("moroz", "password", true, false);

            // If successful...
            if (result.Succeeded)
            {
                // If we have no return URL...
                if (string.IsNullOrEmpty(returnUrl))
                    // Go to home
                    return RedirectToAction(nameof(Index));

                // Otherwise, go to the return url
                return Redirect(returnUrl);
            }

            return Content("Failed to login", "text/html");
        }
    }
}
