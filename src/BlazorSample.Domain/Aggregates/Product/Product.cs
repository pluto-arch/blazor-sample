﻿using BlazorSample.Domain.Infra;

namespace BlazorSample.Domain.Aggregates.Product;

public class Product : BaseAggregateRoot<string>, ISoftDelete

{
    public Product()
    {
        Devices = [];
    }

    /// <summary>
    ///     产品名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     所属项目
    /// </summary>
    public int? ProjectId { get; set; }

    /// <summary>
    ///     描述信息
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTimeOffset CreationTime { get; set; }

    /// <summary>
    ///     设备列表
    /// </summary>
    public List<Device> Devices { get; set; }


    /// <inheritdoc />
    public bool Deleted { get; set; }

    public void AddDevice(Device device)
    {
        Devices.Add(device);
    }
}