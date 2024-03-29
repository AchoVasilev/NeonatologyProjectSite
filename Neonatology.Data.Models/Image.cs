﻿namespace Neonatology.Data.Models;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

public class Image : BaseModel<string>
{
    public Image()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    public string RemoteImageUrl { get; set; }

    public string Url { get; set; }

    public string Name { get; set; }

    public string Extension { get; set; }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    public virtual ApplicationUser User { get; set; }
}