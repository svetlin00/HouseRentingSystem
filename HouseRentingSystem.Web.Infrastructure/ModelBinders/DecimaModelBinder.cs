using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Globalization;

namespace HouseRentingSystem.Web.Infrastructure.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
     
        public DecimalModelBinder()
        {
            
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);

            decimal? acutalValue = null;
            bool success = false;


            if (valueResult != ValueProviderResult.None && !string.IsNullOrEmpty(valueResult.FirstValue))
            {
                try
                {

                    string decValue = valueResult.FirstValue;
                    decValue = decValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    decValue = decValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    acutalValue = Convert.ToDecimal(decValue, CultureInfo.CurrentCulture);
                    success = true;
                }
                catch (FormatException e)
                {

                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, e, bindingContext.ModelMetadata);
                }

            }
            else
            {
                if (bindingContext.ModelType == typeof(Decimal?)) 
                {
                    success = true;
                }
            }
            if (success) 
            {
                bindingContext.Result = ModelBindingResult.Success(acutalValue);
                return  Task.CompletedTask;
            }

            return Task.CompletedTask;



        }
    }
}
