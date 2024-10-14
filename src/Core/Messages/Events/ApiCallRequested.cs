using System.Security.Claims;

namespace Amolenk.GameATron4000.Messages.Events;

public record ApiCallRequested(string RequestUri, Dictionary<string, string> Claims, Action<string> OnSuccess, Action<string> OnFailure) : IEvent;
