using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Options;
public class AiModelConfig
{
    public required string DefaultModel { get; set; } 
    public Dictionary<string, AiModelDetails> Models { get; set; } = [];
}

public class AiModelDetails
{
    public required string Provider { get; set; }
    public required string ApiKey { get; set; }
    public required string Endpoint { get; set; }
}

public class AiModels
{
    public const string Options = nameof(AiModels);
    public required AiModelConfig Text { get; set; }
    public required AiModelConfig Image { get; set; }
    public required AiModelConfig Audio { get; set; }
}
