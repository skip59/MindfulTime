using System.ComponentModel.DataAnnotations;

namespace OpenClasses.Machine
{
    public class UserEvent_out_MT
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public float StorePoint { get; set; }
        [Required]
        public float Temperature { get; set; }
        [Required]
        public string WeatherType { get; set; }
        [Required]
        public string Recomendation { get; set; }
    }
}
