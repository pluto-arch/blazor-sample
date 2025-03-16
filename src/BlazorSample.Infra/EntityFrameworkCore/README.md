﻿## 迁移命令

### 使用vs 包管理控制台 切换项目至 BlazorSample.Infra
```shell
# 添加迁移
Add-Migration InitialCreate -Context BlazorSampleDbContext -Project BlazorSample.Infra -StartupProject BlazorSample.Infra -OutputDir Migrations/BlazorSampleDb

# 移除迁移
Remove-Migration -Context BlazorSampleDbContext -Project BlazorSample.Infra -StartupProject BlazorSample.Infra

# 应用迁移
Update-Database -Context BlazorSampleDbContext -Project BlazorSample.Infra -StartupProject BlazorSample.Infra


# 使用链接字符串应用迁移
Update-Database -Context BlazorSampleDbContext -Project BlazorSample.Infra -StartupProject BlazorSample.Infra -Connection "数据库连接字符串"
```

### dotnet-ef 迁移命令
切换目录值src下。运行命令行
```shell
# 添加迁移
dotnet-ef migration add <migration_name> -c BlazorSampleDbContext -p BlazorSample.Infra -s BlazorSample.Infra -o Migrations/BlazorSampleDb

# 迁移应用到数据库
dotnet-ef database update -c BlazorSampleDbContext -p BlazorSample.Infra -s BlazorSample.Infra

# 指定连接字符串迁移
dotnet-ef database update -c BlazorSampleDbContext -p BlazorSample.Infra -s BlazorSample.Infra --connection "数据库连接字符串"
```
