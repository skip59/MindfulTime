using System.ComponentModel.DataAnnotations;

namespace MindfulTime.Notification.Domain.Repository.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string MethodSend { get; set; }
        public DateTime Created { get; set; }

    }
}
