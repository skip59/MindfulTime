namespace MindfulTime.Calendar.Domain.Repository.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set;}
        public DateTime LastModifiedBy { get; set;}
        public float StorePoint { get; set; }
        public User UserId { get; set; }
    }
}
