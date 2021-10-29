namespace Amolenk.GameATron4000.Scripting.Model;

public class NotificationCollector
{
    private List<INotification> _notifications;

    public IReadOnlyCollection<INotification> Notifications =>
        _notifications.AsReadOnly();

    public NotificationCollector()
    {
        _notifications = new List<INotification>();
    }

    public void Add(INotification notification) =>
        _notifications.Add(notification);

    public void Clear() => _notifications.Clear();
}
