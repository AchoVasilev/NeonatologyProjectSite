namespace ViewModels.Feedback;

using static Common.GlobalConstants;
public class FeedbackReplyModel
{
    public int FeedbackId { get; set; }

    public string ReceiverFirstName { get; set; }

    public string ReceiverLastName { get; set; }

    public string ReceiverEmail { get; set; }

    public string SenderEmail { get; init; } = AdministratorEmail;

    public string Subject { get; set; }

    public string Text { get; set; }
}