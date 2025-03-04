namespace QuizIdentity.Domain.Entities.Domain;

public interface IUpdated
{
    DateTimeOffset? UpdateDateUtc { get; set; }

    long? UpdatedByUserId { get; set; }
}
