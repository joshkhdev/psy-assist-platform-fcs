﻿using Telegram.Bot.Types;

namespace PsyAssistFeedback.Application.Interfaces.Service;

public interface IFeedbackMessagesService
{
    Task PublishFeedbackAsync(Message message, Update update, CancellationToken cancellationToken);
}
