using System;

namespace LMS.Common.Entities;

public class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}
