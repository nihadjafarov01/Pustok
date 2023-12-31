﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.AuthVM
{
    public class LoginVM
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
