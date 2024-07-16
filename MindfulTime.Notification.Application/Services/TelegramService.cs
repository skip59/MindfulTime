namespace MindfulTime.Notification.Domain.Services;

public class TelegramService : ISendMessageTelegram
{
    internal static ITelegramBotClient client;
    internal static List<Update> upd = [];

    public class TelegramBot()
    {
        public static Dictionary<string, string> users = [];
        [SlashHandler("/botadd")]
        public async Task StartMenu(ITelegramBotClient botClient, Update update)
        {
            string msg = $"";
            await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
            client = botClient;
            if (!upd.Any(x => x.Message.Chat.Id == update.Message.Chat.Id)) upd.Add(update);
        }

    }
    public async Task<bool> SendMessage(SendModel item)
    {
        try
        {
            if (upd.Count > 0)
            {
                string message = string.Empty;
                foreach (Update update in upd)
                {
                    if (update.Message.Chat.Id == item.TelegramId)
                    {
                        message = $"{update.Message.From.FirstName}. {item.Message}";
                        await PRTelegramBot.Helpers.Message.Send(client, update, message);
                    }
                }
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
