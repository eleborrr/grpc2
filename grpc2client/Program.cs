using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using grpc2;

using var channel = GrpcChannel.ForAddress("http://localhost:5212");

Console.WriteLine("Напиши номер задания (1 или 2)");

input:
var inpt = Console.ReadLine();
if (inpt == "1")
{
    var client = new Meteo.MeteoClient(channel);
    using var call = client.GetMeteoData(new Empty());
    await foreach (var message in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"{DateTime.Now} погода на {message.Date} {message.Time} = {message.Temperature}C");
    } 
}
else if (inpt == "2")
{
    var headers = new Metadata();
    var client = new Private.PrivateClient(channel);
    var client2 = new Auth.AuthClient(channel);
    try
    {
        Console.WriteLine("Trying get secret without jwt");
        using var call = client.GetSecretAsync(new Empty(), headers);
        Console.WriteLine(call.ResponseAsync.Result.Secret);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.WriteLine("Getting jwt");
    using var call2 = client2.GetJwtAsync(new Empty());
    var jwt = call2.ResponseAsync.Result.Secret;
    headers.Add("Authorization", $"Bearer {jwt}");
    Console.WriteLine("Jwt added");

    Console.WriteLine("Trying get secret without jwt");

    using var call3 = client.GetSecretAsync(new Empty(), headers);
    Console.WriteLine(call3.ResponseAsync.Result.Secret);
}
else
{
    Console.WriteLine("Неверный ввод. Напиши номер задания (1 или 2)");
    goto input;
}







