namespace QuizIdentity.Domain.Entities.Domain;

public interface ICreated
{
    DateTimeOffset CreateDateUtc { get; set; }

    long CreatedByUserId { get; set; }
}
