﻿using System.ComponentModel.DataAnnotations;

namespace Finance_Management.Data
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
