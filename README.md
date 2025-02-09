# VehicleApi
## Building and Running the solution
The solution can be ran with the following command
```
dotnet run --project Vehicles.Api
```
This will host the API at https://localhost:7097


## Running the tests
The tests can be ran with the following command
```
dotnet test
```

The tests are split into 3 groups
- Arhictecture: Tests that verify that our hexagonal architecture constraints have not been violated
- Unit: Testing individual units of code using mock data
- Component: End to end, black box integration tests that test the whole solution, validating the output based on the input without knowledge of any implementation details


## Notes
Most endpoints are implemented by directly calling the injected repository, but the '/Query' endpoint demonstrates a simple example of the CQRS pattern. In reality you might use MediatR to dispatch the query through a pipeline when implemening a production ready CQRS system.


## Future considerations
To take this API from it's current state to a production ready API some of the following would be needed
- Observability, using something like the OpenTelemetry stack for logging, tracing and metrics
- Authorization and Authentication for security
- CI/CD pipelines
- Proper documentation, both technical and user
