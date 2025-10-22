# Consumed Time
4 hours and 57 minutes

# Tech Stack

* **.NET 9.0**

---

# Used Packages

* CsvHelper
* Swagger
* XUnit

---

# How To Run

1. Open the `.slnx` file (you'll need Visual Studio 2022 or higher).
2. Ensure that the target project to build is **BigIron.Api**.
3. Run, it will open **Swagger** for you.
4. Fill with data. There’s a `Data.csv` file inside the root folder you can use.

---

# Technical Decisions Overview

## Architecture

This project uses a **multi-layered architecture**.
There are a few layers, each following the **Single Responsibility Principle (S)** from **SOLID**.

### Projects Overview

| Project | Layer        | Description                                                                                                                                                                                                                            |
| ------- | ------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Api     | Controllers  | Exposes the HTTP endpoints based on "modules". In this scenario, we have only one (ISR), so not much complexity here.                                                                                                                  |
| Core    | DTOs         | Data Transfer Objects serve as sample contracts that can be received from outside. At the Core level, these DTOs ensure data transformation and act as contracts between different layers (e.g., Processors, Readers, and Services).   |
| Core    | ValueObjects | Encapsulate critical business logic, inspired by DDD concepts, and can also be used within a layered architecture.                                                                                                                     |
| Core    | Models       | Represent "Domain Entities" containing business logic. For example, GeoLocalization validation is implemented here as it’s critical to maintain accuracy.                                                                              |
| Core    | Entities (?) | Not used in this project since we don’t persist data. If we did, entities would map to the database (EF, Dapper, NoSQL, etc.).                                                                                                         |
| Core    | Readers      | Responsible for reading files and mapping them to a common DTO (see `ISRCsvReader`).                                                                                                                                                   |
| Core    | Processors   | Process batches of data and return results (see `ISRListProcessor`).                                                                                                                                                                   |
| Core    | Services     | Define use cases or flows. For example, `ISRService` reads data, processes it, and returns results.                                                                                                                                    |
| Core    | Wrappers     | Define a default response type, so we can provide messaging and error handling graciously.                                                                                                                                             |
| Core    | Enums        | Just common Enums                                                                                                                                                                                                                      |
| Core    | Mappings     | Mapping methods between DTOs, Models....                                                                                                                                                                                               |
---

### Architectural Thoughts

#### About Splitting Reading, Processing, and Service Layers

1. Ensures each part of the code does only one thing (**S** from **SOLID**).
2. Avoids large, bloated classes and improves readability.
3. Increases reusability; you can read, process, or reuse logic independently.
4. Makes scaling and refactoring much easier as the system evolves.

---

### Scenario 1: When receiving the CSV file, process it and store it in the Database

**Steps:**

1. In the service layer, create a method to:

   * Read the file (already done and reusable)
   * Process the file (already done and reusable)
   * Convert to entity
   * Store in the database

2. At the API level:

   * Add a controller with a proper DTO to call the new service method

3. At testing:

   * Create testing for each layer you want to cover, or even E2E tests

---

### Scenario 2: When receiving the CSV file, store it locally and then create a background job to process it later and store the results in the Database

**Steps:**

1. In the service layer, create a method **`ReadAndStoreFile`**:

   * Read the file (already done and reusable)
   * Store the file

2. In the service layer, create another method **`ProcessFileList`**:

   * Retrieve the files we need to cover
   * Process the file (already done and reusable)
   * Convert to entity
   * Store in the database

3. At the API level:

   * Add a controller with a proper DTO to call the `ReadAndStoreFile` method
   * Add a background job to call `ProcessFileList`

4. At testing:

   * Create testing for each layer you want to cover, or even E2E tests

---

### Scenario 3: When receiving the CSV file, read it locally, flag it (`IsProcessed`, `RequestRouteId` UUID), and process it later via background job

**Steps:**

1. In the service layer, create a method **`ReadAndQueueToProcess`**:

   * Read the file (already done and reusable)
   * Store the DTO in the table with the flags mentioned

2. In the service layer, create another method **`ProcessPendingRoutes`**:

   * Retrieve the routes to process and group them by `RouteId`
   * Process the data (already done)
   * Store it in another table
   * Update the flags

3. At the API level:

   * Add a controller with a proper DTO to call `ReadAndQueueToProcess`
   * Add a background job to call `ProcessPendingRoutes`

4. At testing:

   * Create testing for each layer you want to cover, or even E2E tests

---

### Considerations

This architecture enables scalability and flexibility.
You can easily evolve the system, refactor specific parts, or even mix multiple approaches without major disruption.

---

### Improvement Space

There are a few potential optimizations:

1. **Improve method naming** make it more descriptive of the actual behavior.
2. **Optimize distance calculations:**

   * By pre-storing customer addresses by area (neighborhood, city), you could reduce combinations from **N!** to **(N - 1)!**.
     Example:

     ```
     N = 10 → 10 * 9 * 8... * 1 = 3,628,800  
     (N - 1)! = 9! = 362,880
     ```

     That’s **10x fewer combinations**.
   * Create **sub-areas** (e.g., 100 locations split into 10 sub-areas).
     Process each sub-area separately, then aggregate results through access points similar to what Google Maps does for efficient routing.
   * Consider **sharding** or **indexing** data for high scalability.
     This allows pre-calculated, distributed datasets that continuously improve as data grows (no AI required).
