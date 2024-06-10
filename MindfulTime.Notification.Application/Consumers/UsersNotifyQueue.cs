﻿using MindfulTime.Notification.Domain.Interfaces;
using MindfulTime.Notification.Domain.Services;
using MessageEntity = MindfulTime.Notification.Infrastructure.Entities.MessageEntity;

namespace MindfulTime.Notification.Domain.Consumers;

public class UsersNotifyQueue(UserNotificationRepositoryService repository, IBaseRepository<Message> baseRepository, IMessageService<MailSendModel> mailMessage, IMessageService<SendModel> tgMessage) 
    : IConsumer<NUserMT>, IConsumer<NUser_del_MT>, IConsumer<NUser_upd_MT>, IConsumer<UserEvent_out_MT>, IConsumer<UserEvent_out_Mail>
{
    private readonly UserNotificationRepositoryService _repository = repository;
    private readonly IBaseRepository<Message> _baseRepository = baseRepository;
    private readonly IMessageService<MailSendModel> _mailMessageService = mailMessage;
    private readonly IMessageService<SendModel> _tgMessageService = tgMessage;

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

        var contextToDb = new MessageEntity
        {
            Body = JsonConvert.SerializeObject(context.Message),
            Id = Guid.NewGuid(),
            Created = DateTime.Now.ToLocalTime(),
            MethodSend = "TG",
            Title = "Отправлено"
        };

        var user = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Id == context.Message.UserId);
        if (user != null)
        {
            SendModel sendModel = new()
            {
                Message = context.Message.Recomendation,
                UserId = user.Id,
                TelegramId = int.TryParse(user.TelegramId, out int result) ? result : 0,
            };
            await _tgMessageService.SendMessage(sendModel);
        };
        return;
    }

    public async Task Consume(ConsumeContext<UserEvent_out_Mail> context)
    {

        var contextToDb = new MessageEntity
        {
            Body = JsonConvert.SerializeObject(context.Message),
            Id = Guid.NewGuid(),
            Created = DateTime.Now.ToLocalTime(),
            MethodSend = "Mail",
            Title = "Отправлено"
        };

        var user = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Id == context.Message.UserId);
        if (user != null)
        {
            MailSendModel sendModel = new()
            {
                Message = context.Message.Recomendation,
                UserId = user.Id,
                Mail = user.Email,
            };
            await _mailMessageService.SendMessage(sendModel);
        };
        return;
    }
}
