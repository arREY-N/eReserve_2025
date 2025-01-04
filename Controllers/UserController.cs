using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models.ViewModels;
using WebApplication1.Migrations;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
	{

		private readonly Context? _context;

		public UserController(Context? context)
		{
			_context = context;
		}

		public async Task<IActionResult> Display(int UserID)
		{
			var userList = await Utility.GetUserList(_context);

			return View(userList);
		}

		public IActionResult Create()
		{
			ViewData["Designations"] = new SelectList(_context!.Designations, "DesignationID", "DesignationName");
			
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("Username, Password, FirstName, MiddleName, LastName, DesignationID, DesignationName, UserNumber, UserEmail")] User user)
		{
			if (ModelState.IsValid) 
			{
				_context!.Users.Add(user);
				await _context.SaveChangesAsync();
				// add UserID as argument here
				return RedirectToAction("Homepage", new { user.UserID });
			}
			return View();
		}

		public async Task<IActionResult> Edit(int UserId)
		{
			ViewData["Designations"] = new SelectList(_context!.Designations, "DesignationID", "DesignationName");
			var user = new Models.User();
			try
			{
				user = await Utility.GetUser(UserId, _context);
			} catch(Exception)
			{
				return NotFound();
			}
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Edit([Bind("UserID, Username, Password, FirstName, MiddleName, LastName, DesignationID, DesignationName, UserNumber, UserEmail")] User user)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, "Error editing user information. Please check again.");
                return View(user);
            }

            try
            {
                _context!.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine("Error editing: " + user.UserID);
                ModelState.AddModelError(string.Empty, "Error updating user information. Please try again.");
                return View(user);
            }
            return RedirectToAction("Display");
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> Delete(int UserId)
		{
			var item = await _context!.Users.FindAsync(UserId);

			if (item != null)
			{
				try
				{
                    _context.Users.Remove(item);
                } catch (Exception e)
				{
					Console.WriteLine("Error: " + e.Message);
				}
			}
			await _context.SaveChangesAsync();
			return RedirectToAction("Display");
		}

		public IActionResult Login()
		{
			if (HttpContext.Session.GetString("UserID") != null)
			{
				return View("Homepage");
			}
			return View();
		}

		public IActionResult LogOut()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Login(string Username, string Password)
		{
			var user = await _context!.Users.Include(d => d.Designation).FirstOrDefaultAsync(x => x.Username == Username);
			
			if (user != null && user.Password == Password)
			{
                Console.WriteLine("Successful login. UserID=" + user.UserID + "\n\n");
                var layout = (user.DesignationID == 1) ? "_AdminLayout" : "_UserLayout";

                HttpContext.Session.SetString("UserID", user.UserID.ToString());
				HttpContext.Session.SetString("Username", user.Username);
				HttpContext.Session.SetString("DesignationID", user.DesignationID.ToString()!);
                HttpContext.Session.SetString("Layout", layout);

				return RedirectToAction("Homepage");
            }

			ModelState.AddModelError(string.Empty, "Login failed.");
			return RedirectToAction("Login");
		}

		public async Task<IActionResult> Homepage()
		{
            var User = HttpContext.Session.GetString("UserID");
            if (int.TryParse(User, out int UserID))
			{
				Console.WriteLine("User ID: " + UserID);
				return View();
            }
			else
			{
				ModelState.AddModelError(string.Empty, "Invalid UserID");
			}
            return RedirectToAction("Home", "Index");
        }
	}
}

