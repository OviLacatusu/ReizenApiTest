namespace MessagingReizen
{
    public interface IMessage
    {
        Guid MessageId { get; }
        String Content { get; }
    }
}
