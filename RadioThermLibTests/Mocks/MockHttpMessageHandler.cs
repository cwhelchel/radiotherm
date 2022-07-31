using System.Text.Json;
using System.Text.Json.Nodes;
using RadioThermLib.Models;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace RadioThermLibTests.Mocks
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        Uri statusUri = new Uri("http://notreallyreal/tstat");
        Uri versionUri = new Uri("http://notreallyreal/tstat/model");

        ThermostatState currentState;
        StringContent? sc;

        public MockHttpMessageHandler()
        {
            currentState = Default.EmptyState with { CurrentState = ThermostatStateEnum.Off, Temperature = 69.0f, TemporaryHeatSetPoint = 666.999f};

            var json = JsonSerializer.Serialize(currentState);
            sc = new StringContent(json);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Get)
            {
                if (request.RequestUri == statusUri)
                {
                    return GetCurrentState();
                }
                else if (request.RequestUri == versionUri)
                {
                    string s = JsonSerializer.Serialize(new { model = "CT69 V6.9" });
                    return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(s) };
                }
            }
            else if (request.Method == HttpMethod.Post)
            {
                if (request.RequestUri?.AbsolutePath == statusUri.AbsolutePath)
                {
                    var c = await request.Content!.ReadAsStringAsync();
                    var jnode = JsonSerializer.Deserialize<JsonNode>(c);

                    if (jnode["t_heat"] != null)
                    {
                        var heat = jnode["t_heat"].AsValue();
                        float newTemp = heat.GetValue<float>();

                        currentState = currentState with { TemporaryHeatSetPoint = newTemp, CurrentState = ThermostatStateEnum.Heat };
                    }
                    else if (jnode["t_cool"] != null)
                    {
                        var heat = jnode["t_cool"].AsValue();
                        float newTemp = heat.GetValue<float>();

                        currentState = currentState with { TemporaryCoolSetPoint = newTemp, CurrentState = ThermostatStateEnum.Cool };
                    }

                    return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = sc };
                }
            }

            return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest, Content = new StringContent("fuck") };
        }

        private HttpResponseMessage GetCurrentState()
        {
            var json = JsonSerializer.Serialize(currentState);

            var response = new HttpResponseMessage 
            { 
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent(json) 
            };

            return response;
        }
    }
}
