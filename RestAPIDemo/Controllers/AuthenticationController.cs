using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPIDemo.Models;
using RestAPIDemo.Models.Requests;
using RestAPIDemo.Models.Responses;
using RestAPIDemo.Repositories.Authenticators;
using RestAPIDemo.Repositories.PasswordHashers;
using RestAPIDemo.Repositories.RefreshTokenReopsitory;
using RestAPIDemo.Repositories.TokenGenerators;
using RestAPIDemo.Repositories.TokenValidators;
using RestAPIDemo.Repositories.UserRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestAPIDemo.Controllers
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AccessTokenGenerator _accessTokenGenerator;

        public AuthenticationController(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            Authenticator authenticator,
            RefreshTokenValidator refreshTokenValidator,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
        }


        [HttpPost("api/[controller]/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest) 
        {
            if(!ModelState.IsValid)
                return BadRequestModelState();
                
            if(registerRequest.Password != registerRequest.ConfirmPassword)
                return BadRequest(new ErrorResponse("Şifreniz, Şifreyi Onayla ile Eşleşmiyor!"));

            User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);

            if (existingUserByEmail != null)
                return Conflict(new ErrorResponse("Bu Email Önceden Kullanılmış!"));

            User existingUserByUsername = await _userRepository.GetByUsername(registerRequest.Username);

            if (existingUserByUsername != null)
                return Conflict(new ErrorResponse("Kullanıcı Adı Önceden Alınmış!"));

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

            User registrationUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                PasswordHash = passwordHash
            };

            await _userRepository.CreateUser(registrationUser);

            return Ok();
        }
        

        [HttpPost("api/[controller]/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if(!ModelState.IsValid)
                return BadRequestModelState();

            User user = await _userRepository.GetByUsername(loginRequest.Username);

            if (user == null)
            {
                return Unauthorized();
            }

            bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

            if (!isCorrectPassword)
                return Unauthorized();

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);

        }

        [HttpPost("api/[controller]/refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
                return BadRequestModelState();

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            if(!isValidRefreshToken)
                return BadRequest(new ErrorResponse("Geçersiz Refresh Token!"));

            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);

            if(refreshTokenDTO == null)
                return NotFound(new ErrorResponse("Refresh Token Bulunamadı!"));

            await _refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await _userRepository.GetById(refreshTokenDTO.UserId);

            if(user == null)
                return NotFound(new ErrorResponse("Kullanıcı Bulunamadı!"));

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);

        }
        
        [Authorize]
        [HttpDelete("api/[controller]/logout")]
        public async Task<IActionResult> Logout() 
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");

            if(!Guid.TryParse(rawUserId, out Guid userId))
            {
                return Unauthorized();
            }

            await _refreshTokenRepository.DeleteAll(userId);

            return NoContent();
        }
        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages =
            ModelState.Values.SelectMany(values => values.Errors.Select(errors => errors.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
