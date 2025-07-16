namespace MessagingTrips
{
    public interface IMessage
    {
        Guid MessageId { get; }
        String Content { get; }
    }
}
