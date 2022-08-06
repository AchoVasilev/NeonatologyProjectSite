namespace ViewModels.Feedback;

public class FeedbackViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Type { get; set; }

    public bool IsSolved { get; set; }

    public string Comment { get; set; }

    public string CreatedOn { get; set; }
}