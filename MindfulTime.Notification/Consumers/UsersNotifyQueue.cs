using MassTransit;
using Microsoft.EntityFrameworkCore;
using MindfulTime.Notification.Domain.Repository.Entities;
using MindfulTime.Notification.Domain.Repository.Interfaces;
using MindfulTime.Notification.Domain.Repository.Services;
using MindfulTime.Notification.TelegramBot;
using MindfulTime.Notification.TelegramBot.Models;
using Newtonsoft.Json;
using OpenClasses.Machine;
using OpenClasses.Notification;

namespace MindfulTime.Calendar.Consumers
{
    public class UsersNotifyQueue(UserNotificationRepositoryService repository, IBaseRepository<Message> baseRepository) : IConsumer<NUserMT>, IConsumer<NUser_del_MT>, IConsumer<NUser_upd_MT>, IConsumer<UserEvent_out_MT>
    {
        private readonly UserNotificationRepositoryService _repository = repository;
        private readonly IBaseRepository<Message> _baseRepository = baseRepository;
        public async Task Consume(ConsumeContext<NUserMT> context)
        {
            await _repository.CreateAsync(context.Message);
        }

        public async Task Consume(ConsumeContext<NUser_del_MT> context)
        {
            await _repository.DeleteAsync(context.Message);
        }

        public async Task Consume(ConsumeContext<NUser_upd_MT> context)
        {
            await _repository.UpdateAsync(context.Message);
        }

        public async Task Consume(ConsumeContext<UserEvent_out_MT> context)
        {

            var contextToDb = new Message
            {
                Body = JsonConvert.SerializeObject(context.Message),
                Id = Guid.NewGuid(),
                Created = DateTime.Now.ToLocalTime(),
                MethodSend = "TG",
                Title = "Отправлено"
            };
            //var repositoryResult = await _baseRepository.CreateAsync(contextToDb);
            //if (repositoryResult.IsError) return;
            var user = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Id == context.Message.UserId);
            if (user != null)
            {
                SendModel sendModel = new()
                {
                    Message = context.Message.Recomendation,
                    UserId = user.Id,
                    TelegramId = int.TryParse(user.TelegramId, out int result) ? result : 0,
                };
                await Sender.SendMessage(sendModel);
            };
            return;
        }


    }

}
