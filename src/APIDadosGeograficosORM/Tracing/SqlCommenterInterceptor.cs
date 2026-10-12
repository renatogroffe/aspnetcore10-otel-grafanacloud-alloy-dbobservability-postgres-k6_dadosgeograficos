using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace APIDadosGeograficosORM.Tracing;

public sealed class SqlCommenterInterceptor : DbCommandInterceptor
{
    private static void AddSqlComment(DbCommand command)
    {
        var activity = Activity.Current;

        if (activity is null ||
            string.IsNullOrWhiteSpace(activity.Id))
        {
            return;
        }

        // Evita adicionar o comentário mais de uma vez.
        if (command.CommandText.Contains(
            "/*traceparent=",
            StringComparison.Ordinal))
        {
            return;
        }

        var traceparent = activity.Id;

        var tracestate = activity.TraceStateString;

        var comment =
            $" /*traceparent='{traceparent}'";

        if (!string.IsNullOrWhiteSpace(tracestate))
        {
            comment +=
                $",tracestate='{Uri.EscapeDataString(tracestate)}'";
        }

        comment += "*/";

        command.CommandText += comment;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        AddSqlComment(command);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        AddSqlComment(command);
        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        AddSqlComment(command);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AddSqlComment(command);
        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<object> ScalarExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result)
    {
        AddSqlComment(command);
        return result;
    }

    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result,
        CancellationToken cancellationToken = default)
    {
        AddSqlComment(command);
        return ValueTask.FromResult(result);
    }
}