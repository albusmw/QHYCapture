Option Explicit On
Option Strict On

Public Class frmXMLGeneration

    Public ReadOnly Property EXEPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    Private Sub btnBuildFile_Click(sender As Object, e As EventArgs) Handles btnBuildFile.Click

        Dim Code As New List(Of String)

        Code.Add("Imports Microsoft.VisualBasic")
        Code.Add("Imports System")
        Code.Add("Imports System.Text")
        Code.Add("Imports System.Drawing")
        Code.Add("Imports System.Diagnostics")
        Code.Add("Imports System.Collections.Generic")

        Code.Add("Public Class MainClass")
        Code.Add("Public Sub ExecuteCode")
        Code.AddRange(Split(tbCode.Text, System.Environment.NewLine))
        Code.Add("End Sub")
        Code.Add("End Class")

        RunXMLSequenceGen(Code.ToArray)

    End Sub

    Private Sub RunXMLSequenceGen(ByVal strCode As String())

        ' Creates object of the compiler
        Dim objCodeCompiler As New VBCodeProvider

        'References/Parameters.
        Dim objCompilerParameters As New System.CodeDom.Compiler.CompilerParameters()
        objCompilerParameters.ReferencedAssemblies.Add("System.dll")
        objCompilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll")
        objCompilerParameters.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll")
        objCompilerParameters.GenerateInMemory = True

        'Compiler Results
        Dim objCompileResults As System.CodeDom.Compiler.CompilerResults = objCodeCompiler.CompileAssemblyFromSource(objCompilerParameters, Join(strCode, System.Environment.NewLine))

        'If an Error occurs
        If objCompileResults.Errors.HasErrors Then
            MsgBox("Compiling error: Line > " & objCompileResults.Errors(0).Line.ToString & ", " & objCompileResults.Errors(0).ErrorText)
            Exit Sub
        End If

        'Creates assembly
        Dim objAssembly As System.Reflection.Assembly = objCompileResults.CompiledAssembly

        Dim objTheClass As Object = objAssembly.CreateInstance("MainClass")
        If objTheClass Is Nothing Then
            MsgBox("Can't load class...")
            Exit Sub
        End If

        'Try to excute
        Try
            objTheClass.GetType.InvokeMember("ExecuteCode", System.Reflection.BindingFlags.InvokeMethod, Nothing, objTheClass, Nothing)
        Catch ex As Exception
            If IsNothing(ex.InnerException) = True Then
                MsgBox("Execution error:" & ex.Message)
            Else
                MsgBox("Execution error:" & ex.Message & System.Environment.NewLine & " >> " & ex.InnerException.Message)
            End If
        End Try

    End Sub

    Private Sub frmXMLGeneration_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Load default code
        Dim DefaultCodeFile As String = System.IO.Path.Combine(EXEPath, "XMLGeneration.code.txt")
        If System.IO.File.Exists(DefaultCodeFile) Then
            tbCode.Text = System.IO.File.ReadAllText(DefaultCodeFile)
        End If
    End Sub

End Class