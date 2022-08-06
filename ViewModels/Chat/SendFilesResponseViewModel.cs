namespace ViewModels.Chat;

public class SendFilesResponseViewModel
{
    public bool HaveFiles { get; set; }

    public bool HaveImages { get; set; }

    public string MessageContent { get; set; }

    public string SenderId { get; set; }

    public string ReceiverId { get; set; }
}