namespace BulletinBoard.BLL.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string what, int id) : base(what + " з Id = " + id + " не знайдено.")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
