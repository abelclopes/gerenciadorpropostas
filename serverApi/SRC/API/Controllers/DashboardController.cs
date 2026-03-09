using System.Threading.Tasks;
using Dapper;
using DOMAIN.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        public DashboardController(IContext context, IMemoryCache memoryCache) : base(context, memoryCache)
        { }

        [HttpGet("kpis")]
        [ProducesResponseType(typeof(DashboardKpiModel), 200)]
        [SwaggerResponse(401)]
        [SwaggerResponse(403)]
        public async Task<IActionResult> GetKpis()
        {
            var conn = Context.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            var sql = @"
SELECT
  COUNT(1) AS TotalPropostas,
  SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS PropostasAguardando,
  SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS PropostasAprovadas,
  SUM(CASE WHEN Status NOT IN (1,2) THEN 1 ELSE 0 END) AS PropostasOutrosStatus,
  CAST(
    ISNULL(
      SUM(
        CASE
          WHEN Valor IS NULL OR LTRIM(RTRIM(Valor)) = '' THEN 0
          WHEN CHARINDEX(',', Valor) > 0 THEN TRY_CONVERT(decimal(18,2), REPLACE(REPLACE(Valor, '.', ''), ',', '.'))
          ELSE TRY_CONVERT(decimal(18,2), Valor)
        END
      ), 0
    ) AS decimal(18,2)
  ) AS ValorTotal,
  SYSUTCDATETIME() AS UpdatedAtUtc
FROM dbo.Propostas
WHERE Excluido = 0;";

            var result = await conn.QueryFirstOrDefaultAsync<DashboardKpiModel>(sql);
            if (result == null)
            {
                result = new DashboardKpiModel();
            }

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Ok(result);
        }
    }
}
