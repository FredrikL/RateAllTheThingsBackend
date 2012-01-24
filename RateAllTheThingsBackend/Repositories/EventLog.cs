using System.Data.SqlClient;
using Dapper;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class EventLog : BaseRepo, IEventLog
    {
        public void LogEvent(Event e)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                connection.Execute("INSERT INTO LOG(AuthorId, BarCodeId, Event, Data, Ip) VALUES(@AUTHOR, @BARCODEID, @EVENT, @DATA, @IP)", 
                    new
                        {
                            AUTHOR = e.AuthorId,
                            BARCODEID = e.BarCodeId,
                            EVENT = e.EventName,
                            DATA = e.Data,
                            IP = e.Ip
                        });
            }
        }
    }
}