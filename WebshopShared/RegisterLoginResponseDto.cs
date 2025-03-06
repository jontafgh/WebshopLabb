using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopShared
{
    public class RegisterLoginResponseDto
    {
        public bool Succeeded { get; set; }
        public List<string> ErrorList { get; set; } = [];
    }
}
