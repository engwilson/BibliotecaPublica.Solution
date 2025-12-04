namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class EditoraCreateDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Endereco { get; set; }
        public string? Contato { get; set; }
    }
}
