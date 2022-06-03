using System;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Xunit;
using ZIF.CM.SampleExternalCalculationService.Tests.Helpers;
using ZIF.ExternalServiceEvaluation;

namespace ZIF.CM.SampleExternalCalculationService.Tests;

public class Tests : IClassFixture<MainFixture>
{
    private readonly MainFixture _mainFixture;
    private readonly ExternalServiceEvaluationService.ExternalServiceEvaluationServiceClient _grpcClient;

    public Tests(MainFixture mainFixture)
    {
        _mainFixture = mainFixture;
        _grpcClient = mainFixture.GrpcClient;
    }
    
    [Fact]
    public async Task Evaluation_Should_Return_Correct_Result_Value()
    {
        //Arrange
        var evaluationRequest = new EvaluationRequest()
        {
            Parameters =
            {
                {"A", new Parameter()
                    {
                        PropertyId = "7B15A5BB-A73D-4639-8F70-5704FB587FE8",
                        Value = new TypedValue()
                        {
                            Double = 1000
                        },
                        Quality = ValueQuality.Good
                    }
                },
                {"B", new Parameter()
                    {
                        PropertyId = "7B15A5BB-A73D-4639-8F70-5704FB587FE8",
                        Value = new TypedValue()
                        {
                            Double = 1000
                        },
                        Quality = ValueQuality.Good
                    }
                }
            },
            Timestamp = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            IsRecalculation = false,
            CalculationId = "6538B461-91A5-42EF-869B-0528312D5AA9",
            CalculationVersionId = "D0A21F73-70DE-4F2F-B569-89CFEC8F2EE3"
        };
        
        //Act
        var result = await _grpcClient.EvaluateAsync(evaluationRequest);
        
        //Assert
        result.Result.Double.Should().Be(1000);
    }
    
    [Fact]
    public async Task Evaluation_Should_Return_Correct_Quality()
    {
        //Arrange
        var evaluationRequest = new EvaluationRequest()
        {
            Parameters =
            {
                {"A", new Parameter()
                    {
                        PropertyId = "7B15A5BB-A73D-4639-8F70-5704FB587FE8",
                        Value = new TypedValue()
                        {
                            Double = 1000
                        },
                        Quality = ValueQuality.Bad
                    }
                },
                {"B", new Parameter()
                    {
                        PropertyId = "7B15A5BB-A73D-4639-8F70-5704FB587FE8",
                        Value = new TypedValue()
                        {
                            Double = 1000
                        },
                        Quality = ValueQuality.Good
                    }
                }
            },
            Timestamp = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            IsRecalculation = false,
            CalculationId = "6538B461-91A5-42EF-869B-0528312D5AA9",
            CalculationVersionId = "D0A21F73-70DE-4F2F-B569-89CFEC8F2EE3"
        };
        
        //Act
        var result = await _grpcClient.EvaluateAsync(evaluationRequest);
        
        //Assert
        result.Quality.Should().Be(ValueQuality.Bad);
    }
}