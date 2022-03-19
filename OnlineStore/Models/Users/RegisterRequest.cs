﻿using OnlineStore.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models.Users
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Mật khẩu ít nhất 8 ký tự.", MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Avatar { get; set; }
    }
}
