namespace Neonatology.ViewModels.Chat;

using System.Collections.Generic;

public class ChatWithUserViewModel
{
    public ChatUserViewModel User { get; set; }

    public IEnumerable<ChatMessageWithUserViewModel> Messages { get; set; }

    public bool CanChat { get; set; }
}