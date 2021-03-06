﻿namespace Kaspersky.Api.Common
{
    public static class ApiResponse
    {
        public static ApiResponse<T> Create<T>( T value ) => new ApiResponse<T>( value );
    }

    public class ApiResponse<T>
    {
        public T Result { get; }
        public ApiResponse( T result ) => Result = result;
    }
}
