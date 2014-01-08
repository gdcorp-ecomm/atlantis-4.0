<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Atlantis.Framework.Web.DotTypeRegistrationValidationHandler.TestWeb._Default" %>
<%@ Import Namespace="Atlantis.Framework.Providers.DotTypeRegistration.Interface" %>
<%@ Import Namespace="Atlantis.Framework.Providers.DotTypeRegistrationValidationHandler" %>
<%@ Import Namespace="Atlantis.Framework.Web.DynamicRouteHandler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <%
    var a = new DotTypeRegistrationValidationDynamicRouteHandler();
  %>
  <form method="post" action="<%: a.RoutePath.Path %>">
    <div>
      <% var i = 0;
         if (DotTypeFormFieldsByDomain != null) { 
        foreach (var formFields in DotTypeFormFieldsByDomain.FormFieldsByDomain)
         {
           //if (i == 0)
           //{
           //  i++;
           //  continue;
           //}

            var ffl = formFields.Value;
            foreach (var ffl2 in ffl)
            {
              foreach (var ff in ffl2)
              {
                if (ff.Type == FormFieldTypes.InputText)
                {
      %>
              <div>
                <input type='text' name='tui-legaltype' value="CCO"/>
              </div>
      <%
                }
                if (ff.Type == FormFieldTypes.Hidden)
                {
      %>
              <div>
                <input type='hidden' name='<%= ff.Name%>' value="<%=ff.Value %>"/>
              </div>
              <% }
   if (ff.Type == FormFieldTypes.Label) 
   {%>
              <div>
                <label><%= ff.Value %></label>
              </div>
    <%
                }
              }
            }
          }
        } 
      %>
    </div>
    <input type="submit" value="Submit">
  </form>
</body>
</html>
