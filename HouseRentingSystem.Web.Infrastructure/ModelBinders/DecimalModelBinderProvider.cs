using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Mvc;
using IModelBinderProvider = Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderProvider;


namespace HouseRentingSystem.Web.Infrastructure.ModelBinders
{
    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            // Check if the model is of type decimal or nullable decimal (decimal?)
            if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
            {
                // Return a new instance of the binder without explicitly passing ModelMetadata
                return new DecimalModelBinder();
            }

            return null; // Return null if not a matching type
        }
    }
}
