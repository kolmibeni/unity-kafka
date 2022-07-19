#To install Confluent.Kafka from within Visual Studio, search for Confluent.Kafka in the NuGet Package Manager UI, or run the following command in the Package Manager Console:
Install-Package Confluent.Kafka -Version 1.7.0
#To add a reference to a dotnet core project, execute the following at the command line:
dotnet add package -v 1.7.0 Confluent.Kafka

dotnet run --project ./src/Producer/Producer.csproj
dotnet run --project ./src/Subscriber/Subscriber.csproj