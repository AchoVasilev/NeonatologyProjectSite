namespace Neonatology.ViewModels.Chat;

public class LoadMoreMessagesViewModel
{
    public int Id { get; set; }

    public string Content { get; set; }

    public string SendedOn { get; set; }

    public string CurrentUsername { get; set; }

    public string FromUsername { get; set; }

    public string ReceiverUsername { get; set; }
}