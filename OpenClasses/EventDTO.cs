using System.ComponentModel.DataAnnotations;

namespace OpenClasses
{
    public class EventDTO
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        public double Priority { get; set; }
        public double Complexity { get; set; }
        public double Importance { get; set; }
        public bool HasDependencies { get; set; } 
        public bool AllDay { get; set; }
        [Required]
        public Guid UserId { get; set; }
        private double DurationInHours
        {
            get
            {
                if (DateTime.TryParse(Start, out DateTime start) && DateTime.TryParse(End, out DateTime end))
                {
                    return (end - start).TotalHours;
                }
                return 1;
            }
        }

        private double PriorityFactor
        {
            get
            {
                return Priority switch
                {
                    3 => 1.5,
                    2 => 1.2,
                    1 => 1.0,
                    _ => 1.0
                };
            }
        }

        private double ComplexityFactor
        {
            get
            {
                return Complexity switch
                {
                    3 => 2.0,
                    2 => 1.5,
                    1 => 1.0,
                    _ => 1.0
                };
            }
        }

        private double ImportanceFactor
        {
            get
            {
                return Importance switch
                {
                    3 => 3.0,
                    2 => 2.0,
                    1 => 1.0,
                    _ => 1.0
                };
            }
        }

        private double DependencyFactor => HasDependencies ? 1.0 : 0.7;
        public double StorePoint => AllDay ? 0.5 * PriorityFactor * ComplexityFactor * ImportanceFactor * DependencyFactor : (DurationInHours * ComplexityFactor * ImportanceFactor * DependencyFactor) > 100 ? 100 : DurationInHours * ComplexityFactor * ImportanceFactor * DependencyFactor;


    }
}
