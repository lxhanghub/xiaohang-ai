using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextToAudio;
using Microsoft.SemanticKernel.TextToImage;

namespace Application.Common.AI.ModelSelector;
public interface IModelSelector
{
    IChatCompletionService GetTextModel(string? modelId = null);
}
