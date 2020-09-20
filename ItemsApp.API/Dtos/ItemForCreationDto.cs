using System.ComponentModel.DataAnnotations;

namespace ItemsApp.API.Dtos
{
    public class ItemForCreationDto
    {
        [Required]
        public string ItemName { get; set; }

        [Required]
        public int Cost { get; set; }
    }
}