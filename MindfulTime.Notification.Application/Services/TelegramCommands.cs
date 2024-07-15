using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRTelegramBot.Attributes;
using PRTelegramBot.Helpers;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using PRTelegramBot.Utils;
using System.Xml.Linq;

namespace MindfulTime.Notification
{
    public static class TelegramCommands
    {
        //https://t.me/Novators_bot?start=start
        [SlashHandler("/start")]
        public static async Task Start(ITelegramBotClient botClient, Update update)
        {
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update,
                $"Добро пожаловать в бот для уведомлений. Скопируйте номер чата: ```{update.GetChatId()}``` Добовьте его в настройках проекта MindfulTime.",
                new OptionMessage() { ClearMenu = true, ParseMode = Telegram.Bot.Types.Enums.ParseMode.Markdown });
        }

        public static async Task AlwaisTask(ITelegramBotClient botClient, TimeSpan interval, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(interval, cancellationToken);
            }
        }
    }
}
