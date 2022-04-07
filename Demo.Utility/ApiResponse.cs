using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Utility
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }
    }
    public class APIResponseGeneric<T> where T : class
    {
        public APIResponseGeneric()
        {
            status = ApplicationMessage.Success;
        }
        public string status { get; set; }
        public string message { get; set; }
        public T data { get; set; }

    }
}
