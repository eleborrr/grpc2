using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace grpc2.Services;

public class MeteoService: Meteo.MeteoBase
{
    public override async Task GetMeteoData(Empty request, IServerStreamWriter<MeteoData> responseStream, ServerCallContext context)
    {
        using (var client = new HttpClient())
        {
            var curTime = DateTime.Now;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                string formattedDate = curTime.ToString("yyyy-MM-ddTHH:mm");
                var response = await client.GetAsync(
                    $"https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&hourly=temperature_2m&start_hour={formattedDate}&end_hour={formattedDate}");
                var content = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                
                var times = content.RootElement.GetProperty("hourly").GetProperty("time").EnumerateArray();
                times.MoveNext();
                var dateAndTime = times.Current.GetString().Split("T");
                
                var temperatures = content.RootElement.GetProperty("hourly").GetProperty("temperature_2m").EnumerateArray();
                temperatures.MoveNext();
                
                curTime = curTime.AddHours(2);
                await Task.Delay(1000);
           
                await responseStream.WriteAsync(new MeteoData(){Date = dateAndTime[0],Time = dateAndTime[1], Temperature = temperatures.Current.GetDouble()});

            }
        }
        
    }
}