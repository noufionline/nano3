using Dapper;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.TypeMappers
{
    public class TimestampMapper : SqlMapper.TypeHandler<DateTime>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                parameter.Value = Timestamp.FromDateTimeOffset(DateTimeOffset.MinValue);
            }
            else
            {
                parameter.Value = Timestamp.FromDateTime(value);
            }
        }

        public override DateTime Parse(object value)
        {
            var date =(DateTime)value;

            var offsetdate = date==DateTime.MinValue ? Timestamp.FromDateTimeOffset(DateTimeOffset.MinValue) :  Timestamp.FromDateTimeOffset(date);
            return offsetdate.ToDateTimeOffset().LocalDateTime;
        }
    }
}
