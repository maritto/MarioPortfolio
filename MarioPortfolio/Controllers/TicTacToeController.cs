﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MarioPortfolio.Controllers
{
    public class TicTacToeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}