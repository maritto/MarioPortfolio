using MarioPortfolio.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarioPortfolio.Controllers
{
    public class PictureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PictureController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetMyProfilePic()
        {
            byte[] x = _context.Users.Where(p => p.Id == _userManager.GetUserId(User)).First().ProfilePicture;
            if (x == null)
            {
                return File("~/img/ShrekAnonymous.png", "image/png");
            }
            return File(x, "image/png");
        }
    }
}
