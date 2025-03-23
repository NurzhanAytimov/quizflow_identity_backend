using QuizIdentity.Domain.Entities.Identity;
using QuizIdentity.Domain.Enum;

namespace QuizIdentity.Domain.Entities.Subscriptions;

public class Subscription
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? SubscriptionId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public bool IsActive { get; set; }
    public PlatformType Platform { get; set; }

    #region Navigation properties

    public User? User { get; set; }

    #endregion
}
