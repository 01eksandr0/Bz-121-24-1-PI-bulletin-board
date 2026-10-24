using System;
using System.Collections.Generic;
using BulletinBoard.BLL.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;

namespace BulletinBoard.WebAPI.Filters
{
    /// <summary>
    /// Перетворює винятки бізнес-логіки на HTTP-відповіді з відповідним статус-кодом.
    /// Завдяки цьому в контролерах немає try/catch.
    /// </summary>
    public class BusinessExceptionFilter : IExceptionFilter
    {
        private static readonly Dictionary<Type, int> StatusByException = new Dictionary<Type, int>
        {
            { typeof(NotFoundException), StatusCodes.Status404NotFound },
            { typeof(AccessDeniedException), StatusCodes.Status403Forbidden },
            { typeof(ValidationFailedException), StatusCodes.Status400BadRequest }
        };

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception as BusinessException;
            if (exception == null)
            {
                return; // інші помилки - це збій сервера (500), їх не приховуємо
            }

            int status;
            if (!StatusByException.TryGetValue(exception.GetType(), out status))
            {
                status = StatusCodes.Status400BadRequest;
            }

            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = status,
                Title = ReasonPhrases.GetReasonPhrase(status),
                Detail = exception.Message
            })
            {
                StatusCode = status
            };
            context.ExceptionHandled = true;
        }
    }
}
