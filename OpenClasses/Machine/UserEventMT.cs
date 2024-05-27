using System.ComponentModel.DataAnnotations;

namespace OpenClasses.Machine
{
    public class UserEventMT
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public float StorePoint { get; set; }
        public float Temperature { get; set; }
        public string WeatherType { get; set; }
    }
}
