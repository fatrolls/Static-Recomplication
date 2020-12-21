
Imports System.Collections.Generic
Imports System.Text
Imports System.Collections
Imports System.Diagnostics
Imports System.Runtime.InteropServices

<Flags()> _
Public Enum MemoryProtection As UInteger
    PAGE_NOACCESS = &H1
    PAGE_READONLY = &H2
    PAGE_READWRITE = &H4
    PAGE_WRITECOPY = &H8
    PAGE_EXECUTE = &H10
    PAGE_EXECUTE_READ = &H20
    PAGE_EXECUTE_READWRITE = &H40
    PAGE_EXECUTE_WRITECOPY = &H80
    PAGE_GUARD = &H100
    PAGE_NOCACHE = &H200
    PAGE_WRITECOMBINE = &H400
End Enum
Public Enum MemoryState As UInteger
    MEM_COMMIT = &H1000
    ' Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk. 
    MEM_RESERVE = &H2000
    ' Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage being allocated. For reserved pages, the information in the Protect member is undefined.
    MEM_FREE = &H10000
    ' Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the information in the AllocationBase, AllocationProtect, Protect, and Type members is undefined. 
End Enum
Public Enum MemoryType As UInteger
    MEM_IMAGE = &H1000000
    ' Indicates that the memory pages within the region are mapped into the view of an image section. 
    MEM_MAPPED = &H40000
    ' Indicates that the memory pages within the region are mapped into the view of a section. 
    MEM_PRIVATE = &H20000
    ' Indicates that the memory pages within the region are private (that is, not shared by other processes).
End Enum

Public Class MemoryRegion
    Public RegionAddress As IntPtr
    Public RegionSize As IntPtr

    Public Type As MemoryType
    Public State As MemoryState
    Public Protect As MemoryProtection

    Public Blocks As System.Collections.Generic.List(Of MemoryBlock) = New List(Of MemoryBlock)()

    Friend Sub New(ByVal mbi As MEMORY_BASIC_INFORMATION)
        RegionAddress = If(mbi.AllocationBase = IntPtr.Zero, mbi.BaseAddress, mbi.AllocationBase)
        RegionSize = mbi.RegionSize

        Type = mbi.Type
        State = mbi.State
        Protect = mbi.Protect
    End Sub

    ' overload operator +
    Public Shared Operator +(ByVal a As MemoryRegion, ByVal b As MemoryBlock) As MemoryRegion
        Debug.Assert(a.RegionAddress = b.RegionAddress)

        a.RegionSize = New IntPtr(a.RegionSize.ToInt64() + b.BlockSize.ToInt64())
        a.Protect = a.Protect Or b.Protect
        a.State = a.State Or b.State
        Return a
    End Operator
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If TypeOf obj Is MemoryRegion Then
            Return DirectCast(obj, MemoryRegion).RegionAddress.Equals(Me.RegionAddress)
        End If
        Return False
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return RegionAddress.GetHashCode()
    End Function
    Public Overrides Function ToString() As String
        Return String.Format("Region {0}, {1}", Me.RegionAddress.ToString("X"), Me.RegionSize.ToString("X"))
    End Function
End Class

Public Class MemoryBlock
    Public BlockAddress As IntPtr
    Public BlockSize As IntPtr
    Public RegionAddress As IntPtr

    Public Type As MemoryType
    Public State As MemoryState
    Public Protect As MemoryProtection
    Public AllocationProtect As MemoryProtection

    Friend Sub New(ByVal mbi As MEMORY_BASIC_INFORMATION)
        RegionAddress = If(mbi.AllocationBase = IntPtr.Zero, mbi.BaseAddress, mbi.AllocationBase)
        BlockAddress = mbi.BaseAddress
        BlockSize = mbi.RegionSize
        Type = mbi.Type
        State = mbi.State
        Protect = mbi.Protect
        AllocationProtect = mbi.AllocationProtect
    End Sub
    Friend ReadOnly Property Start() As Long
        Get
            Return BlockAddress.ToInt64()
        End Get
    End Property
    Friend ReadOnly Property [End]() As Long
        Get
            Return BlockAddress.ToInt64() + BlockSize.ToInt64()
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("Block {0}, {1}", Me.BlockAddress.ToString("X"), Me.BlockSize.ToString("X"))
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If TypeOf obj Is MemoryBlock Then
            Return DirectCast(obj, MemoryBlock).BlockAddress.Equals(Me.BlockAddress)
        End If
        Return False
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return BlockAddress.GetHashCode()
    End Function

    'public bool CanRead
    '{
    '    get
    '    {
    '        switch (Protect & ~(MemoryProtection.PAGE_GUARD | MemoryProtection.PAGE_NOCACHE | MemoryProtection.PAGE_WRITECOMBINE))
    '        {
    '            case MemoryProtection.PAGE_READONLY:
    '            case MemoryProtection.PAGE_READWRITE:
    '            case MemoryProtection.PAGE_WRITECOPY:
    '            case MemoryProtection.PAGE_EXECUTE_READ:
    '            case MemoryProtection.PAGE_EXECUTE_READWRITE:
    '            case MemoryProtection.PAGE_EXECUTE_WRITECOPY:
    '                return true;
    '        }
    '        return false;
    '    }
    '}
    'public bool CanWrite
    '{
    '    get
    '    {
    '        switch (Protect & ~(MemoryProtection.PAGE_GUARD | MemoryProtection.PAGE_NOCACHE | MemoryProtection.PAGE_WRITECOMBINE))
    '        {
    '            case MemoryProtection.PAGE_READWRITE:
    '            case MemoryProtection.PAGE_WRITECOPY:
    '            case MemoryProtection.PAGE_EXECUTE_READWRITE:
    '            case MemoryProtection.PAGE_EXECUTE_WRITECOPY:
    '                return true;
    '        }
    '        return false;
    '    }
    '}
