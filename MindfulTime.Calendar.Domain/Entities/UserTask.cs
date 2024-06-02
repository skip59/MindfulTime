namespace MindfulTime.Calendar.Infrastructure.Entities;

public class UserTask
{
    [Key]
    public Guid EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public bool AllDay { get; set; }
    public double StorePoint { get; set; }
    public Guid UserId { get; set; }
}
