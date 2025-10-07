namespace UnitTests_TextParser

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type Test1 () =

    [<AssemblyInitialize>]
    static member AssemblyInit (context: TestContext) =
        // This method is called once for the test assembly, before any tests are run.
        ()

    [<AssemblyCleanup>]
    static member AssemblyCleanup () =
        // This method is called once for the test assembly, after all tests are run.
        ()

    [<ClassInitialize>]
    member this.ClassInit (context: TestContext) =
        // This method is called once for the test class, before any tests of the class are run.
        ()

    [<ClassCleanup>]
    member this.ClassCleanup () =
        // This method is called once for the test class, after all tests of the class are run.
        ()

    [<TestInitialize>]
    member this.TestInit () =
        // This method is called before each test method.
        ()

    [<TestCleanup>]
    member this.TestCleanup () =
        // This method is called after each test method.
        ()

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
