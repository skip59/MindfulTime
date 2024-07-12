namespace MindfulTime.Notification.Infrastructure.Entities;

public class MessageResultEntity
{
    [Key]
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string MethodSend { get; set; }
    public DateTime Created { get; set; }
}
