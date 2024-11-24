using carGooBackend.Repositories;
using carGooBackend.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Linq;
using System.Threading.Tasks;
using carGooBackend.Models;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Korisnik> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<Korisnik> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDto)
        {
            // Proveri da li korisnik sa datim email-om već postoji
            var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Mail);
            if (existingUser != null)
            {
                return BadRequest(new { Message = $"Email '{registerRequestDto.Mail}' je već zauzet." });
            }

            var identityUser = new Korisnik
            {
                UserName = registerRequestDto.Mail,
                Email = registerRequestDto.Mail,
                FirstName = registerRequestDto.FirstName,
                PhoneNumber = registerRequestDto.PhoneNumber,
                LastName = registerRequestDto.LastName,
                PreduzeceId = registerRequestDto.PreduzeceId,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    var roleResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (roleResult.Succeeded)
                    {
                        return Ok(new { Message = "Korisnik je registrovan! Molimo vas da se prijavite" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Neuspešno dodeljivanje uloga", Errors = roleResult.Errors });
                    }
                }
                return Ok(new { Message = "Korisnik je registrovan bez uloga! Molimo vas da se prijavite" });
            }
            else
            {
                return BadRequest(new { Message = "Registracija korisnika nije uspela", Errors = identityResult.Errors });
            }
        }

        // POST api/auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.EmailAddress);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    // Kreiranje tokena
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                }
                return BadRequest(new { Message = "Šifra nije ispravna" });
            }
            return BadRequest(new { Message = "Uneta email adresa ne postoji" });
        }

    }
}

