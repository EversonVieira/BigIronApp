# Tech Stack
.NET 9.0

# Used Packages
CsvHelper
Swagger
XUnit

# How To Run
1 - Open the slnx file (you'll need to use Visual Studio 2022 or higher)
2 - Ensure that the Target Project to build is the BigIron.Api
3 - Run it, it will open the swagger for you
4 - Fill with data, there's a Data.csv file inside the root folder, you can use that.

# Technnical decisions overview

## Architecture
This project uses Multi-layered-Archicture, there's few layers over there and they follow single principle responsibility from SOLID.

Projects | Description
Api | Serve as presentation layer, it's the one that exposes the services, processors and everything needed. Could be replaced by Console.App, Windows Forms, Blazor or anything else.
Core | Where the business lays down, at this example we're not following DDD (despite we could and in fact have some influences from it)
Tests | They're focused on integration tests, at this point, just for ensure that we're being able to communicate.

Project |Layer | Description

Api | Controllers | Exposes the HTTP endpoints based on "modules", in this scenario we have only one (ISR) so not really complexity over here.
Core | DTOs | Data Transfer Objects, this serve as sample contracts that can be received from outside, at Core level those DTOS ensure data transformation and helps to serve as contract between different layers, eg. Processors, Readers and Services.
Core | ValueObjects | This serves as encapsulation for some business logic that are critical, this comes from DDD and can be used within layered architecture as well. 
Core | Models | Models at this point represents the "Domain Entities", it has some business logic within since we're providing basic validation for GeoLocalization, that are Critical and we don't want to lose track of it.
Core | Entities(?) | At this task we don't need to store anything, that's why it doesn't exist at all, but if we would need to do, we also would like to have entities that would be mapped for database. Here we could use different context and entities types (NoSql Entities, EF entities, Dapper Entity (1 to 1 map)...)
Core | Readers | Those are responsible for reading files and casting for a common DTO, see ISRCsvReader for reference.
Core | Processors | This one represents a part of code where we process a bunch of data and retrieve something, in this scenario you can check ISRListProcesor
Core | Services | Services serve as bridge, It's where we define UseCases or flows, in this scenario ISRService receives a request, uses the Reader to get a list of ISRDto then Processes this list using the processor

### Architectural thoughts

#### About split reading, processing and service layers
1 - When splitting those you ensure that every part of the code does only one thing (S From Solid)
2 - This avoid big classes doing more than they're supposed to and improves the code readability
3 - This also improves code reusability, since you can read and file or process whenever you need it
4 - If in the near future we want to evolve that within few scenarios this approach helps with scalability.

##### Scenario 1: When receiving the CSV file, process it and store it in the Database

Steps:
1 - In the service layer, create a method to 
    1.1 - Read the file (already done and reusable)
    1.2 - Process the file (already done and reusable)
    1.3 - Convert to entity 
    1.4 - Store in the database

2 - At the Api level
   2.2 - Add a controller with a proper DTO to call the new service method

3 - At testing
   2.3 - Create testing for each layer you want to cover, or even E2E tests


##### Scenario 2: When receiving the CSV file, store it locally and then create a background job that would process that when the end user usage is low and store it in the Database

Steps:
1 - In the service layer, create a method "ReadAndStoreFile"
    1.1 - Read the file (already done and reusable)
    1.2 - Store the file
 
2 - In the service layer, create another method "ProcessFileList"
    1.1 - Retrieve the files we need to cover.
    1.2 - Process the file (already done and reusable)
    1.3 - Convert to entity
    1.4 - Storing the database

2 - At the Api level
   2.2 - Add a controller with a proper DTO to call the "ReadAndStoreFile" method
   2.3 - Add a background job to call "ProcessFileList"

3 - At testing
   2.3 - Create testing for each layer you want to cover, or even E2E tests


##### Scenario 3: When receiving the CSV file, read it locally and then store it in the database with 1 flag (IsProcessed and RequestRouteId UUID), create a background job that would process that when the end user usage is low, and store it in the Database

Steps:
1 - In the service layer, create a method "ReadAndQueueToProcess"
    1.1 - Read the file (already done and reusable)
    1.2 - Store the DTO at the table within the flags mentioned in the scenario
 
2 - In the service layer, create another method "ProcessPendingRoutes"
    1.1 - Retrieve the routes we need to cover and group them by RouteId.
    1.2 - Process the Data(Already done), store it in another table, and then update the flags.
    
2 - At the Api level
   2.2 - Add a controller with a proper DTO to call the "ReadAndQueueToProcess" method
   2.3 - Add a background job to call "ProcessPendingRoutes"

3 - At testing
   2.3 - Create testing for each layer you want to cover, or even E2E tests

##### Considerations

If you pay attention closely, you'll figure out that this approach gives scalability, we can move forward in every direction, and the code refactor would be almost. Also, we could change between approaches easily
and even cover multiple approaches at the same time.


#### Improvements Space

There are a couple of methods that can be improved here.

Method names can be rewritten to clearer understanding of what they're actually doing.
The Process Method to calculate Distances can be improved in many different forms.
For example:

1 - By previously storing the customers' addresses inside an area(neighborhood, city), we could set up (AccessPoints), reducing the combination possibilities from N! to (N - 1)!
Eg: N = 10, Possibilites are: 10 * 9 * 8... * 1 = 3628800 then (n-1)! (9!) = 362880, which means 10 times fewer combinations.

2 Set up sub-areas, for example, if we have 100 locations, we can set 10sub-areass, calculate between all of those,10, and then use those within the (AccessPoints) to reduce even more the combinations and the requested process
This is actually similar to what Google Maps does to ensure proper calculation.ion

3 - Since it could have a lot of data, we could distribute databases (sharding) or even index areas, creating a true pre-calculated database that not only processes distances at one time 
but learns, improves, and creates better routes within the growing data (and no, we don't need AI to do that).










