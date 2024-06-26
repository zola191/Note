﻿using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Notebook.Server.Authentication;
using Notebook.Server.Dto;
using Notebook.Server.Services;

namespace Notebook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly IAccountService accountService;
        private readonly IJwtProvider jwtProvider;

        public AccountController(IAccountService accountService, IJwtProvider jwtProvider)
        {
            this.accountService = accountService;
            this.jwtProvider = jwtProvider;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AccountRequest accountRequest)
        {
            var existingAccount = await accountService.FindByEmail(accountRequest.Email);

            if (existingAccount != null)
            {
                throw new Exception("Account is already exist");
            }

            var response = await accountService.CreateAsync(accountRequest);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var existingAccount = await accountService.FindByEmail(loginRequest.Email);

            if (existingAccount == null)
            {
                throw new Exception("Bad login or password");
            }

            if (existingAccount.Password != loginRequest.Password)
            {
                throw new Exception("Bad password");
            }

            var token = jwtProvider.Generate(existingAccount);
            existingAccount.Token = token;

            return Ok(existingAccount);
        }

    }
}
