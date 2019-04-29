using GraphQL.Types;

using Model;

namespace LogService.Gql
{
    public class LogEntryType : ObjectGraphType<LogEntry>
    {
        public LogEntryType()
        {
            Name = "LogEntry";

            Field(x => x.TimeStamp).Description("The timestamp from when the log entry was made.");
            Field(x => x.Text).Description("The raw text from the log entry.");
        }
    }
}
