using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core
{
    public class GlobalResponse
    {
        public bool success { get; set; } = true;
        public string? message { get; set; }
        public string? error { get; set; } = null; 
        public dynamic data { get; set; } 
    }

    public class GlobalSearchRequest
    {
        public int? pageNo { get; set; } = 1;
        public int? pageSize { get; set; } = 10000;
        public string? searchText { get; set; }
    }

    public class GlobalResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; } = null;
        public T? Data { get; set; }
        public int? TotalCount { get; set; } = null;
    }

}
