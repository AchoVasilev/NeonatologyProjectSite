namespace ViewModels.Chat;

using System.Collections.Generic;
using Common.Models;

public class ChatUserViewModel : PagingModel
{
    public string Id { get; set; }

    public string FullName { get; set; }

    public string DoctorEmail { get; set; }

    public string GroupName { get; set; }

    public bool HasPaid { get; set; }

    public IEnumerable<ChatConversationsViewModel> ChatModels { get; set; }
}