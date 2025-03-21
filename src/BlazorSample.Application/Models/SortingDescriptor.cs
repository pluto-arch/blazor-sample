﻿using System.Diagnostics.CodeAnalysis;

namespace BlazorSample.Application.Models;

public enum SortingOrder { Ascending, Descending }


public class SortingDescriptor
{

    [AllowNull]
    public string PropertyName { get; set; }

    public SortingOrder SortDirection { get; set; }
}