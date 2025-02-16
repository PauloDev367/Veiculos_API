using System;
using System.ComponentModel.DataAnnotations;

namespace VeiculosApi.Http.Request;

public class RemovePhotoRequest
{
    [Required]
    public List<Guid> Ids { get; set; }
}
