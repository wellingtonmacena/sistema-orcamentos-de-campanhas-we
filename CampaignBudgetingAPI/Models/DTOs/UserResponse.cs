namespace CampaignBudgetingAPI.Models.DTOs
{
    /// <summary>
    /// Objeto de Transferência de Dados para o retorno de informações do usuário.
    /// </summary>
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }

        // Outras propriedades da entidade que o cliente precisa saber
    }
}