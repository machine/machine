using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Metadata.Reader.API;

namespace Machine.Specifications.ReSharperRunner
{
  internal static class MetadataExtensions
  {
    public static bool IsContext(this IMetadataTypeInfo type)
    {
      if (!type.IsAbstract && type.GenericParameters.Length == 0)
      {
        return type.HasSpecifications();
      }
      return false;
    }

    public static bool HasSpecifications(this IMetadataTypeInfo type)
    {
      return type.GetSpecifications().Any() || type.GetBehaviors().Any();
    }

    public static IEnumerable<IMetadataField> GetSpecifications(this IMetadataTypeInfo type)
    {
      return type.GetPrivateFieldsOfType<It>();
    }

    public static IEnumerable<IMetadataField> GetBehaviors(this IMetadataTypeInfo type)
    {
      return type.GetPrivateFieldsWith(typeof(Behaves_like<>));
    }

    static IEnumerable<IMetadataField> GetPrivateFields(this IMetadataTypeInfo type)
    {
      return type.GetFields().Where(field => !field.IsStatic);
    }

    static IEnumerable<IMetadataField> GetPrivateFieldsOfType<T>(this IMetadataTypeInfo type)
    {
      // HACK: String comparison.
      return type.GetPrivateFields().Where(x => x.Type.PresentableName == typeof(T).FullName);
    }

    static IEnumerable<IMetadataField> GetPrivateFieldsWith(this IMetadataTypeInfo type, Type fieldType)
    {
      // HACK: String comparison.
      return type.GetPrivateFields().Where(x => x.Type.PresentableName.StartsWith(fieldType.FullName));
    }
  }
}