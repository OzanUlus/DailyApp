using FluentValidation.Results;
using System.Runtime.CompilerServices;

namespace DailyApp.Extentions
{
    public static class ValidationExtentions
    {
        public static List<string> CustomValidationErrorList(this ValidationResult validationResult) 
        {
         var tempList = new List<string>();

            validationResult.Errors.ForEach(e => tempList.Add(e.ToString()));

            return tempList;
        
        
        }
    }
}
