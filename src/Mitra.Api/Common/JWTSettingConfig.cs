﻿using System.ComponentModel.DataAnnotations;

namespace Mitra.Api.Common
{
    public class JWTSettingConfig
    {
        [Required]
        public string Secret { get; set; }
        [Required]
        public int ExpiresInMinutes { get; set; }
    }
}
