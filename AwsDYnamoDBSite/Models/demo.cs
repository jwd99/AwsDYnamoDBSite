using Amazon.DynamoDBv2.DataModel;

namespace AwsDYnamoDBSite.Models
{
    public class demo
    {

        [DynamoDBTable("products")]
        public class Product
        {
            [DynamoDBHashKey("category")]
            public string Category { get; set; }

            [DynamoDBRangeKey("name")]
            public string Name { get; set; }

            [DynamoDBProperty("description")]
            public string Description { get; set; }

            [DynamoDBProperty("price")]
            public decimal Price { get; set; }
        }

    }
}
