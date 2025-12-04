namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class FilialCreateDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
    }
}
