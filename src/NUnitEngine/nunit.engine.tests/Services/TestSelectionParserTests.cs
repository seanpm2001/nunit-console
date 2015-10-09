﻿// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NUnit.Engine.Services.Tests
{
    public class TestSelectionParserTests
    {
        private TestSelectionParser _parser;

        [SetUp]
        public void CreateParser()
        {
            _parser = new TestSelectionParser();
        }

        [TestCase("cat=Urgent", "<cat>Urgent</cat>")]
        [TestCase("cat==Urgent", "<cat>Urgent</cat>")]
        [TestCase("cat!=Urgent", "<not><cat>Urgent</cat></not>")]
        [TestCase("cat =~ Urgent", "<cat op='=~'>Urgent</cat>")]
        [TestCase("cat !~ Urgent", "<cat op='!~'>Urgent</cat>")]
        [TestCase("cat = Urgent || cat = High", "<or><cat>Urgent</cat><cat>High</cat></or>")]
        [TestCase("name='SomeTest'", "<name>SomeTest</name>")]
        [TestCase("method=TestMethod", "<method>TestMethod</method>")]
        [TestCase("method=Test1||method=Test2||method=Test3", "<or><method>Test1</method><method>Test2</method><method>Test3</method></or>")]
        [TestCase("test='My.Test.Fixture.Method(42)'", "<test>My.Test.Fixture.Method(42)</test>")]
        [TestCase("cat==Urgent && test=='My.Tests'", "<and><cat>Urgent</cat><test>My.Tests</test></and>")]
        [TestCase("cat==Urgent and test=='My.Tests'", "<and><cat>Urgent</cat><test>My.Tests</test></and>")]
        [TestCase("cat==Urgent || test=='My.Tests'", "<or><cat>Urgent</cat><test>My.Tests</test></or>")]
        [TestCase("cat==Urgent or test=='My.Tests'", "<or><cat>Urgent</cat><test>My.Tests</test></or>")]
        [TestCase("cat==Urgent || test=='My.Tests' && cat == high", "<or><cat>Urgent</cat><and><test>My.Tests</test><cat>high</cat></and></or>")]
        [TestCase("cat==Urgent && test=='My.Tests' || cat == high", "<or><and><cat>Urgent</cat><test>My.Tests</test></and><cat>high</cat></or>")]
        [TestCase("cat==Urgent && (test=='My.Tests' || cat == high)", "<and><cat>Urgent</cat><or><test>My.Tests</test><cat>high</cat></or></and>")]
        [TestCase("cat==Urgent && !(test=='My.Tests' || cat == high)", "<and><cat>Urgent</cat><not><or><test>My.Tests</test><cat>high</cat></or></not></and>")]
        public void TestParser(string input, string output)
        {
            Assert.That(_parser.Parse(input), Is.EqualTo(output));
        }

        [TestCase(null, typeof(ArgumentNullException))]
        [TestCase("", typeof(NUnitEngineException))]
        [TestCase("   ", typeof(NUnitEngineException))]
        [TestCase("  \t\t ", typeof(NUnitEngineException))]
        public void TestParser_InvalidInput(string input, Type type)
        {
            Assert.That(() => _parser.Parse(input), Throws.TypeOf(type));
        }
    }
}
