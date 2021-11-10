using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI.Models
{
    public enum StatusCodes
    {
        Successful,
        Warning,
        Error
    }

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class Result
    {
        public string Status { get; set; }
        public List<HashSet<CartesianCoordinates>> Squares { get; set; }
    }
}
