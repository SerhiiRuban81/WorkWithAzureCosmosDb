using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosDbExample
{
    class Program
    {
        string endpointUrl;
        string primaryKey;

        Database database;

        Container container;

        string databaseId = "books";
        string containerId = "book1";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Program program = new Program();
            program.endpointUrl = "https://rubancosmosdb.documents.azure.com:443/";
            program.primaryKey = "d9lY0kLAi9GNfwVndprQ3X90cSqKGcasxGScLGwKJbA2NTRakLJose1j3aQMGTCkE8vXgqM2zX2Zuf4RPHrcow==";
            await program.StartToCosmos();
            
        }

        private async Task CreateDContainerAsync()
        {
            container = await database.CreateContainerIfNotExistsAsync(containerId, "/Title");
            Console.WriteLine($"Создан контейнер {container.Id}");
        }

        private async Task StartToCosmos()
        {
            CosmosClient cosmosClient = new CosmosClient(accountEndpoint: endpointUrl, authKeyOrResourceToken: primaryKey);
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            await CreateDContainerAsync();
            await InsertData();
            await RunQueryAsync();
        }

        private async Task InsertData()
        {
            Book book = new Book {
                Id = "1",
                Title = "12 стульев",
                DateOfPublish = DateTime.Now,
                Authors = new Author[] {
                    new Author{ Firstname = "Илья", Lastname = "Ильф"},
                    new Author {Firstname = "Евгений", Lastname = "Петров"}
                },
                PublishingHouse = new PublishingHouse {
                    Name = "Буква",
                    Address = new Address { Country = "Украина", City = "Одесса", Line1 = "Дерибасовская 12" }
                }
            };
            ItemResponse<Book> itemResponse = await container.CreateItemAsync<Book>(book, new PartitionKey(book.Title));
            Console.WriteLine($"Добавлен элемент {itemResponse.Resource.Title}, сложность операции = {itemResponse.RequestCharge}");
        }

        private async Task RunQueryAsync()
        {
            string queryString = "SELECT * FROM c WHERE c.Title = '12 стульев'";
            QueryDefinition queryDefinition = new QueryDefinition(queryString);
            FeedIterator<Book> feedIterator = container.GetItemQueryIterator<Book>(queryDefinition);
            List<Book> books = new List<Book>();
            while (feedIterator.HasMoreResults)
            {
                FeedResponse<Book> feedResp = await feedIterator.ReadNextAsync();
                foreach (Book book in feedResp)
                {
                    Console.WriteLine($"\t{book}");
                    books.Add(book);
                    Console.WriteLine("--------------------");
                }
            }
        }
    }
}
