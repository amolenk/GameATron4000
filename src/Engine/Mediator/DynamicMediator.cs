﻿namespace Amolenk.GameATron4000.Engine;

public interface IDynamicMediator : IMediator
{
    void Subscribe<TRequest>(IRequestHandler<TRequest> handler)
        where TRequest : IRequest;

    void Subscribe<TNotification>(INotificationHandler<TNotification> handler)
        where TNotification : INotification;
}
