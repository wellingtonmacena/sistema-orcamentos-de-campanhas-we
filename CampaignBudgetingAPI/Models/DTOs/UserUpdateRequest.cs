using System.ComponentModel.DataAnnotations;

namespace CampaignBudgetingAPI.Models.DTOs
{
    /// <summary>
    /// Objeto de Transferência de Dados para a atualização de um usuário existente.
    /// </summary>
    public class UserUpdateRequest
    {
        // O ID é necessário para identificação no corpo (além da URL)
        [Required(ErrorMessage = "O ID é obrigatório para a atualização.")]
        public Guid Id { get; set; }

        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(150, ErrorMessage = "O e-mail não pode exceder 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        // A senha geralmente é atualizada em um endpoint separado, mas pode ser incluída aqui
        [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 50 caracteres.")]
        public string? Password { get; set; }
    }
}