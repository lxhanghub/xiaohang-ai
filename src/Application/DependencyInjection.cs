using System.Reflection;
using System.Runtime;
using Application;
using Application.Common.AI.ModelSelector;
using Application.Common.Behaviours;
using Application.Options;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(SaveChangeBehaviour<,>));
        });

        // Fix for CS1061: Ensure the Get<T>() method is used correctly with IConfiguration
        var aiSettings = new AiSettings
        {
            AiModels = configuration.GetSection("AiModels").Get<Dictionary<string, AiModelConfig>>() ?? new()
        };

        services.AddSingleton(aiSettings);
        
        var kernelBuilder = Kernel.CreateBuilder();
        AddAIChatCompletion(kernelBuilder, aiSettings!.AiModels["Text"].Models);
        // Fix for CS1503: Use a factory method to resolve IKernel  
        services.AddSingleton(sp => kernelBuilder.Build());
        services.AddSingleton<IModelSelector, ModelSelector>();

        return services;
    }

    private static void AddAIChatCompletion(IKernelBuilder kernelBuilder, Dictionary<string, AiModelDetails> Models)
    {
        foreach (var model in Models)
        {
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: model.Key,
                apiKey: model.Value.ApiKey,
                endpoint: new Uri(model.Value.Endpoint),
                serviceId: model.Key
            );
        }
    }
}
