﻿namespace HappyWarehouse.BusinessLogic.DTOs.QueryOptions
{
    public class QueryOption
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Page{ get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
