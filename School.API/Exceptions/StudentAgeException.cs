using System;

namespace School.API.Exceptions
{
    public class StudentAgeException : Exception
    {
        public int Age { get; set; }
        public StudentAgeException()
        {
        }
        public StudentAgeException(string message) : base(message)
        {
        }
        public StudentAgeException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public StudentAgeException(string message, int age) : base(message)
        {
            Age = age;
        }
    }
}