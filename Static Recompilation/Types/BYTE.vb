
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
'using BlackStorm.Reversing.Win32;
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.ComponentModel
Imports System.Globalization
Imports System.ComponentModel.Design.Serialization
Imports System.Reflection
Imports System.Diagnostics

Public Structure [BYTE]
#Region "fields"
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Value As Byte
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Offset As UInteger
#End Region

#Region "properties"
    Public Property Offset() As UInteger
        Get
            Return m_Offset
        End Get
        Set(ByVal value As UInteger)
            m_Offset = value
        End Set
    End Property
    Public Property Value() As Byte
        Get
            Return m_Value
        End Get
        Set(ByVal value As Byte)
            m_Value = value
        End Set
    End Property
#End Region

#Region "constructors"
    Public Sub New(ByVal arg As Byte)
        m_Value = arg
        m_Offset = 0
    End Sub
    Public Sub New(ByVal arg As SByte)
        m_Value = CByte(arg)
        m_Offset = 0
    End Sub
#End Region

#Region "implicit convertors"
    Public Shared Widening Operator CType(ByVal arg As SByte) As [BYTE]
        Return New [BYTE](arg)
    End Operator
    Public Shared Widening Operator CType(ByVal arg As Byte) As [BYTE]
        Return New [BYTE](arg)
    End Operator
    Public Shared Widening Operator CType(ByVal arg As [BYTE]) As SByte
        Return CSByte(arg.m_Value)
    End Operator
    Public Shared Widening Operator CType(ByVal arg As [BYTE]) As Byte
        Return arg.m_Value
    End Operator
    'public static implicit operator long(BYTE arg)
    '{
    '    return (long)arg.m_Value;
    '}
    'public static implicit operator ulong(BYTE arg)
    '{
    '    return arg.m_Value;
    '}
#End Region

#Region "overrides"
    Public Overrides Function ToString() As String
        Return m_Value.ToString("X2")
    End Function
    Public Overloads Function ToString(ByVal format As String) As String
        Return m_Value.ToString(format)
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return m_Value.GetHashCode()
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not (TypeOf obj Is [BYTE]) Then
            Return False
        End If

        Return Me.Equals(CType(obj, [BYTE]))
    End Function
    Public Overloads Function Equals(ByVal obj As [BYTE]) As [Boolean]
        Return Me.m_Value.Equals(obj.m_Value)
    End Function
#End Region

#Region "Parsing"
    Public Shared Function Parse(ByVal sHex As String) As [BYTE]
        Return Byte.Parse(sHex, System.Globalization.NumberStyles.HexNumber)
    End Function
    Public Shared Function TryParse(ByVal sHex As String, ByRef value As Byte) As Boolean
        Return Byte.TryParse(sHex, System.Globalization.NumberStyles.HexNumber, Nothing, value)
    End Function
#End Region

#Region "static methods"
    Public Shared Function Read(ByVal reader As BinaryReader) As [BYTE]
        Dim ret As New [BYTE]()
        ret.m_Offset = CUInt(reader.BaseStream.Position)
        ret.m_Value = reader.ReadByte()
        Return ret
    End Function
#End Region
End Structure

#Region "convertor class"
Public Class BYTEConverter
    Inherits TypeConverter
    ' Methods
    Public Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
        Return ((sourceType = GetType(String)) OrElse MyBase.CanConvertFrom(context, sourceType))
    End Function

    Public Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destinationType As Type) As Boolean
        Return ((destinationType = GetType(InstanceDescriptor)) OrElse MyBase.CanConvertTo(context, destinationType))
    End Function

    Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
        If TypeOf value Is String Then
            Dim val As Byte = 0
            If Byte.TryParse(DirectCast(value, String).Trim(), System.Globalization.NumberStyles.HexNumber, Nothing, val) Then
                Return New [BYTE](val)
            End If
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
        If destinationType Is Nothing Then
            Throw New ArgumentNullException("destinationType")
        End If
        If (destinationType = GetType(InstanceDescriptor)) AndAlso (TypeOf value Is [BYTE]) Then
            Dim member As ConstructorInfo = GetType([BYTE]).GetConstructor(New Type() {GetType(Byte)})
            If member IsNot Nothing Then
                Dim val As String = value.ToString()
                Dim result As Byte = 0
                If Byte.TryParse(val, result) Then
                    Return New InstanceDescriptor(member, New Object() {result})
                End If
            End If
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class
#End Region
