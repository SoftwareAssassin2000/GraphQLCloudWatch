using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using GraphQL;
using GraphQL.Types;

using dto = Model;
using BusinessLogic;

namespace LogService.Gql
{
    [Route("graphql")]
    [ApiController]
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        #region Construction & Dependencies

        private ILogManager LogManager { get; set; }
        
        public Controller()
        {
            this.LogManager = new LogManager();
        }

        #endregion
        
        [Authorize]
        public async Task<IActionResult> Post([FromBody] dto.GqlRequest query)
        {
            var inputs = query.Variables.ToInputs();

            //define the schema and inject Query and LogManager instances
            var schema = new Schema
            {
                Query = new Query(this.LogManager)
            };

            //get results from GraphQL engine
            var result = await new DocumentExecuter().ExecuteAsync((System.Action<ExecutionOptions>)(x =>
            {
                x.Schema = schema;
                x.Query = query.Query;
                x.OperationName = query.OperationName;
                x.Inputs = inputs;
            }));

            //return a bad request if errors were encountered
            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            //return the results in case of success
            return Ok(result);
        }
    }
}