using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class DeleteResponse(bool IsSuccess, string? Message)
    {
        public bool IsSuccess { get; set; }
        public object Message { get; set; }
    }
}

