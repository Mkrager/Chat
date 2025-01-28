using Chat.Api;

var builder = WebApplication.CreateBuilder(args);


var app = builder
    .ConfigureServiec()
    .ConfigurePipeline();

app.Run();

public partial class Program { }