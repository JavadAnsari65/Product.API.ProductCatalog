namespace Product.API.ProductCatalog.Extensions.ExtraClasses
{
    public class ApiResponse<T>
    {
        public bool Result { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
