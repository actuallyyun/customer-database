namespace src.Customer
{
    public class ExceptionHandler
    {
        public class CustomerNotFoundException : Exception
        {
            public CustomerNotFoundException() { }

            public CustomerNotFoundException(string message)
                : base(message) { }

            public CustomerNotFoundException(string message, Exception inner)
                : base(message, inner) { }
        }
        public class InvalidEmailException : Exception
        {
            public InvalidEmailException() { }

            public InvalidEmailException(string message)
                : base(message) { }

            public InvalidEmailException(string message, Exception inner)
                : base(message, inner) { }
        }
        public class NonNullableFieldException : Exception
        {
            public NonNullableFieldException() { }

            public NonNullableFieldException(string message)
                : base(message) { }

            public NonNullableFieldException(string message, Exception inner)
                : base(message, inner) { }
        }
        
    }
}
