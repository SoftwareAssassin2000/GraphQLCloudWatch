using System;
using GraphQL.Types;

using LogService.Gql;

using BusinessLogic;

namespace LogService
{
    public class Query : ObjectGraphType
    {
        public Query(ILogManager logManager)
        {
            Field<StringGraphType>(
                name: "hello",
                resolve: context => "world"
            );

            Func<ResolveFieldContext, string, object> func = (context, text) => logManager.GetAll(text);
            FieldDelegate<ListGraphType<LogEntryType>>(
                name: "logs",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "text", Description = "search for this text in the log entry" }
                ),
                resolve: func
            );
        }
    }
}
