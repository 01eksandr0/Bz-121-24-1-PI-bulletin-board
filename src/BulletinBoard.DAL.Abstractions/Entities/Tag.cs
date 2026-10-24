namespace BulletinBoard.DAL.Abstractions.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
