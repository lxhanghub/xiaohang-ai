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

namespace Application.Common.AI.ModelSelector;
public class ModelSelector : IModelSelector
{
    private readonly Kernel _kernel;
    private readonly AiSettings _aiSettings;

    public ModelSelector(Kernel kernel, AiSettings aiSettings)
    {
        _kernel = kernel;
        _aiSettings = aiSettings;
    }

    public IChatCompletionService GetTextModel(string? modelId = null)
    {
        modelId ??= _aiSettings.AiModels["Text"].DefaultModel;
        return _kernel.GetRequiredService<IChatCompletionService>(modelId);
    }

}
