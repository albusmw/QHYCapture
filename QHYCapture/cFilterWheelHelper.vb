Option Explicit On
Option Strict On
Imports QHYCapture.ComponentModelEx

Public Class cFilterWheelHelper

    '''<summary>Filter as to be send as ASCII string.</summary>
    '''<remarks>Filter must NOT be in this order in the filter wheel !!!!</remarks>
    Public Enum eFilter As Byte
        <ComponentModel.Description("Unchanged")>
        Unchanged = 0
        <ComponentModel.Description("Light")>
        L = 1
        <ComponentModel.Description("Red")>
        R = 2
        <ComponentModel.Description("Green")>
        G = 3
        <ComponentModel.Description("Blue")>
        B = 4
        <ComponentModel.Description("H-alpha")>
        H_alpha = 5
        <ComponentModel.Description("S-II")>
        S_II = 6
        <ComponentModel.Description("O-III")>
        O_III = 7
    End Enum

    '''<summary>Constructor.</summary>
    '''<param name="FilterOrder">Physical filter otder in the wheel, separated by - (only the 1st character is relevant).</param>
    Public Sub New(ByVal FilterOrder As String)
        FilterOrderList = Split(FilterOrder, "-")
    End Sub
    '''<summary>Filter order list - !!! the 1st entry (index 0) is the first physical filter slot.</summary>
    Private FilterOrderList As String() = Array.Empty(Of String)()

    '''<summary>Get the filter slot (starting with 1) for the requested filter.</summary>
    '''<returns>Filter slot (starting with 1) the filter is positioned in the wheel.</returns>
    Public Function FilterSlot(ByVal RequestedFilter As String) As Integer
        For Idx As Integer = 0 To FilterOrderList.GetUpperBound(0)
            If FilterOrderList(Idx).Substring(0, 1).ToUpper = RequestedFilter.Substring(0, 1).ToUpper Then Return Idx + 1
        Next Idx
        Return -1
    End Function

    '''<summary>Get the filter slot (starting with 1) for the requested filter.</summary>
    '''<returns>Filter slot (starting with 1) the filter is positioned in the wheel.</returns>
    Public Function FilterSlot(ByVal RequestedFilter As eFilter) As Integer
        For Idx As Integer = 0 To FilterOrderList.GetUpperBound(0)
            If FilterOrderList(Idx).Substring(0, 1).ToUpper = RequestedFilter.ToString.Substring(0, 1).ToUpper Then Return Idx + 1
        Next Idx
        Return -1
    End Function

    '''<summary>Get the filter name (from the filter slot).</summary>
    '''<param name="FilterSlot">Filter slot as reported from the filter wheel.</param>
    Public Function FilterNameLong(ByVal FilterSlot As Integer) As String
        Return VerboseFilterList(FilterSlot)
    End Function

    '''<summary>Get the filter enum from the name.</summary>
    '''<param name="Name">Name (only the 1st character is relevant).</param>
    Public Function FilterNameToEnum(ByVal Name As String) As eFilter
        For Each SingleEnum As eFilter In [Enum].GetValues(GetType(eFilter))
            If SingleEnum.ToString.Substring(0, 1).ToUpper = Name.Substring(0, 1).ToUpper Then Return SingleEnum
        Next SingleEnum
        Return eFilter.Unchanged
    End Function

    '''<summary>Get a list of filter names as present in the wheel - index 0 is invalid as the position count starts with 1.</summary>
    <ComponentModel.Browsable(False)>
    Public ReadOnly Property VerboseFilterList() As String()
        Get
            Dim RetVal As New List(Of String)
            RetVal.Add("---")
            For Idx As Integer = 0 To FilterOrderList.GetUpperBound(0)
                Dim FilterEnum As eFilter = FilterNameToEnum(FilterOrderList(Idx))
                RetVal.Add(ComponentModelEx.EnumDesciptionConverter.GetEnumDescription(FilterEnum))
            Next Idx
            Return RetVal.ToArray
        End Get
    End Property

    '===================================================================================================================

    '''<summary>Get the filter name (from the enum).</summary>
    Public Shared Function FilterNameLong(ByVal FilterToName As eFilter) As String
        Return [Enum].GetName(GetType(eFilter), FilterToName)
    End Function

    '''<summary>Get the 1st letter from the selected filter name (from the enum).</summary>
    Public Shared Function FilterNameShort(ByVal FilterToName As eFilter) As String
        Return FilterNameLong(FilterToName).Substring(0, 1).ToUpper
    End Function

End Class
