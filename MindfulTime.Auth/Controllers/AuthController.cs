﻿using Microsoft.AspNetCore.Mvc;
using MindfulTime.Auth.Interfaces;
using OpenClasses;

namespace MindfulTime.Auth.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class AuthController(IUserService user) : ControllerBase
    {
        private readonly IUserService _userService = user;

        [HttpPost]
        public async Task<IActionResult> CheckUser(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.ReadUser(userDto);
                if (user.isError) return BadRequest(user.ErrorMessage);
                return Ok(user.Data);
            }
            return BadRequest("Входные данные не валидны.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.CreateUser(userDto);
                if (user.isError) return BadRequest(user.ErrorMessage);
                return Ok(user.Data);
            }
            return BadRequest("Входные данные не валидны.");
        }

        [HttpPost]
        public async Task<IActionResult> GetUsers(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var users = await _userService.ReadAllUsers(userDto);
                if (users.isError) return BadRequest(users.ErrorMessage);
                return Ok(users.Data);
            }
            return BadRequest("Входные данные не валидны.");
        }
    }
}
