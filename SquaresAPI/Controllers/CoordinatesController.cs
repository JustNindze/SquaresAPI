using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SquaresAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoordinatesController : ControllerBase
    {
        private readonly ILogger<CoordinatesController> _logger;

        public CoordinatesController(ILogger<CoordinatesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ImportPoint")]
        public object ImportPoint(CartesianCoordinates point)
        {
            if (!CoordinatesData.AddPoint(point))
            {
                return Conflict(GetResponseMessage(StatusCodes.Warning, "Point already exists in the list"));
            }
            else
            {
                return Ok(GetResponseMessage(StatusCodes.Successful, "Point imported successfully"));
            }
        }

        [HttpPost]
        [Route("ImportPoints")]
        public object ImportPoints(HashSet<CartesianCoordinates> points)
        {
            var pointsAlreadyInHashSet = CoordinatesData.AddPoints(points);

            if (pointsAlreadyInHashSet == null)
            {
                return Ok(GetResponseMessage(StatusCodes.Successful, "All points imported successfully"));
            }
            else
            {
                string message = "Point/s";
                foreach (var point in pointsAlreadyInHashSet)
                {
                    message += string.Format(" ({0};{1})", point.X, point.Y);
                }
                message += " already exists in the list";

                return Conflict(GetResponseMessage(StatusCodes.Warning, message));
            }
        }

        [HttpPost]
        [Route("RemovePoint")]
        public object RemovePoint(CartesianCoordinates point)
        {
            if (!CoordinatesData.RemovePoint(point))
            {
                return Conflict(GetResponseMessage(StatusCodes.Warning, "Point does not exist in the list"));
            }
            else
            {
                return Ok(GetResponseMessage(StatusCodes.Successful, "Point removed successfully"));
            }
        }

        [HttpPost]
        [Route("RemovePoints")]
        public object RemovePoints(HashSet<CartesianCoordinates> points)
        {
            var pointsAlreadyInHashSet = CoordinatesData.RemovePoints(points);

            if (pointsAlreadyInHashSet == null)
            {
                return Ok(GetResponseMessage(StatusCodes.Successful, "All points removed successfully"));
            }
            else
            {
                string message = "Point/s";
                foreach (var point in pointsAlreadyInHashSet)
                {
                    message += string.Format(" ({0};{1})", point.X, point.Y);
                }
                message += " does not exist in the list";

                return Conflict(GetResponseMessage(StatusCodes.Warning, message));
            }
        }

        [HttpGet]
        [Route("GetSquares")]
        public object GetSquares()
        {
            var squares = CoordinatesData.GetSquares();
            return Ok(GetResult(StatusCodes.Successful, squares));
        }

        [NonAction]
        public Response GetResponseMessage(StatusCodes statusCode, string message)
        {
            return new Response
            {
                Status = statusCode.ToString(),
                Message = message
            };
        }

        [NonAction]
        public Result GetResult(StatusCodes statusCode, List<HashSet<CartesianCoordinates>> squares)
        {
            return new Result
            {
                Status = statusCode.ToString(),
                Squares = squares
            };
        }
    }
}
