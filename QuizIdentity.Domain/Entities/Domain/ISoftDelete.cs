namespace QuizIdentity.Domain.Entities.Domain;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    DateTimeOffset? DeleteDateUtc { get; set; }

    long DeletedByUserId { get; set; }
}
