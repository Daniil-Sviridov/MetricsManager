using Dapper;
using System.Data;
using System;
namespace MetricsAgent
{
    // Задаём хендлер для парсинга значений в TimeSpan, если таковые попадутся
    //в наших классах моделей
    public class TimeSpanHandler1 : SqlMapper.TypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value) => TimeSpan.FromSeconds((long)value);
        public override void SetValue(IDbDataParameter parameter, TimeSpan value) => parameter.Value = value;
    }
}