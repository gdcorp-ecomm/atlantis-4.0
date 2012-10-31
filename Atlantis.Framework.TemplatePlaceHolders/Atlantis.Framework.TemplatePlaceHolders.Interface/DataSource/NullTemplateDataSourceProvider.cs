using System;
using System.Reflection;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  internal class NullTemplateDataSourceProvider : ITemplateDataSourceProvider
  {
    public dynamic GetDataSource(IDataSource dataSourceOptions)
    {
      ErrorLogHelper.LogError(new Exception("NullTemplateDataSourceProvider selected. Please verify you have a valid template data source provider in your placeholder and that it has been registered with your provider container."), MethodBase.GetCurrentMethod().DeclaringType.FullName);
      return null;
    }
  }
}
