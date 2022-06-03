
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ZIF.ExternalServiceEvaluation;
using ValueType = ZIF.ExternalServiceEvaluation.ValueType;
namespace ZIF.CM.SampleExternalCalculationService.Services;

public class SampleExternalCalculationGrpcService : ExternalServiceEvaluationService.ExternalServiceEvaluationServiceBase
{
    public override Task<EvaluationResponse> Evaluate(EvaluationRequest request, ServerCallContext context)
    {
        //Формула расчета массового расхода газа.
        //Массовый расход (т/ч) = объемный расход (м3/ч) * плотность (кг/м3) / 1000
        var responseValue = request.Parameters["A"].Value.Double * request.Parameters["B"].Value.Double / 1000;
        
        if (request.IsRecalculation)
        {
            //Сделать что-нибудь необходимое при перерасчете
        }
        
        //Выставить качество выходного значения (не обязательно)
        var responseQuality = ValueQuality.Good;
        if (request.Parameters["A"].Quality == ValueQuality.Bad || 
            request.Parameters["B"].Quality == ValueQuality.Bad)
        {
            responseQuality = ValueQuality.Bad;
        }
        
        //Сформировать ответ
        var response = new EvaluationResponse()
        {
            Result = new TypedValue()
            {
                Double = responseValue
            },
            Quality = responseQuality
        };
        
        //Вернуть ответ
        return Task.FromResult(response);
    }

    public override Task<MetadataResponse> GetMetadata(Empty request, ServerCallContext context)
    {
        //Сформировать ответ на запрос метаданных о внешнем сервиса
        var result = new MetadataResponse()
        {
            Parameters = //Имена, типы и состав параметров полностью совпадают с используемыми в реализованном алгоритме расчета
            {
                {
                    "A", new ParameterMetadata()
                    {
                        ValueType = ValueType.Double,
                        Description = "Объемный расход (м3/ч)"
                    }
                },
                {
                    "B", new ParameterMetadata()
                    {
                        ValueType = ValueType.Double,
                        Description = "Плотность (кг/м3)"
                    }
                }
            },
            Result = new ResultMetadata()
            {
                ValueType = ValueType.Double, //Тип возвращемого значения совпадает с используемым в реализованном алгоритме расчета
                Description = "Массовый расход (т/ч)"
            },
            Author = "Сергей Газорасчётов",
            Description = "Расчёт массового расхода газа",
            ExtendedDescriptionUrl = "https://github.com/GasCorp/MassCalc/blob/v1.2.1/README.md",
            LastModifiedUtc = Timestamp.FromDateTime(LastModifiedUtc.Value) //Для удобства можно подставлять время сборки сервиса
        };
        return Task.FromResult(result);
    }

    private static readonly Lazy<DateTime> LastModifiedUtc = new(() =>
    {
        var assemblyFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
        return assemblyFileInfo.LastWriteTimeUtc;
    });
}
