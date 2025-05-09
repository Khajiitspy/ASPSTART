namespace ASPSTART.Interfaces
{
    using ASPSTART.SMTP;

    public interface ISMTPService
    {
        Task<bool> SendMessageAsync(Message message);
    }
}
