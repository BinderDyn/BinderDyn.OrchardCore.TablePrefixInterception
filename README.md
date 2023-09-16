# Table Prefix Interception
## Designed to work with OrchardCore and custom relational, prefixed tables in one database
### How to use

1. In your Startup.cs add these lines to your service configuration:
```csharp
 services.AddTablePrefixInterceptor(o =>
 {
    o.TableNamesToPrefix = new[] { "ENTITY_NAME1", "SOME_OTHER_ENTITY" };
 });
```

2. The interceptor will use the configured prefix from the ShellSettings 
3. Use the registered interceptor in your db context
```csharp
optionsBuilder.AddInterceptors(_serviceProvider.GetRequiredService<TablePrefixInterceptor>());
```
4. Intercepted queries or commands to the database will use the prefix but only for EF Core related queries.

**Works with SqlServer, MySql and Postgres. SQLite is not supported.**

> :warning: **You probably shouldn't use Orchard Core or Entity Framework specific table names as this might lead to
> crashes or other unforeseen behavior.** 
> Specific table names include:
> - __EFMigrationsHistory
> - Document
> - UserIndex
> - and so on...
> Get the full list by viewing all tables present in an Orchard Core database



