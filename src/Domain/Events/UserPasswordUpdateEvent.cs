﻿namespace Domain;

/// <summary>
/// 用户密码修改事件
/// </summary>
public class UserPasswordUpdateEvent : DomainEvent
{
    public Guid UserId { get; set; }
}
