using System.ComponentModel.DataAnnotations;

namespace RI_7030.Models
{
    /// <summary>
    /// Token for password reset flow.
    /// </summary>
    public class PasswordResetToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(200)]
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }
    }
}
