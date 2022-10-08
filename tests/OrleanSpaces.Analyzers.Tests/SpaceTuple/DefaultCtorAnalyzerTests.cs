using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = OrleanSpaces.Analyzers.Test.CSharpCodeFixVerifier<
    OrleanSpaces.Analyzers.SpaceTuple.DefaultCtorAnalyzer,
    OrleanSpaces.Analyzers.SpaceTuple.DefaultCtorFixer>;

namespace OrleanSpaces.Analyzers.Tests.SpaceTuple;

//[TestClass]
//public class DefaultCtorAnalyzerTests
//{
//    [TestMethod]
//    public async Task TestMethod1()
//    {
//        var test = @"";
//        await VerifyCS.VerifyAnalyzerAsync(test);
//    }

//    [TestMethod]
//    public async Task TestMethod2()
//    {
//        var test = @"
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using System.Diagnostics;

//    namespace ConsoleApplication1
//    {
//        class {|#0:TypeName|}
//        {   
//        }
//    }";

//        var fixtest = @"
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using System.Diagnostics;

//    namespace ConsoleApplication1
//    {
//        class TYPENAME
//        {   
//        }
//    }";

//        var expected = VerifyCS.Diagnostic("OrleanSpacesAnalyzers").WithLocation(0).WithArguments("TypeName");
//        await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
//    }
//}
