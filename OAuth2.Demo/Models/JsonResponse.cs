﻿namespace OAuth2.Demo.Models
{
    public class JsonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}