﻿using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models
{
    public class RegistrationRequest
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
