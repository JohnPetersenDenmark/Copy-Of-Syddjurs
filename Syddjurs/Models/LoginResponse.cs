﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public  class LoginResponse
    {

        public string? Token { get; set; }
        //public string? UserName { get; set; }
        //public string? Email { get; set; }
        //public List<Claim>? Claims { get; set; }
    }
}
