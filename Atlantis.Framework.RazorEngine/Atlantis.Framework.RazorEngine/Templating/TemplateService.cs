using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Atlantis.Framework.RazorEngine.Compilation;

namespace Atlantis.Framework.RazorEngine.Templating
{
  public class TemplateService
  {
    private readonly ICompilerService _compilerService;
    private readonly IDictionary<string, ITemplate> _templateCache = new ConcurrentDictionary<string, ITemplate>();
    private readonly Type _templateType;

    public TemplateService(ICompilerService compilerService, Type templateType = null)
    {
      if (compilerService == null)
      {
        throw new ArgumentNullException("compilerService");
      }

      _compilerService = compilerService;
      _templateType = templateType;

      Namespaces = new HashSet<string> { "System", "System.Collections.Generic", "System.Linq" };
    }

    public ISet<string> Namespaces { get; private set; }

    private static void SetModel<T>(ITemplate template, T model)
    {
      var dynamicModel = template as ITemplate<dynamic>;
      if (dynamicModel != null)
        dynamicModel.Model = model;

      var strictModel = template as ITemplate<T>;
      if (strictModel != null)
        strictModel.Model = model;
    }

    private static void SetService(ITemplate template, TemplateService service)
    {
      template.Service = service;
    }

    private void Compile(string cacheKey, string template, Type modelType, string saveLocation = null)
    {
      if (string.IsNullOrEmpty(cacheKey))
      {
        throw new ArgumentException("Pre-compiled templates must have a cacheKey", "cacheKey");
      }

      GetTemplate(cacheKey, template, modelType, saveLocation);
    }

    internal ITemplate CreateTemplate(string cacheKey, string template, Type modelType, string saveLocation = null)
    {
      var context = new TypeContext { TemplateKey = cacheKey,
                                      TemplateType = _templateType,
                                      TemplateContent = template,
                                      ModelType = modelType,
                                      SaveLocation = saveLocation };

      foreach (string @namespace in Namespaces)
      {
        context.Namespaces.Add(@namespace);
      }

      Type instanceType = _compilerService.CompileType(context);
      var instance = (ITemplate)Activator.CreateInstance(instanceType);

      return instance;
    }

    internal ITemplate GetTemplate(string templateKey, string template, Type modelType, string saveLocation = null)
    {
      // TODO: thread safety around template cache?

      ITemplate razorTemplate;

      if (!_templateCache.TryGetValue(templateKey, out razorTemplate))
      {
        razorTemplate = CreateTemplate(templateKey, template, modelType, saveLocation);
      }

      _templateCache[templateKey] = razorTemplate;

      return razorTemplate;
    }

    internal string CompileAndRun<T>(string cacheKey, string template, T model, string saveLocation = null)
    {
      // TODO: thread safety on reads

      ITemplate razorTemplate;
      if (!_templateCache.TryGetValue(cacheKey, out razorTemplate))
      {
        Compile(cacheKey, template, model.GetType(), saveLocation);

        if (!_templateCache.TryGetValue(cacheKey, out razorTemplate))
        {
          throw new Exception("Unable to compile template.");
        }
      }

      SetService(razorTemplate, this);
      SetModel(razorTemplate, model);
      razorTemplate.Execute();

      return razorTemplate.Result;
    }
  }
}