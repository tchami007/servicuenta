namespace ServiCuentas.Shared
{
    public class Result<T>
    {
        public T _value {  get; set; }
        public string? _errorMessage {  get; set; }
        public List<string> _errorMessages { get; set; }
        public bool _success { get; set; }

        private Result(T value) 
        {
            _value = value; 
            _success = true; 
        }
        private Result(string? errorMessage) 
        { 
            _errorMessage = errorMessage; 
            _errorMessages = new List<string>();
            _success = false; 
        }
        private Result(List<string> errorMessages) 
        {
            _errorMessage = errorMessages.FirstOrDefault().ToString();
            _errorMessages = errorMessages; 
            _success = false; 
        }

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(string? errorMessage) => new Result<T>(errorMessage);
        public static Result<T> Failure(List<string> errorMessages) => new Result<T>(errorMessages);
    }
}
