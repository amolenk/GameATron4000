﻿namespace Amolenk.GameATron4000.Engine.Mediator;

public interface ICustomMediator : IMediator
{
    void Subscribe<TRequest>(IRequestHandler<TRequest> handler)
        where TRequest : IRequest;

    void Subscribe<TNotification>(INotificationHandler<TNotification> handler)
        where TNotification : INotification;
}