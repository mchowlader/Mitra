using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitra.Api.Models;

public class BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public int CreateBy { get; set; }
    public DateTime CreateDate { get; set; }
    public int? UpdateBy { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsRemoved { get; set; }
}