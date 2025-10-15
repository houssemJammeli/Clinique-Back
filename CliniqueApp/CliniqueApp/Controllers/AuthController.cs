using CliniqueApp.Context;
using CliniqueApp.DTOs.AuthDTOs;
using CliniqueApp.Models;
using CliniqueApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CliniqueDbContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher<Utilisateur> _passwordHasher;

        public AuthController(CliniqueDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<Utilisateur>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (await _context.Utilisateurs.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email déjà utilisé");

            Utilisateur user = request.Role switch
            {
                "Admin" => new Admin(),
                "Medecin" => new Medecin(),
                "Receptionniste" => new Receptionniste(),
                "Patient" => new Patient(),
                _ => null
            };

            if (user == null) return BadRequest("Rôle invalide");

            user.Nom = request.Nom;
            user.Prenom = request.Prenom;
            user.Email = request.Email;
            user.Password = _passwordHasher.HashPassword(user, request.Password);

            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur créé avec succès");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return Unauthorized("Email ou mot de passe incorrect");
            //var hashedRequestPassword = _passwordHasher.HashPassword(user, request.Password);
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Email ou mot de passe incorrect");

            string role = user.GetType().Name; // ex: "Patient", "Medecin"…
            var token = _jwtService.GenerateToken(user, role);

            return Ok(new AuthResponse
            {
                Token = token,
                Role = role,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Id = user.Id
            });
        }

    }
}
