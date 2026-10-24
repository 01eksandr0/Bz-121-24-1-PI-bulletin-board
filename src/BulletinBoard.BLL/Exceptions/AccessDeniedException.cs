namespace BulletinBoard.BLL.Exceptions
{
    public class AccessDeniedException : BusinessException
    {
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}
