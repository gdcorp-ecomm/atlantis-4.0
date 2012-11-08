using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Razor;
using System.Web.Razor.Parser;
using Atlantis.Framework.DataCache;
using Atlantis.Framework.RazorEngine.Templating;

namespace Atlantis.Framework.RazorEngine.Compilation
{
  internal abstract class DirectCompilerServiceBase : CompilerServiceBase
  {
    private static readonly SlimLock _slimLock = new SlimLock();
    private static readonly AssemblyFileManager _assemblyFileManager = new AssemblyFileManager();

    private readonly CodeDomProvider _codeDomProvider;

    protected DirectCompilerServiceBase(RazorCodeLanguage codeLanguage, CodeDomProvider codeDomProvider, MarkupParser markupParser) : base(codeLanguage, markupParser)
    {
      if (codeDomProvider == null)
        throw new ArgumentNullException("codeDomProvider");

      _codeDomProvider = codeDomProvider;
    }

    private CompilerResults Compile(TypeContext context)
    {
      var compileUnit = GetCodeCompileUnit(context.ClassName, context.TemplateContent, context.Namespaces, context.TemplateType, context.ModelType);

      var compilerParameters = new CompilerParameters { GenerateInMemory = false,
                                                        GenerateExecutable = false,
                                                        IncludeDebugInformation = false,
                                                        CompilerOptions = "/target:library /optimize",
                                                        OutputAssembly = _assemblyFileManager.GetTempAssemblyCompilePath(context) };

      var assemblies = CompilerServices.GetLoadedAssemblies().Where(a => !a.IsDynamic).Select(a => a.Location).ToArray();
      compilerParameters.ReferencedAssemblies.AddRange(assemblies);

      _assemblyFileManager.SetupCompileDirectory(context);

      return _codeDomProvider.CompileAssemblyFromDom(compilerParameters, compileUnit);
    }

    public override Type CompileType(TypeContext context)
    {
      Assembly templateAssembly;

      CompilerResults compilerResults = null;
      AssemblyLoadResult assemblyLoadResult = _assemblyFileManager.LoadAssembly(context, out templateAssembly);

      if(assemblyLoadResult == AssemblyLoadResult.NotFound)
      {
        using (_slimLock.GetWriteLock())
        {
          // Make sure another thread did not create the file first while we wait for the lock
          if (!File.Exists(_assemblyFileManager.GetAssemblyFilePath(context)))
          {
            compilerResults = Compile(context);
          }
          else
          {
            _assemblyFileManager.LoadAssembly(context, out templateAssembly);
          }
        }
      }

      if(assemblyLoadResult != AssemblyLoadResult.Success && compilerResults != null)
      {
        if(compilerResults.Errors == null || compilerResults.Errors.Count == 0)
        {
          templateAssembly = compilerResults.CompiledAssembly;
          _assemblyFileManager.SaveAssemblyToDisk(compilerResults.CompiledAssembly.Location, context);
        }
        else
        {
          throw new TemplateCompilationException(compilerResults.Errors);
        }
      }
      
      Type templateType = null;
      if(templateAssembly != null)
      {
        templateType = templateAssembly.GetType("Atlantis.Framework.RazorEngine.Templates." + context.ClassName);
      }

      return templateType;
    }
  }
}