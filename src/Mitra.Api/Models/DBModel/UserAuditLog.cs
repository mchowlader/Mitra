using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mitra.Api.Models.DBModel;

public class UserAuditLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public DateTime ActionMoment { get; set; }
    public string IpAddress { get; set; }
    public long ByUserId { get; set; }
    public long AffectedUserId { get; set; }
    public long ActionId { get; set; }
    public Action Action { get; set; }
}
