using PRTelegramBot.Interface;

namespace MindfulTime.Notification.TelegramBot.Models
{
    public class StepCache : ITelegramCache
    {
        public string Name { get; set; }
        public bool ClearData()
        {
            Name = string.Empty;
            return true;
        }
    }
}
