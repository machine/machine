using System;
using System.Collections.Generic;

using Machine.Container.Model;

namespace Machine.Container.Services
{
  [Flags]
  public enum LookupFlags 
  {
    None = 0,
    ThrowIfAmbiguous = 1,
    ThrowIfUnable = 2,
    Default = ThrowIfAmbiguous | ThrowIfUnable
  }
  public interface IServiceGraph
  {
    ServiceEntry Lookup(Type type, LookupFlags flags);
    ServiceEntry Lookup(Type type);
    ServiceEntry Lookup(string name);
    void Add(ServiceEntry entry);
    IEnumerable<ServiceRegistration> RegisteredServices { get; }
  }
}