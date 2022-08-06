namespace ViewModels.Chat;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ChatSendMessageInputModel
{
    [Required]
    [MaxLength(50)]
    public string Message { get; set; }

    [Required]
    public string ReceiverId { get; set; }

    public IEnumerable<ChatUserViewModel> Users { get; set; }
}