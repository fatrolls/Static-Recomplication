
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Diagnostics
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles
Imports System.ComponentModel

<Serializable(), ComVisible(True), Flags()> _
Public Enum ProcessAccess
    Read = 1
    ReadWrite = 3
    Write = 2
End Enum

<SecurityPermission(SecurityAction.Demand, Flags:=SecurityPermissionFlag.UnmanagedCode)> _
Public NotInheritable Class ProcessMemoryStream
    Inherits Stream
    Private m_ProcessHandle As IntPtr
    'private int m_processID;
    Private m_VirtualMemory As MemorySnapshot

    ' Fields
    Private m_canRead As Boolean
    Private m_canSeek As Boolean
    Private m_canWrite As Boolean
    Private m_pos As Long
    Private m_ExposedHandle As Boolean

    Friend Sub New()
        MyBase.New()
        Me.m_ProcessHandle = IntPtr.Zero
        Me.m_VirtualMemory = Nothing
        'this.m_processID = 0;
        Me.m_pos = 0
        Me.m_canRead = False
        Me.m_canSeek = True
        Me.m_canWrite = False

        ' TODO
        m_ExposedHandle = False
    End Sub

    Public Sub New(ByVal processID As Integer)
        Me.New(processID, ProcessAccess.ReadWrite)
    End Sub

    Public Sub New(ByVal processID As Integer, ByVal desiredAcces As ProcessAccess)
        MyBase.New()
        Dim test As Process = Process.GetProcessById(processID)
        If test.HasExited Then
            Throw New ArgumentException("Process has existed!")
        End If

        Dim access As Integer = CInt(Win32ProcessAccess.PROCESS_QUERY_INFORMATION)
        ' Minimum access
        Select Case desiredAcces
            Case ProcessAccess.Read
                access = access Or CInt(Win32ProcessAccess.PROCESS_VM_READ)
                m_canRead = True
                m_canWrite = False
                Exit Select
            Case ProcessAccess.ReadWrite
                access = access Or CInt(Win32ProcessAccess.PROCESS_VM_READ) Or CInt(Win32ProcessAccess.PROCESS_VM_WRITE) Or CInt(Win32ProcessAccess.PROCESS_VM_OPERATION)
                m_canRead = True
                m_canWrite = True
                Exit Select
            Case ProcessAccess.Write
                access = access Or CInt(Win32ProcessAccess.PROCESS_VM_WRITE) Or CInt(Win32ProcessAccess.PROCESS_VM_OPERATION)
                m_canRead = False
                m_canWrite = True
                Exit Select
        End Select
        m_ProcessHandle = Kernel32.OpenProcess(access, False, processID)
        If m_ProcessHandle = IntPtr.Zero Then
            Throw New Exception("OpenProcess failed with errorcode " + Convert.ToString(Marshal.GetLastWin32Error()))
        End If

        m_VirtualMemory = New MemorySnapshot(m_ProcessHandle)
        Me.m_pos = 0
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        SyncLock Me
            Try
                If disposing Then
                    If (Me.m_ProcessHandle <> IntPtr.Zero) Then
                        Kernel32.CloseHandle(m_ProcessHandle)
                        m_ProcessHandle = IntPtr.Zero
                    End If
                End If
                Me.m_canRead = False
                Me.m_canWrite = False
                Me.m_canSeek = False
            Finally
                MyBase.Dispose(disposing)
            End Try
        End SyncLock
    End Sub

    Protected Overrides Sub Finalize()
        Try
            Me.Dispose(False)
        Finally
            MyBase.Finalize()
        End Try
    End Sub

    Public Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return m_canRead
        End Get
    End Property

    Public Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return m_canSeek
        End Get
    End Property

    Public Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return m_canWrite
        End Get
    End Property

    Public Overrides Sub Flush()
        'throw new Exception("The method or operation is not implemented.");
    End Sub

    Public Overrides ReadOnly Property Length() As Long
        Get
            Dim lastRegion As MemoryRegion = m_VirtualMemory.Regions(m_VirtualMemory.Regions.Count - 1)
            Return lastRegion.RegionAddress.ToInt64() + lastRegion.RegionSize.ToInt64()
        End Get
    End Property

    Public Overrides Property Position() As Long
        Get
            CheckInternalState()
            Return m_pos
        End Get
        Set(ByVal value As Long)
            CheckInternalState()

            If value < 0 OrElse value > Me.Length Then
                Throw New ArgumentOutOfRangeException("value", "value must be in range of process memory.")
            End If
            Me.Seek(value, SeekOrigin.Begin)
        End Set
    End Property

    Private Sub CheckInternalState()
        If Me.m_ProcessHandle = IntPtr.Zero Then
            Throw New InvalidDataException("Process handle is closed or invalid!")
        End If
        'this.VerifyOSHandlePosition(); TODO
        If Me.m_ExposedHandle Then
        End If
    End Sub

    Public Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
        CheckInternalState()

        If (origin < SeekOrigin.Begin) OrElse (origin > SeekOrigin.[End]) Then
            Throw New ArgumentException("Invalid seek origin", "origin")
        End If

        Dim proposed As Long = 0
        Select Case origin
            Case SeekOrigin.Begin
                proposed = offset
                Exit Select
            Case SeekOrigin.Current
                proposed += offset
                Exit Select
            Case SeekOrigin.[End]
                proposed = Length - offset
                'CHECK TODO
                Exit Select
        End Select
        If proposed < 0 OrElse proposed > Me.Length Then
            Throw New ArgumentOutOfRangeException("offset", "Position must be in range of process memory.")
        End If
        m_pos = proposed
        Return m_pos
    End Function

    ''' <summary>
    ''' reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
    ''' </summary>
    ''' <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source. </param>
    ''' <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream. </param>
    ''' <param name="count">The maximum number of bytes to be read from the current stream.</param>
    ''' <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    Public Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
        If buffer Is Nothing Then
            Throw New ArgumentNullException("buffer", "Argument may not be null.")
        End If
        If offset < 0 Then
            Throw New ArgumentOutOfRangeException("offset", "Invalid offset")
        End If
        If count < 0 Then
            Throw New ArgumentOutOfRangeException("count", "Invalid argument")
        End If
        If (buffer.Length - offset) < count Then
            Throw New ArgumentException("Size of buffer to small.")
        End If
        If m_pos + count > Me.Length Then
            ' TODO Check
            count = Convert.ToInt32(Length - m_pos)
        End If

        Dim [error] As Integer = 0
        Dim numBytesRead As Integer = 0

        Dim address As New IntPtr(m_pos)
        Dim endAddress As New IntPtr(m_pos + count)
        Dim readBlocks As MemoryBlock() = m_VirtualMemory.Blocks.Find(address, endAddress)

        For Each block As MemoryBlock In readBlocks
            Dim bytesThisBlock As Long = 0
            If endAddress.ToInt64() > block.[End] Then
                ' Will read over this block
                bytesThisBlock = block.[End] - m_pos
            Else
                bytesThisBlock = endAddress.ToInt64() - m_pos
            End If
            ' Read till end of block
            If block.State = MemoryState.MEM_COMMIT Then
                numBytesRead = ReadProcessMemoryNative(buffer, offset, CInt(bytesThisBlock), [error])
            Else
                numBytesRead = Convert.ToInt32(bytesThisBlock)
            End If
            offset += numBytesRead
            m_pos += numBytesRead
        Next
        Return numBytesRead
    End Function
    ''' <summary>
    ''' Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
    ''' </summary>
    ''' <param name="buffer"></param>
    ''' <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream. </param>
    ''' <param name="count">The number of bytes to be written to the current stream. </param>
    Public Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
        If buffer Is Nothing Then
            Throw New ArgumentNullException("buffer", "Argument may not be null.")
        End If
        If offset < 0 Then
            Throw New ArgumentOutOfRangeException("offset", "Invalid offset")
        End If
        If count < 0 Then
            Throw New ArgumentOutOfRangeException("count", "Invalid argument")
        End If
        If (buffer.Length - offset) < count Then
            Throw New ArgumentException("Size of buffer to small.")
        End If
        If m_pos + count > Length Then
            count = CInt(Length - m_pos)
        End If
        Dim [error] As Integer = 0
        Dim numBytesWritten As Integer = WriteProcessMemoryNative(buffer, offset, count, [error])
        m_pos += numBytesWritten

    End Sub


    Public Overrides Sub SetLength(ByVal value As Long)
        Throw New Exception("The method or operation is not implemented.")
    End Sub

#Region "Private methods"
    Private Function ReadProcessMemoryNative(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer, ByRef Win32Error As Integer) As Integer
        Dim numBytesRead As IntPtr = IntPtr.Zero

        Dim address As New IntPtr(m_pos)
        Dim tempBuffer As Byte()
        ReDim tempBuffer(count + 1)
        If Kernel32.ReadProcessMemory(m_ProcessHandle, address, tempBuffer, count, numBytesRead) = False Then
            Throw New Win32Exception(Marshal.GetLastWin32Error())
        End If

        Buffer.BlockCopy(tempBuffer, 0, bytes, offset, count)

        Win32Error = 0
        Return numBytesRead.ToInt32()
    End Function

    Private Function WriteProcessMemoryNative(ByVal bytes As Byte(), ByVal offset As Integer, ByVal count As Integer, ByRef Win32Error As Integer) As Integer
        If bytes.Length = 0 Then
            Win32Error = 0
            Return 0
        End If
        Dim numBytesWritten As IntPtr = IntPtr.Zero
        Dim address As New IntPtr(m_pos)
        Dim tempBuffer As Byte()
        ReDim tempBuffer(count + 1)

        If Kernel32.WriteProcessMemory(m_ProcessHandle, address, tempBuffer, count, numBytesWritten) = False Then
            Win32Error = Marshal.GetLastWin32Error()
            Throw New Win32Exception(Marshal.GetLastWin32Error())
        End If

        Buffer.BlockCopy(tempBuffer, 0, bytes, offset, count)

        Win32Error = 0
        Return numBytesWritten.ToInt32()
    End Function
#End Region
End Class