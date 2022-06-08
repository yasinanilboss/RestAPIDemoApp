using System.ComponentModel.DataAnnotations;

namespace RestAPIDemo.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
