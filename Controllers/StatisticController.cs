using CP.Api.DTOs.Response;
using CP.Api.DTOs.Statistic;
using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers
{
    /// <summary>
    /// Statistic API controller
    /// </summary>
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        /// <summary>
        /// Statistic controller constructor
        /// </summary>
        /// <param name="statisticService">Statistic service</param>
        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        /// <summary>
        /// Get statistic of requested resource
        /// </summary>
        /// <param name="type">Type of resource</param>
        /// <param name="id">Id of resource</param>
        /// <param name="input">Detail of request</param>
        /// <returns>ResponseDTO <seealso cref="StatisticOutput"/></returns>
        [HttpPost]
        public ActionResult<ResponseDTO<StatisticOutput>> GetStatistic([FromRoute] StatisticType type, [FromBody] StatisticInput input, [FromRoute] int? id)
        {
            StatisticOutput? result = type switch
            {
                StatisticType.Register => _statisticService.GetRegisterStatistic(input),
                StatisticType.Account => _statisticService.GetAccountStatistic(accountId: id ?? default, input),
                StatisticType.Category => _statisticService.GetCategoryStatistic(categoryId: id ?? default, input),
                _ => null
            };

            return result switch
            {
                null => NotFound(new ResponseDTO<StatisticOutput> { Message = "Statistic not found", Success = false }),
                _ => Ok(new ResponseDTO<StatisticOutput> { Data = result, Success = true, Message = "Statistic found" })
            };
        }
    }
}
