﻿namespace Notebook.Server.Dto
{
    public class ErrorModel
    {
        public string Message { get; set; }

        public ErrorModel(string message)
        {
            Message = message;
        }

    }
}
