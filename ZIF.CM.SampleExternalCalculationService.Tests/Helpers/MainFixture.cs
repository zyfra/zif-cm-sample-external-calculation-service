using System.Threading.Tasks;
using Grpc.Net.Client;
using Xunit;
using ZIF.ExternalServiceEvaluation;

namespace ZIF.CM.SampleExternalCalculationService.Tests.Helpers;

public class MainFixture  : IAsyncLifetime
{
    public readonly CustomWebApplicationFactory CustomWebApplicationFactory;
    public readonly ExternalServiceEvaluationService.ExternalServiceEvaluationServiceClient GrpcClient;
    private readonly GrpcChannel _grpcChannel;
    
    public MainFixture()
    {
        CustomWebApplicationFactory = new CustomWebApplicationFactory();
        CustomWebApplicationFactory.CreateDefaultClient();
        
        var options = new GrpcChannelOptions
        {
            HttpHandler = CustomWebApplicationFactory.Server.CreateHandler()
        };
        
        _grpcChannel = GrpcChannel.ForAddress(CustomWebApplicationFactory.Server.BaseAddress, options);
        
        GrpcClient = new ExternalServiceEvaluationService.ExternalServiceEvaluationServiceClient(_grpcChannel);
    }
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _grpcChannel.Dispose();
        return Task.CompletedTask;
    }
}