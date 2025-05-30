using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Domain.Entities;
public class UserSession: AggregateRoot<Guid>
{
    public ChatHistory ChatHistory { get; set; } 

    public Guid UserId { get; set; }


}
