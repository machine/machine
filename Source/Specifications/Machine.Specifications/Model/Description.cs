﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine.Specifications.Model
{
  public class Description
  {
    private List<Specification> _specifications;
    private object _instance;
    private IEnumerable<Context> _beforeEachs;
    private IEnumerable<Context> _beforeAlls;
    private IEnumerable<Context> _afterEachs;
    private IEnumerable<Context> _afterAlls;
    public string Name { get; private set; }
    public object Instance
    {
      get { return _instance; }
    }

    public IEnumerable<Specification> Specifications
    {
      get { return _specifications; }
    }

    public Type Type
    {
      get; private set;
    }

    public Description(Type type, object instance, IEnumerable<Context> beforeEachs, IEnumerable<Context> beforeAlls, IEnumerable<Context> afterEachs, IEnumerable<Context> afterAlls)
    {
      Name = type.Name.ReplaceUnderscores();
      Type = type;
      _instance = instance;
      _afterAlls = afterAlls;
      _afterEachs = afterEachs;
      _beforeAlls = beforeAlls;
      _beforeEachs = beforeEachs;
      _specifications = new List<Specification>();
    }

    public void AddSpecification(Specification specification)
    {
      _specifications.Add(specification);
    }

    public DescriptionVerificationResult Verify()
    {
      var verificationResults = VerifySpecifications();
      return new DescriptionVerificationResult(verificationResults);
    }

    private IEnumerable<SpecificationVerificationResult> VerifySpecifications()
    {
      _beforeAlls.InvokeAll();
      var results = ExecuteSpecifications();
      _afterAlls.InvokeAll();

      return results;
    }

    private IEnumerable<SpecificationVerificationResult> ExecuteSpecifications()
    {
      var results = new List<SpecificationVerificationResult>();
      foreach (Specification specification in _specifications)
      {
        var result = VerifySpecification(specification);
        results.Add(result);
      }

      return results;
    }

    public SpecificationVerificationResult VerifySpecification(Specification specification)
    {
      VerificationContext context = new VerificationContext(_instance);
      try
      {
        _beforeEachs.InvokeAll();
      }
      catch (Exception err)
      {
        return new SpecificationVerificationResult(err);
      }

      var result = specification.Verify(context);
      try
      {
        _afterEachs.InvokeAll();
      }
      catch (Exception err)
      {
        return new SpecificationVerificationResult(err);
      }

      return result;
    }

    public void RunContextBeforeAll()
    {
      _beforeAlls.InvokeAll();
    }

    public void RunContextAfterAll()
    {
      _afterAlls.InvokeAll();
    }
  }
}
