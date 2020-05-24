using System;

namespace WebApi.Models
{
    public class ApiErrorVm
    {
        public ApiErrorVm()
        {
        }

        public ApiErrorVm(Exception exception)
        {
            Code = exception.GetType().Name;
            Message = exception.Message;
        }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Сообщение ошибки
        /// </summary>
        public string Message { get; set; }
    }
}