End Class

Public Class MemorySnapshot
    Public Shared SystemInformation As SYSTEM_INFO
    'private Process m_Process;
    Private m_ProcessHandle As IntPtr = IntPtr.Zero

    Public Regions As New MemoryRegionCollection()
    Public Blocks As New MemoryBlockCollection()


    Shared Sub New()
        Kernel32.GetSystemInfo(SystemInformation)
    End Sub

    Public Sub New(ByVal process As Process)
        m_ProcessHandle = process.Handle
        Refresh()
    End Sub
    Public Sub New(ByVal processHandle As IntPtr)
        m_ProcessHandle = processHandle
        Refresh()
    End Sub
    Public Sub Refresh()
        Dim address As IntPtr = SystemInformation.lpMinimumApplicationAddress
        Dim mbi As New MEMORY_BASIC_INFORMATION()
        Dim length As New UIntPtr(CUInt(Marshal.SizeOf(mbi)))

        Blocks.Clear()
        Regions.Clear()

        While Kernel32.VirtualQueryEx(m_ProcessHandle, address, mbi, length) = length.ToUInt32()
            If mbi.RegionSize = New IntPtr(-1) Then
                Return
            End If

            ' Process Memory block
            Dim block As New MemoryBlock(mbi)
            Blocks.Add(block)

            If Regions.Count = 0 Then
                Dim region As New MemoryRegion(mbi)
                Regions.Add(region)
                region.Blocks.Add(block)
            Else
                Dim region As MemoryRegion = Regions(Regions.Count - 1)
                If region.RegionAddress = block.RegionAddress Then
                    region += block
                Else
                    region = New MemoryRegion(mbi)
                    Regions.Add(region)
                End If
                region.Blocks.Add(block)
            End If

            Try
                Dim [next] As Long = address.ToInt64() + mbi.RegionSize.ToInt64()
                address = New IntPtr([next])
            Catch err As Exception
                Trace.WriteLine(err)
                Return
            End Try
        End While
    End Sub
End Class

Public Class MemoryBlockCollection
    Inherits CollectionBase

    Friend Sub New()
    End Sub
    Public Function Add(ByVal Block As MemoryBlock) As Integer
        Return MyBase.InnerList.Add(Block)
    End Function
    Public Function Contains(ByVal Block As MemoryBlock) As Boolean
        Return MyBase.InnerList.Contains(Block)
    End Function
    Public Sub CopyTo(ByVal array As MemoryBlock(), ByVal index As Integer)
        MyBase.InnerList.CopyTo(array, index)
    End Sub
    Public Function IndexOf(ByVal Block As MemoryBlock) As Integer
        Return MyBase.InnerList.IndexOf(Block)
    End Function

    ' Properties
    Default Public ReadOnly Property Item(ByVal index As Integer) As MemoryBlock
        Get
            Return DirectCast(MyBase.InnerList(index), MemoryBlock)
        End Get
    End Property
    Public Function Find(ByVal startAddress As IntPtr, ByVal endAddress As IntPtr) As MemoryBlock()
        Debug.Assert(startAddress.ToInt64() <= endAddress.ToInt64())

        Dim start As Long = startAddress.ToInt64()
        Dim [end] As Long = endAddress.ToInt64()

        Dim blocks As New List(Of MemoryBlock)()
        For Each block As MemoryBlock In Me.InnerList
            If (start >= block.Start AndAlso start < block.[End]) OrElse ([end] > block.Start AndAlso [end] <= block.[End]) OrElse (block.Start > start AndAlso block.[End] < [end]) Then
                blocks.Add(block)
            End If
        Next
        Return blocks.ToArray()
    End Function
End Class

Public Class MemoryRegionCollection
    Inherits CollectionBase

    Friend Sub New()
    End Sub
    Public Function Add(ByVal region As MemoryRegion) As Integer
        Return MyBase.InnerList.Add(region)
    End Function

    Public Function Contains(ByVal region As MemoryRegion) As Boolean
        Return MyBase.InnerList.Contains(region)
    End Function

    Public Sub CopyTo(ByVal array As MemoryRegion(), ByVal index As Integer)
        MyBase.InnerList.CopyTo(array, index)
    End Sub
    Public Function IndexOf(ByVal address As IntPtr) As Integer
        For i As Integer = 0 To InnerList.Count - 1
            If Me(i).RegionAddress = address Then
                Return i
            End If
        Next
        Return -1
    End Function
    Public Function IndexOf(ByVal region As MemoryRegion) As Integer
        Return MyBase.InnerList.IndexOf(region)
    End Function

    ' Properties
    Default Public Property Item(ByVal index As Integer) As MemoryRegion
        Get
            Return DirectCast(MyBase.InnerList(index), MemoryRegion)
        End Get
        Set(ByVal value As MemoryRegion)
            MyBase.InnerList(index) = value
        End Set
    End Property
End Class
