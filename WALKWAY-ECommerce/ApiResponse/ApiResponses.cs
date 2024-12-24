namespace WALKWAY_ECommerce.ApiResponse
{
    public class ApiResponses<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }


        public ApiResponses(int statusCode,string message, T data=default(T), string error=null) {
            StatusCode=statusCode;
            Message=message;
            Data=data;
            Error=error;
        }
    }
}
