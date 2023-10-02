using Mitra.Api.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitra.Api.Models.DBModel;

[Table("Action")]
public class Action : BaseModel
{
    public string Name { get; set; }
}