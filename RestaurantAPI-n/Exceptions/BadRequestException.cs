namespace RestaurantAPI_n.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message): base(message) { }
    }
}
