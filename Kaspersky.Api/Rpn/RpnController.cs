using Kaspersky.Api.Common;
using Kaspersky.Utils.Rpn;
using Microsoft.AspNetCore.Mvc;

namespace Kaspersky.Api.Rpn
{
    [Route( "api/rpn" )]
    [ApiController]
    public sealed class RpnController : ControllerBase
    {
        /// <summary>
        /// Вычисление выражения в обратной польской записи
        /// </summary>
        /// <remarks>
        /// Операторы: + - * / ^
        /// Операнды: числа
        /// Из "10 4 2 / -" должно получиться 8
        /// </remarks>
        [HttpPost( "execute" )]
        public ApiResponse<double> Execute( RpnRequest request ) => ApiResponse.Create( RpnCalculator.Run( request.Expression ) );
    }
}
