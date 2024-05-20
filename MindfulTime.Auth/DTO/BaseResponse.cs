namespace MindfulTime.Auth.DTO
{
    public class BaseResponse<T>
    {
        public bool isError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
