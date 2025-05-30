using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Options;
public class AiModelConfig
{
    public required string DefaultModel { get; set; } 
    public Dictionary<string, AiModelDetails> Models { get; set; } = new();
}

public class AiModelDetails
{
    public required string Provider { get; set; }
    public required string ApiKey { get; set; }
    public required string Endpoint { get; set; }
}

public class AiSettings
{
    public Dictionary<string, AiModelConfig> AiModels { get; set; } = new();
}
