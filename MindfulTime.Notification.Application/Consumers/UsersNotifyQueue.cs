using MassTransit;
using Microsoft.EntityFrameworkCore;
using MindfulTime.Notification.Application.Interfaces;
using MindfulTime.Notification.Application.TelegramBot.Models;
using MindfulTime.Notification.Domain.Repository.Entities;
using MindfulTime.Notification.Domain.Repository.Interfaces;
using MindfulTime.Notification.Domain.Repository.Services;
using Newtonsoft.Json;
using OpenClasses.Machine;
using OpenClasses.Notification;

namespace MindfulTime.Calendar.Application.Consumers
{
    public class UsersNotifyQueue(UserNotificationRepositoryService repository, IBaseRepository<Message> baseRepository, IMessageService message) : IConsumer<NUserMT>, IConsumer<NUser_del_MT>, IConsumer<NUser_upd_MT>, IConsumer<UserEvent_out_MT>
    {
        private readonly UserNotificationRepositoryService _repository = repository;
        private readonly IBaseRepository<Message> _baseRepository = baseRepository;
        private readonly IMessageService _messageService = message;
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
                await _messageService.SendMessage(sendModel);
            };
            return;
        }


    }

}
