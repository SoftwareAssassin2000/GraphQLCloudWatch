using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;

using Model;

namespace DataAccess
{
    public class CloudWatchLogRepository : ILogRepository
    {
        private const string ACCESS_KEY_ID = "AKIAIDL7SL4XLEB2LREQ";
        private const string SECRET_ACCESS_KEY = "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";

        private const string LOG_GROUP_NAME = "/aws/lambda/ok-s3-lambda-auth-AspNetCoreFunction-15CGI3TSM9210";
        private const string LOG_STREAM_NAME = "2019/04/13/[$LATEST]aeeea13f723c4f2894f1df4c5ed71d0d";

        public async Task<List<LogEntry>> GetAll(string text)
        {
            using (var client = new AmazonCloudWatchLogsClient(ACCESS_KEY_ID, SECRET_ACCESS_KEY, RegionEndpoint.USEast2))
            {
                //get the log entries for the specified log group and log stream
                var res = await client.GetLogEventsAsync(new GetLogEventsRequest(LOG_GROUP_NAME, LOG_STREAM_NAME));

                //return events from Cloudwatch
                return res.Events

                    //filter out any that don't match the provided text
                    .Where(r => r.Message.Contains(text))

                    //convert into LogEntry DTO instances
                    .Select(r => new LogEntry() { TimeStamp = r.Timestamp, Text = r.Message })

                    //provide a list of the above created LogEntry instances
                    .ToList()

                    ;
            }
        }
    }
}
