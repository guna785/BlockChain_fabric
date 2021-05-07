using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Models
{
    public class LoginVIewModel
    {
        [Required]
        public string uname { get; set; }
        [Required]
        public string password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
