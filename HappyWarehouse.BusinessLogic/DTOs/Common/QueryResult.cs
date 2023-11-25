namespace HappyWarehouse.BusinessLogic.DTOs.Common
{
    public class QueryResult<T>
    {
        public T Result { get; set; } = default;
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
