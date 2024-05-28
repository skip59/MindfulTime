namespace MindfulTime.Auth.Domain.Repository
{
    public class BaseResponse<T>
    {
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; } = string.Empty;
        public T Data { get; set; }
    }

}
