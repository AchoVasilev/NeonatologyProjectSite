namespace Neonatology.ViewModels.Feedback;

using Common.Attributes;
using Common.Mapping;

[NotInherited]
public class CreateFeedbackModel : FeedbackInputModel, IMapFrom<FeedbackInputModel>
{
}