namespace BulletinBoard.DAL.Abstractions.Entities
{
    /// <summary>Спільний контракт для всіх сутностей: кожна має цілочисельний ключ.</summary>
    public interface IEntity
    {
        int Id { get; set; }
    }
}
