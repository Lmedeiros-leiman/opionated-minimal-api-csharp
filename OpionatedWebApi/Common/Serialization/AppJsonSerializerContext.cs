using System.Text.Json.Serialization;
using OpionatedWebApi.Features.WeaterForecast.Endpoints;

namespace OpionatedWebApi.Common.Serialization;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(GetForecastEndpoint.Response[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;
