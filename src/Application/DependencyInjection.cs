using System.Reflection;
using System.Runtime;
using Application;
using Application.Common.AI.FunctionCalls;
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

        services.Configure<AiModels>(configuration.GetSection(AiModels.Options).Bind);

        var aimodels = configuration.GetSection("AiModels").Get<Dictionary<string, AiModelConfig>>() ?? [];

        var kernelBuilder = Kernel.CreateBuilder();

        AddAIChatCompletion(kernelBuilder, aimodels["Text"].Models);

        var build = kernelBuilder.Build();

        build.Plugins.AddFromType<WeatherPlugin>();
        
        services.AddSingleton(sp => build);

        services.AddSingleton(sp =>
        {
            var kernel = sp.GetRequiredService<Kernel>();
            var pluginPath = Path.Combine(AppContext.BaseDirectory, "Prompts");
            return kernel.CreatePluginFromPromptDirectory(pluginPath);
        });

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
