﻿namespace ASPSTART.Areas.Admin.Models.Products
{
    public class AdProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new();
    }
}
