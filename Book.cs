using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CosmosDbExample
{
    public class Book
    {
        [JsonProperty(PropertyName ="id")]
        public string Id { get; set; }

        public string Title { get; set; }

        public Author[]  Authors { get; set; }

        public PublishingHouse PublishingHouse { get; set; }

        public DateTime DateOfPublish { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Author
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }

    public class PublishingHouse
    {
        public string Name { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Line1 { get; set; }
    }
}
