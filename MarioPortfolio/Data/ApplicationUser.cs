using EasyImageHandler;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarioPortfolio
{
    public class ApplicationUser: IdentityUser
    {
        [Image]
        public byte[] ProfilePicture { get; set; }
    }
}
