﻿using IDesign.Access.Identity.JWT;
using Microsoft.Extensions.Options;

namespace IDesign.Host.OptionsSetup
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "JwtTokenConfig";
        private readonly IConfiguration _config;

        public JwtOptionsSetup(IConfiguration config)
        {
            _config = config;
        }

        public void Configure(JwtOptions options)
        {
            _config.GetSection(SectionName).Bind(options);
        }
    }
}
