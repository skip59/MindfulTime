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

namespace MindfulTime.Notification
{
    internal class Commands
    {
        [SlashHandler("/start")]
        public static async Task Start(ITelegramBotClient botClient, Update update)
        {
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, $"Добро пожаловать в бот для уведомлений. Скопируйте номер чата: ```{update.GetChatId()}``` Добовьте его в настройках проекта MindfulTime.", new OptionMessage() { ClearMenu = true, ParseMode = Telegram.Bot.Types.Enums.ParseMode.Markdown });
        }

        public static async Task StepPass(ITelegramBotClient botClient, Update update)
        {
            var mes = update.Message;

            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, "Успешно", new OptionMessage() { ClearMenu = true });
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
