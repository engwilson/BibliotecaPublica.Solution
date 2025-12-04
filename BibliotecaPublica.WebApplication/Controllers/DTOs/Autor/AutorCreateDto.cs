namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public class AutorCreateDto
    {
        public string PrimeiroNome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string? Biografia { get; set; }
    }
}
