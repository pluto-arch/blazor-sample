﻿using Dotnetydd.Tools;

namespace BlazorSample.Infra.Utils;

public static class SnowFlakeId
{
    private static readonly Lazy<SnowFlake> snowFlake = new Lazy<SnowFlake>(() => new SnowFlake(1));

    public static SnowFlake Generator => snowFlake.Value;
}