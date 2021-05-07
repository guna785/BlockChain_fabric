using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string emailId { get; set; }
        [Required]
        public string phoneNo { get; set; }
        [Required]
        public string password { get; set; }
       
        [Required]
        public string gender { get; set; }
        [Required]
        public int age { get; set; }
    }
}
