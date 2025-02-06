using carGooBackend.Repositories;
using carGooBackend.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using carGooBackend.Models;
using Microsoft.CodeAnalysis.CSharp;
using carGooBackend.Data;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Korisnik> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly CarGooDataContext _context;

        public AuthController(UserManager<Korisnik> userManager, ITokenRepository tokenRepository, CarGooDataContext context)
        {
            this.userManager = userManager;
            _context = context;
            this.tokenRepository = tokenRepository;
        }

        // POST api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDto)
        {
            var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Mail);
            var preduzeces = await _context.Preduzeca.ToListAsync();  // Učitajte sve preduzetnike (preduzeća)

            if (existingUser != null)
            {
                return BadRequest(new { Message = $"Email '{registerRequestDto.Mail}' je već zauzet." });
            }
            //nesto
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
                        var preduzece = preduzeces.FirstOrDefault(p => p.Id == identityUser.PreduzeceId);  // Pronađite odgovarajuće preduzeće

                        if (preduzece != null)
                        {
                            preduzece.Korisnici.Add(identityUser);  // Dodajte korisnika u kolekciju preduzeća

                            // Ne zaboravite sačuvati promene u bazi podataka
                            await _context.SaveChangesAsync();

                            return Ok(new { Message = "Korisnik je uspešno kreiran!" });
                        }
                        else
                        {
                            return BadRequest(new { Message = "Preduzeće nije pronađeno!" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Message = "Neuspešno dodeljivanje vrste korisnika", Errors = roleResult.Errors });
                    }
                }
                return Ok(new { Message = "Korisnik registrovan ali nema vrstu korisnika! Molimo vas da se prijavite" });
            }
            else
            {
                return BadRequest(new { Message = "Neuspešna registracija!", Errors = identityResult.Errors });
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
                return BadRequest(new { Message = "Pogresna lozinka" });
            }
            return BadRequest(new { Message = "Uneti mail ne postoji" });
        }

        // GET api/auth/GetAllUsers
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await userManager.Users.ToListAsync();

                var userDtos = new List<object>();

                foreach (var user in users)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    userDtos.Add(new
                    {
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.FirstName,
                        user.LastName,
                        user.PhoneNumber,
                        Roles = roles // Include roles in the response
                    });
                }

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Došlo je do greške na serveru.", Error = ex.Message });
            }
        }
    }
}