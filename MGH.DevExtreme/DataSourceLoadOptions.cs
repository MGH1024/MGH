using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace MGH.DevExtreme
{
    [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
    public class DataSourceLoadOptions : DataSourceLoadOptionsBase
    {
    }

}
