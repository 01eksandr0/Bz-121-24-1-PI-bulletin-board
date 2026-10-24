namespace BulletinBoard.BLL.Exceptions
{
    public class ValidationFailedException : BusinessException
    {
        public ValidationFailedException(string message) : base(message)
        {
        }
    }
}
