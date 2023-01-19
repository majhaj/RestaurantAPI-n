using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI_n.Models
{
    public class UpdateRestuarantDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }

    }
}
