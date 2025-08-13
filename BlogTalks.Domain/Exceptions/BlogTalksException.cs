using System.Net;

namespace BlogTalks.Domain.Exceptions
{
    public class BlogTalksException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        // Constructors
        public BlogTalksException() { }

        public BlogTalksException(string message) : base(message) { }

        public BlogTalksException(string message, Exception innerException) : base(message, innerException) { }

        public BlogTalksException(string message, int statusCode) : base(message)
        {
            StatusCode = (HttpStatusCode)statusCode;
        }

        public BlogTalksException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        // Optional: Add custom properties for additional error information
    }
}