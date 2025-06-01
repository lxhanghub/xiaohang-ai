using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextToAudio;
using Microsoft.SemanticKernel.TextToImage;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Options;

namespace Application.Common.AI.ModelSelector;

public class ModelSelector : IModelSelector
{
    private readonly Kernel _kernel;
    private readonly AiModels _aiSettings;

    public ModelSelector(Kernel kernel, IOptions<AiModels> options)
    {
        _kernel = kernel;
        _aiSettings = options.Value;
    }

    public IChatCompletionService GetTextModel(string? modelId = null)
    {
        modelId ??= _aiSettings.Text.DefaultModel;
        return _kernel.GetRequiredService<IChatCompletionService>(modelId);
    }

}
