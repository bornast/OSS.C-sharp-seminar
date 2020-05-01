using System;
using System.Collections.Generic;
using System.Text;

namespace Sindikat.Identity.Application.Models
{
    public class ValidationErrors
    {
        public Dictionary<string, List<string>> Errors { get; private set; } = new Dictionary<string, List<string>>();

        public void AddError(string fieldName, string errorMsg)
        {
            if (!Errors.ContainsKey(fieldName))
            {
                Errors.Add(fieldName, new List<string> { errorMsg });
            }
            else
            {
                var currentErrorMsgs = Errors[fieldName];
                currentErrorMsgs.Add(errorMsg);
            }
        }
    }
}
