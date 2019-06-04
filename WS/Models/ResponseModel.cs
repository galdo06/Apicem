namespace OAuth2.Demo.Models
{
    public class ResponseModel
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseModel(object _data)
        {
            Success = 1;
            Message = "";
            Data = _data;
        }
        public ResponseModel(int _success, string _message)
        {
            Success = _success;
            Message = _message;
        }
        public ResponseModel(int _success, string _message, object _data)
        {
            Success = _success;
            Message = _message;
            Data = _data;
        }
    }
}