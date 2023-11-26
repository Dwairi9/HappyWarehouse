using static HappyWarehouse.Shared.Common.Enums;

namespace HappyWarehouse.DataAccess.Entities
{
    public class SystemLog : Entity
    {
        public LogType LogType { get; set; }
        public string LogContent { get; set; }
    }
}
