using Atlantis.Framework.RazorEngine.Compilation;
using System.Dynamic;

namespace Atlantis.Framework.RazorEngine.Templating
{
  /// <summary>
  /// Provides a base implementation of a template with a model.
  /// </summary>
  /// <typeparam name="TModel">The model type.</typeparam>
  public abstract class TemplateBase<TModel> : TemplateBase, ITemplate<TModel>
  {
    private object _model;

    /// <summary>
    /// Initialises a new instance of <see cref="TemplateBase{TModel}"/>.
    /// </summary>
    public TemplateBase()
    {
      HasDynamicModel = GetType().IsDefined(typeof(HasDynamicModelAttribute), true);
    }

    /// <summary>
    /// Gets whether this template uses a dynamic model.
    /// </summary>
    protected bool HasDynamicModel { get; private set; }

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    public TModel Model
    {
      get { return (TModel)_model; }
      set
      {
        if (HasDynamicModel && !(value is DynamicObject) && !(value is ExpandoObject))
        {
          _model = new RazorDynamicObject {Model = value};
        }
        else
        {
          _model = value;
        }
      }
    }
  }
}