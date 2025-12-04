namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class AssociadoCreateDto
    {
        public string PrimeiroNome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
    }
}
