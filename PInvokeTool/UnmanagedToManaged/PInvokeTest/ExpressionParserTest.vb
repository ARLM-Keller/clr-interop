﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke.Parser


'''<summary>
'''This is a test class for PInvoke.Parser.ExpressionParser and is intended
'''to contain all PInvoke.Parser.ExpressionParser Unit Tests
'''</summary>
<TestClass()> _
Public Class ExpressionParserTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

    '''<summary>
    '''A test for TryParse(ByVal String, ByRef PInvoke.Parser.ExpressionNode)
    '''</summary>
    <TestMethod()> _
    Public Sub Parse1()
        Dim parser As New ExpressionParser()
        Dim node As ExpressionNode = parser.Parse("1+1")
        Assert.AreEqual(node.DisplayString, "+ (Left: 1)(Right: 1)")
    End Sub

    <TestMethod()> _
    Public Sub Parse2()
        Dim parser As New ExpressionParser()
        Dim node As ExpressionNode = parser.Parse("'c'")
        Assert.AreEqual(node.DisplayString, "'c'")
    End Sub

    <TestMethod()> _
    Public Sub Parse3()
        Dim parser As New ExpressionParser()
        Dim node As ExpressionNode = parser.Parse("'c'+2")
        Assert.AreEqual(node.DisplayString, "+ (Left: 'c')(Right: 2)")
    End Sub

    ''' <summary>
    ''' Basic math operators
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub ParseMathOperations()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("1+1")
        Assert.AreEqual(node.DisplayString, "+ (Left: 1)(Right: 1)")
        node = p.Parse("1-1")
        Assert.AreEqual(node.DisplayString, "- (Left: 1)(Right: 1)")
        node = p.Parse("1/1")
        Assert.AreEqual(node.DisplayString, "/ (Left: 1)(Right: 1)")
        node = p.Parse("1%1")
        Assert.AreEqual(node.DisplayString, "% (Left: 1)(Right: 1)")

    End Sub

    <TestMethod()> _
    Public Sub ParseBooleanOperations()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("1&&1")
        Assert.AreEqual(node.DisplayString, "&& (Left: 1)(Right: 1)")
        node = p.Parse("1||1")
        Assert.AreEqual(node.DisplayString, "|| (Left: 1)(Right: 1)")
    End Sub


    <TestMethod()> _
    Public Sub ParseBitwiseOperations()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("1|1")
        Assert.AreEqual(node.DisplayString, "| (Left: 1)(Right: 1)")
        node = p.Parse("1&1")
        Assert.AreEqual(node.DisplayString, "& (Left: 1)(Right: 1)")
    End Sub

    <TestMethod()> _
    Public Sub Paren1()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("(1)")
        Assert.AreEqual(node.DisplayString, "1")
        node = p.Parse("(     1)")
        Assert.AreEqual(node.DisplayString, "1")
    End Sub

    <TestMethod()> _
    Public Sub Paren2()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("1+(2+3)")
        Assert.AreEqual(node.DisplayString, "+ (Left: 1)(Right: + (Left: 2)(Right: 3))")
        node = p.Parse("(1+2)+3")
        Assert.AreEqual(node.DisplayString, "+ (Left: + (Left: 1)(Right: 2))(Right: 3)")
    End Sub

    <TestMethod()> _
    Public Sub CallExpr1()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("defined(foo)")
        Assert.AreEqual("defined (Left: foo)", node.DisplayString)
    End Sub

    <TestMethod()> _
    Public Sub CallExpr2()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("defined(foo, bar)")
        Assert.AreEqual("defined (Left: foo)(Right: , (Left: bar))", node.DisplayString)
    End Sub

    <TestMethod()> _
    Public Sub CallExpr3()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("defined(foo, 1+1)")
        Assert.AreEqual(node.DisplayString, "defined (Left: foo)(Right: , (Left: + (Left: 1)(Right: 1)))")
    End Sub

    <TestMethod()> _
    Public Sub Shift1()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("1>>2")
        Assert.AreEqual(node.DisplayString, ">> (Left: 1)(Right: 2)")
    End Sub

    <TestMethod()> _
    Public Sub Shift2()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("1<<2")
        Assert.AreEqual(node.DisplayString, "<< (Left: 1)(Right: 2)")
    End Sub

    <TestMethod()> _
    Public Sub Cast1()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("(FOO)1")
        Assert.AreEqual(node.DisplayString, "FOO (Left: 1)")
    End Sub

    <TestMethod()> _
    Public Sub Cast2()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("(BAR)(42)")
        Assert.AreEqual(node.DisplayString, "BAR (Left: 42)")
    End Sub

    <TestMethod()> _
    Public Sub Cast3()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode

        node = p.Parse("(FOO)(BAR)1")
        Assert.AreEqual(node.DisplayString, "FOO (Left: BAR (Left: 1))")
    End Sub

    <TestMethod()> _
    Public Sub Complex1()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode = p.Parse("((WORD)((DWORD_PTR)(l) >> 16))")
        Assert.AreEqual(node.DisplayString, "WORD (Left: DWORD_PTR (Left: >> (Left: l)(Right: 16)))")
    End Sub

    <TestMethod()> _
    Public Sub Negative1()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode = p.Parse("-1")
        Assert.AreEqual(node.DisplayString, "- (Left: 1)")
    End Sub

    <TestMethod()> _
    Public Sub Negative2()
        Dim p As New ExpressionParser()
        Dim node As ExpressionNode = p.Parse("-0.1F")
        Assert.AreEqual(node.DisplayString, "- (Left: 0.1F)")
    End Sub

End Class
