using System;

namespace BulletinBoard.BLL.Exceptions
{
    /// <summary>Базовий виняток бізнес-логіки: порушене правило предметної області.</summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
