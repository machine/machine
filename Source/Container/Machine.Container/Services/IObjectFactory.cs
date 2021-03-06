using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  public interface IObjectFactory
  {
    object CreateObject(ConstructorCandidate constructor, object[] parameters);
  }
}