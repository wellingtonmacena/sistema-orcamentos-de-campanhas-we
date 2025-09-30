using CampaignBudgetingAPI.Models.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace CampaignBudgetingAPI.Models.DTOs
{
    /// <summary>
    /// Objeto de Transferência de Dados para a criação de um novo usuário.
    /// </summary>
    public class UserCreateRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(150, ErrorMessage = "O e-mail não pode exceder 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 50 caracteres.")]
        public string Password { get; set; } = string.Empty;

        public UserType UserType { get; set; }

        // Outras propriedades necessárias para a criação, como RoleId, etc.
    }
}