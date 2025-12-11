using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core
{
    //Approach 1
    public class GlobalResponse
    {
        public bool success { get; set; } = true;
        public string? message { get; set; }
        public string? error { get; set; } = null; 
        public dynamic data { get; set; }
        public int? totalcount { get; set; } = null;
    }
    
    //Approach 2
    public class GlobalResponse<T>
    {
        public bool success { get; set; } = true;
        public string? message { get; set; }
        public string? error { get; set; } = null;
        public T? data { get; set; }
        public int? totalcount { get; set; } = null;
    }

    public class GlobalSearchRequest
    {
        public int? pageNo { get; set; } = 1;
        public int? pageSize { get; set; } = 10000;
        public string? searchText { get; set; }
    } 
}
