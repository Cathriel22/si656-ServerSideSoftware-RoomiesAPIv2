﻿using System;
namespace Roomies.API.Domain.Services.Communications
{
    public abstract class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; protected set; }
        public T Resource { get; set; }
        protected BaseResponse(T resource)
        {
            Resource = resource;
            Success = true;
            Message = string.Empty;
        }
        protected BaseResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }
}