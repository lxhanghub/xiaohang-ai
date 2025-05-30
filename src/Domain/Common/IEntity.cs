﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Common;


public interface IEntity
{

}

public interface IEntity<TKey> : IEntity
{
    [Key]
    public TKey Id { get; init; }
}
