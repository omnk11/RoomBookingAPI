﻿using System.ComponentModel.DataAnnotations;

namespace RoomBookingAPI.Model.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Admin" or "User"
    }
}
