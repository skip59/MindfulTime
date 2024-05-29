﻿using MindfulTime.Notification.Application.Interfaces;
using MindfulTime.Notification.Application.TelegramBot.Models;
using PRTelegramBot.Attributes;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MindfulTime.Notification.Services
{
    public class TelegramService : IMessageService
    {
        internal static ITelegramBotClient client;
        internal static List<Update> upd = [];
        public class TelegramBot()
        {
            public static Dictionary<string, string> users = [];
            [ReplyMenuHandler("Бот+")]
            public async Task StartMenu(ITelegramBotClient botClient, Update update)
            {
                string msg = $"Привет {update.Message.From.FirstName.ToUpper()}. Добавь на личной странице https://localhost:7033 свой TelegramId: {update.Message.Chat.Id}, чтобы начать получать уведомления.";
                await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                client = botClient;
               
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
}
