﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class AuthenticatedUserDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int CartId { get; set; }
    }
}
