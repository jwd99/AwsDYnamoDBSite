using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AwsDYnamoDBSite.Models.demo;



namespace AwsDYnamoDBSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class demo : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDBContext;

        public demo(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }

        [Route("get/{category}/{productName}")]
        [HttpGet]
        public async Task<IActionResult> Get(string category, string productName)
        {
            var product = await _dynamoDBContext.LoadAsync<Product>(category, productName);
            return Ok(product);
        }

        [Route("save")]
        [HttpPost]
        public async Task<IActionResult> Save(Product product)
        {
            await _dynamoDBContext.SaveAsync(product);
            return Ok();
        }

        [Route("delete/{category}/{productName}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string category, string productName)
        {
            await _dynamoDBContext.DeleteAsync<Product>(category, productName);
            return Ok();
        }


        [Route("search/{category}")]
        [HttpGet]
        public async Task<IActionResult> Search(string category, string? productName = null, decimal? price = null)
        {
            // Note: You can only query the tables that have a composite primary key (partition key and sort key).

            // 1. Construct QueryFilter
            var queryFilter = new QueryFilter("category", QueryOperator.Equal, category);

            if (!string.IsNullOrEmpty(productName))
            {
                queryFilter.AddCondition("name", ScanOperator.Equal, productName);
            }

            if (price.HasValue)
            {
                queryFilter.AddCondition("price", ScanOperator.LessThanOrEqual, price);
            }

            // 2. Construct QueryOperationConfig
            var queryOperationConfig = new QueryOperationConfig
            {
                Filter = queryFilter
            };

            // 3. Create async search object
            var search = _dynamoDBContext.FromQueryAsync<Product>(queryOperationConfig);

            // 4. Finally get all the data in a singleshot
            var searchResponse = await search.GetRemainingAsync();


            // Return it
            return Ok(searchResponse);
        }
    }
}
