namespace MindfulTime.Notification.Infrastructure.Entities;

public class MessageEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string MethodSend { get; set; }
    public DateTime Created { get; set; }

}
