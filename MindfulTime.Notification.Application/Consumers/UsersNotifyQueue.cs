using MindfulTime.Notification.Domain.Services;

namespace MindfulTime.Notification.Domain.Consumers;

public class UsersNotifyQueue(UserNotificationRepositoryService repository,
    IBaseRepository<MessageResultEntity> baseRepository,
    ISendMessageTelegram telegramMessage,
    ISendMessageEmail emailMessage) : IConsumer<NUserMT>, IConsumer<NUser_del_MT>, IConsumer<NUser_upd_MT>, IConsumer<UserEvent_out_MT>
{
    private readonly UserNotificationRepositoryService _repository = repository;
    private readonly IBaseRepository<MessageResultEntity> _baseRepository = baseRepository;
    private readonly ISendMessageTelegram telegramMessage = telegramMessage;
    private readonly ISendMessageEmail emailMessage = emailMessage;

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

    enum MethodSend
    {
        Email,
        Telegram
    }
   

    public async Task Consume(ConsumeContext<UserEvent_out_MT> context)
    {
        var test =  _repository.ReadAsync().ToList();
        var user = await _repository.ReadAsync().SingleOrDefaultAsync(x => x.Id == context.Message.UserId);
        if (user != null)
        {
            if (!user.IsSendMessage) return;
            var contextToDb = new MessageResultEntity
            {
                Body = JsonConvert.SerializeObject(context.Message),
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                UserName = user.Name,
                UserEmail = user.Email,
                Title = "Отправлено"

            };
            if (user.TelegramId != "0")
            {
                SendModel sendTelegramModel = new()
                {
                    Message = context.Message.Recomendation,
                    UserName = user.Name,
                    UserId = user.Id,
                    TelegramId = int.TryParse(user.TelegramId, out int result) ? result : 0,
                };
                contextToDb.MethodSend = MethodSend.Telegram.ToString();
                var telegramMessageServiceTask = telegramMessage.SendMessage(sendTelegramModel);
                var repositoryTask = _baseRepository.CreateAsync(contextToDb);
                await Task.WhenAll(repositoryTask, telegramMessageServiceTask);
            }
            else
            {
                SendModel sendEmailModel = new()
                {
                    Message = context.Message.Recomendation,
                    UserName = user.Name,
                    UserId = user.Id,
                    Email = user.Email
                };
                contextToDb.MethodSend = MethodSend.Email.ToString();
                var emailMessageServiceTask = emailMessage.SendMessage(sendEmailModel);
                var repositoryTask = _baseRepository.CreateAsync(contextToDb);
                await Task.WhenAll(repositoryTask, emailMessageServiceTask);
            }
        };
        return;
    }


}
