using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Utils.Mail;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecuperarSenhaController : ControllerBase
    {


        private readonly VitalContext _context;
        private readonly EmailSendingService _emailSendingService;

        public RecuperarSenhaController(VitalContext context, EmailSendingService emailSendingService)
        {
            _context = context;
            _emailSendingService = emailSendingService;

        }

        [HttpPost]
        public async Task<IActionResult> SendRecoveryPassword(string email)
        {
            try
            {

                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound("Usuario nao encontrado");
                }

                Random randon = new Random();
                int codigoRec = randon.Next(1000, 9999);

                user.CodRecupSenha = codigoRec;

                await _context.SaveChangesAsync();

                await _emailSendingService.SendRecoveryPassword(user.Email!, codigoRec);

                return Ok("Codigo de confirmacao enviado com sucesso");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPost("ValidarCodigo")]
        public async Task<IActionResult> ValidatePasswordRecoveryCode(string email, int codigo)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound("Usuario nao encontrado");

                }
                if (user.CodRecupSenha != codigo)
                {
                    return BadRequest("Codigo de recuperacao e invalido");
                }

                user.CodRecupSenha = null;

                await _context.SaveChangesAsync();

                return Ok("Codigo de recuperacao esta correto");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            

        }
    }
}
