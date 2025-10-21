using Microsoft.AspNetCore.Mvc;
using carGooBackend.Services;
using carGooBackend.DTOs;
using System.ComponentModel.DataAnnotations;

namespace carGooBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public SupportController(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendSupportRequest([FromBody] SupportRequestDto request)
        {
            try
            {
                // Validacija
                if (string.IsNullOrWhiteSpace(request.Ime) ||
                    string.IsNullOrWhiteSpace(request.Prezime) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.NazivPitanja) ||
                    string.IsNullOrWhiteSpace(request.Poruka))
                {
                    return BadRequest("Sva polja su obavezna.");
                }

                if (!new EmailAddressAttribute().IsValid(request.Email))
                {
                    return BadRequest("Nevalidan email format.");
                }

                if (request.Poruka.Length < 10)
                {
                    return BadRequest("Poruka mora imati najmanje 10 karaktera.");
                }

                // Email koji će biti poslan admin-u
                var supportEmail = "ujkanovicbakir@gmail.com"; // Ili iz configuration
                var subject = $"Nova poruka podrške: {request.NazivPitanja}";

                var emailBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2 style='color: #1976d2;'>Nova poruka podrške</h2>
                        <div style='background-color: #f5f5f5; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                            <p><strong>Ime:</strong> {request.Ime}</p>
                            <p><strong>Prezime:</strong> {request.Prezime}</p>
                            <p><strong>Email:</strong> {request.Email}</p>
                            <p><strong>Naziv pitanja:</strong> {request.NazivPitanja}</p>
                        </div>
                        <div style='margin: 20px 0;'>
                            <h3>Poruka:</h3>
                            <p style='white-space: pre-wrap; background-color: #f9f9f9; padding: 15px; border-radius: 5px;'>{request.Poruka}</p>
                        </div>
                        <hr style='margin: 20px 0;'/>
                        <p style='color: #666; font-size: 12px;'>
                            Ova poruka je poslana preko CarGoo podrške sistema.<br/>
                            Za odgovor odgovorite direktno na email: {request.Email}
                        </p>
                    </body>
                    </html>";

                // Pošalji email admin-u
                await _emailService.SendEmailAsync(supportEmail, subject, emailBody);

                // Pošalji potvrdu korisniku
                var confirmationSubject = "Potvrda - Vaša poruka je primljena";
                var confirmationBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2 style='color: #1976d2;'>Zdravo {request.Ime},</h2>
                        <p>Hvala vam što ste nas kontaktirali!</p>
                        <p>Vaša poruka je uspešno primljena i naš tim će vas kontaktirati u najkraćem mogućem roku.</p>
                        
                        <div style='background-color: #f5f5f5; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                            <h3>Detalji vaše poruke:</h3>
                            <p><strong>Naziv pitanja:</strong> {request.NazivPitanja}</p>
                            <p><strong>Datum slanja:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
                        </div>
                        
                        <p>Srdačan pozdrav,<br/>
                        CarGoo Tim</p>
                    </body>
                    </html>";

                await _emailService.SendEmailAsync(request.Email, confirmationSubject, confirmationBody);

                return Ok(new { message = "Poruka je uspešno poslana." });
            }
            catch (Exception ex)
            {
                // Log greške
                Console.WriteLine($"Greška pri slanju support email-a: {ex.Message}");
                return StatusCode(500, "Greška na serveru. Molimo pokušajte ponovo.");
            }
        }
    }
}
