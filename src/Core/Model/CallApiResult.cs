using System.Net;

namespace Amolenk.GameATron4000.Model;

public record CallApiResult(HttpStatusCode StatusCode, string? Content);