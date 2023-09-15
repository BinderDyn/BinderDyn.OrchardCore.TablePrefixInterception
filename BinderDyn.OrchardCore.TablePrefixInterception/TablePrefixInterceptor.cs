using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OrchardCore.Environment.Shell;

namespace BinderDyn.OrchardCore.TablePrefixInterception;

public class TablePrefixInterceptor : DbCommandInterceptor
{
    private readonly string _tablePrefix;
    private readonly string[] _tableNamesToReplace;

    public TablePrefixInterceptor(ShellSettings shellSettings, params string[] tableNamesToReplace)
    {
        _tablePrefix = shellSettings["TablePrefix"];
        _tableNamesToReplace = tableNamesToReplace;
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        if (_tablePrefix == string.Empty)
            return base.NonQueryExecuting(command, eventData, result);

        command.CommandText = AddMigrationsHistoryPrefix(command);
        foreach (var tableName in _tableNamesToReplace)
        {
            AddTablePrefix(command, tableName);
        }

        return result;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        if (_tablePrefix == string.Empty)
            return base.ReaderExecuting(command, eventData, result);

        command.CommandText = AddMigrationsHistoryPrefix(command);
        foreach (var tableName in _tableNamesToReplace)
        {
            AddTablePrefix(command, tableName);
        }

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = new CancellationToken())
    {
        if (_tablePrefix == string.Empty)
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);

        command.CommandText = AddMigrationsHistoryPrefix(command);
        foreach (var tableName in _tableNamesToReplace)
        {
            AddTablePrefix(command, tableName);
        }

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command,
        CommandEventData eventData, InterceptionResult<object> result)
    {
        if (_tablePrefix == string.Empty) return base.ScalarExecuting(command, eventData, result);

        command.CommandText = AddMigrationsHistoryPrefix(command);
        foreach (var tableName in _tableNamesToReplace)
        {
            AddTablePrefix(command, tableName);
        }

        return result;
    }

    private string AddMigrationsHistoryPrefix(DbCommand command)
    {
        return command.CommandText.Contains("__EFMigrationsHistory")
            ? command.CommandText.Replace("__EFMigrationsHistory", _tablePrefix + "__EFMigrationsHistory")
            : command.CommandText;
    }

    private string AddTablePrefix(DbCommand command, string tableNameToReplace)
    {
        return command.CommandText.Contains(tableNameToReplace)
            ? command.CommandText.Replace(tableNameToReplace, $"{_tablePrefix}_{tableNameToReplace}")
            : command.CommandText;
    }
}