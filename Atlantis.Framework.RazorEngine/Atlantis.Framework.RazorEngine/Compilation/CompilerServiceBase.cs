﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Razor;
using System.Web.Razor.Generator;
using System.Web.Razor.Parser;
using Atlantis.Framework.RazorEngine.Templating;

namespace Atlantis.Framework.RazorEngine.Compilation
{
  /// <summary>
  /// Provides a base implementation of a compiler service.
  /// </summary>
  internal abstract class CompilerServiceBase : ICompilerService
  {
    /// <summary>
    /// Initialises a new instance of <see cref="CompilerServiceBase"/>
    /// </summary>
    /// <param name="codeLanguage">The code language.</param>
    /// <param name="markupParser">The markup parser.</param>
    protected CompilerServiceBase(RazorCodeLanguage codeLanguage, MarkupParser markupParser)
    {
      if (codeLanguage == null)
        throw new ArgumentNullException("codeLanguage");

      CodeLanguage = codeLanguage;
      MarkupParser = markupParser ?? new HtmlMarkupParser();
    }

    /// <summary>
    /// Gets the code language.
    /// </summary>
    public RazorCodeLanguage CodeLanguage { get; private set; }

    /// <summary>
    /// Gets the markup parser.
    /// </summary>
    public MarkupParser MarkupParser { get; private set; }

    /// <summary>
    /// Builds a type name for the specified template type and model type.
    /// </summary>
    /// <param name="templateType">The template type.</param>
    /// <param name="modelType">The model type.</param>
    /// <returns>The string type name (including namespace).</returns>
    public virtual string BuildTypeName(Type templateType, Type modelType)
    {
      if (templateType == null)
        throw new ArgumentNullException("templateType");

      if (!templateType.IsGenericTypeDefinition && !templateType.IsGenericType)
        return templateType.FullName;

      if (modelType == null)
        throw new ArgumentException("The template type is a generic defintion, and no model type has been supplied.");

      bool @dynamic = CompilerServices.IsDynamicType(modelType);
      Type genericType = templateType.MakeGenericType(modelType);

      return BuildTypeNameInternal(genericType, @dynamic);
    }

    /// <summary>
    /// Builds a type name for the specified generic type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="isDynamic">Is the model type dynamic?</param>
    /// <returns>The string typename (including namespace and generic type parameters).</returns>
    public abstract string BuildTypeNameInternal(Type type, bool isDynamic);

    /// <summary>
    /// Compiles the type defined in the specified type context.
    /// </summary>
    /// <param name="context">The type context which defines the type to compile.</param>
    /// <returns>The compiled type.</returns>
    public abstract Type CompileType(TypeContext context);

    /// <summary>
    /// Generates any required contructors for the specified type.
    /// </summary>
    /// <param name="constructors">The set of constructors.</param>
    /// <param name="codeType">The code type declaration.</param>
    private static void GenerateConstructors(IEnumerable<ConstructorInfo> constructors, CodeTypeDeclaration codeType)
    {
      if (constructors == null || !constructors.Any())
        return;

      var existingConstructors = codeType.Members.OfType<CodeConstructor>().ToArray();
      foreach (var existingConstructor in existingConstructors)
        codeType.Members.Remove(existingConstructor);

      foreach (var constructor in constructors)
      {
        var ctor = new CodeConstructor();
        ctor.Attributes = MemberAttributes.Public;

        foreach (var param in constructor.GetParameters())
        {
          ctor.Parameters.Add(new CodeParameterDeclarationExpression(param.ParameterType, param.Name));
          ctor.BaseConstructorArgs.Add(new CodeSnippetExpression(param.Name));
        }

        codeType.Members.Add(ctor);
      }
    }

    /// <summary>
    /// Gets the code compile unit used to compile a type.
    /// </summary>
    /// <param name="className">The class name.</param>
    /// <param name="template">The template to compile.</param>
    /// <param name="namespaceImports">The set of namespace imports.</param>
    /// <param name="templateType">The template type.</param>
    /// <param name="modelType">The model type.</param>
    /// <returns>A <see cref="CodeCompileUnit"/> used to compile a type.</returns>
    public CodeCompileUnit GetCodeCompileUnit(string className, string template, ISet<string> namespaceImports, Type templateType, Type modelType)
    {
      if (string.IsNullOrEmpty(className))
        throw new ArgumentException("Class name is required.");

      if (string.IsNullOrEmpty(template))
        throw new ArgumentException("Template is required.");

      templateType = templateType
                     ?? ((modelType == null)
                             ? typeof(TemplateBase)
                             : typeof(TemplateBase<>));

      var host = new RazorEngineHost(CodeLanguage, () => MarkupParser)
                     {
                       DefaultBaseClass = BuildTypeName(templateType, modelType),
                       DefaultClassName = className,
                       DefaultNamespace = "Atlantis.Framework.RazorEngine.Templates",
                       GeneratedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral",
                                                                         "WriteTo", "WriteLiteralTo",
                                                                         "RazorEngine.Templating.TemplateWriter")
                     };

      var templateNamespaces = templateType.GetCustomAttributes(typeof(RequireNamespacesAttribute), true)
          .Cast<RequireNamespacesAttribute>()
          .SelectMany(att => att.Namespaces);

      foreach (string ns in templateNamespaces)
        namespaceImports.Add(ns);

      foreach (string @namespace in namespaceImports)
        host.NamespaceImports.Add(@namespace);

      var engine = new RazorTemplateEngine(host);
      GeneratorResults result;
      using (var reader = new StringReader(template))
      {
        result = engine.GenerateCode(reader);
      }

      var type = result.GeneratedCode.Namespaces[0].Types[0];
      if (modelType != null)
      {
        if (CompilerServices.IsAnonymousType(modelType))
        {
          type.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(HasDynamicModelAttribute))));
        }
      }

      GenerateConstructors(CompilerServices.GetConstructors(templateType), type);

      var statement = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "Clear");
      foreach (CodeTypeMember member in type.Members)
      {
        if (member.Name.Equals("Execute"))
        {
          ((CodeMemberMethod)member).Statements.Insert(0, new CodeExpressionStatement(statement));
          break;
        }
      }

      return result.GeneratedCode;
    }
  }
}