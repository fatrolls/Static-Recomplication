
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Reflection

<StructLayout(LayoutKind.Sequential)> _
Public Structure PROCESS_INFORMATION
    Public hProcess As IntPtr
    Public hThread As IntPtr
    Public dwProcessId As Integer
    Public dwThreadId As Integer
End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure SECURITY_ATTRIBUTES
    Public nLength As Integer
    Public lpSecurityDescriptor As IntPtr
    Public bInheritHandle As Integer
End Structure
<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Public Structure STARTUPINFO
    Public cb As Int32
    Public lpReserved As String
    Public lpDesktop As String
    Public lpTitle As String
    Public dwX As Int32
    Public dwY As Int32
    Public dwXSize As Int32
    Public dwYSize As Int32
    Public dwXCountChars As Int32
    Public dwYCountChars As Int32
    Public dwFillAttribute As Int32
    Public dwFlags As Int32
    Public wShowWindow As Int16
    Public cbReserved2 As Int16
    Public lpReserved2 As IntPtr
    Public hStdInput As IntPtr
    Public hStdOutput As IntPtr
    Public hStdError As IntPtr
End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure SYSTEM_INFO
    Friend uProcessorInfo As _PROCESSOR_INFO_UNION
    Public dwPageSize As UInteger
    Public lpMinimumApplicationAddress As IntPtr
    Public lpMaximumApplicationAddress As IntPtr
    Public dwActiveProcessorMask As IntPtr
    Public dwNumberOfProcessors As UInteger
    Public dwProcessorType As UInteger
    Public dwAllocationGranularity As UInteger
    Public dwProcessorLevel As UShort
    Public dwProcessorRevision As UShort
End Structure

<StructLayout(LayoutKind.Explicit)> _
Public Structure _PROCESSOR_INFO_UNION
    <FieldOffset(0)> _
    Friend dwOemId As UInteger
    <FieldOffset(0)> _
    Friend wProcessorArchitecture As UShort
    <FieldOffset(2)> _
    Friend wReserved As UShort
End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure MEMORY_BASIC_INFORMATION
    Public BaseAddress As IntPtr
    Public AllocationBase As IntPtr
    Public AllocationProtect As MemoryProtection
    Public RegionSize As IntPtr
    Public State As MemoryState
    Public Protect As MemoryProtection
    Public Type As MemoryType

    Public Overrides Function ToString() As String
        Return String.Format("BaseAddress: {0}" & vbLf & "AllocationBase: {1}" & vbLf & "AllocationProtect: {2}" & vbLf & "RegionSize: {3}", BaseAddress.ToString("X"), AllocationBase.ToString("X"), AllocationProtect.ToString(), RegionSize.ToString("X"))
    End Function
End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure MODULEINFO
    Public lpBaseOfDll As IntPtr
    Public SizeOfImage As UInteger
    Public EntryPoint As IntPtr
End Structure

Public Class PSAPI
    <DllImport("psapi.dll", SetLastError:=True, ExactSpelling:=False, CharSet:=CharSet.Auto)> _
    Public Shared Function GetMappedFileName(ByVal hProcess As IntPtr, ByVal lpv As IntPtr, ByVal lpFilename As StringBuilder, ByVal nSize As UInteger) As UInteger
    End Function

    <DllImport("psapi.dll", SetLastError:=True)> _
    Public Shared Function GetModuleInformation(ByVal hProcess As IntPtr, ByVal hModule As IntPtr, ByRef lpmodinfo As MODULEINFO, ByVal countBytes As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("psapi.dll", SetLastError:=True)> _
    Public Shared Function GetModuleBaseName(ByVal hProcess As IntPtr, ByVal hModule As IntPtr, <Out()> ByVal lpBaseName As StringBuilder, <[In]()> <MarshalAs(UnmanagedType.U4)> ByVal nSize As Integer) As UInteger
    End Function

    <DllImport("psapi.dll", SetLastError:=True, ExactSpelling:=False, CharSet:=CharSet.Auto)> _
    Public Shared Function GetModuleFileNameEx(ByVal hProcess As IntPtr, ByVal hModule As IntPtr, <Out()> ByVal lpFilename As StringBuilder, <[In]()> <MarshalAs(UnmanagedType.U4)> ByVal nSize As Integer) As UInteger
    End Function
End Class


<Flags()> _
Public Enum Win32ProcessAccess As UInteger
    PROCESS_TERMINATE = &H1
    PROCESS_CREATE_THREAD = &H2
    PROCESS_SET_SESSIONID = &H4
    PROCESS_VM_OPERATION = &H8
    PROCESS_VM_READ = &H10
    PROCESS_VM_WRITE = &H20
    PROCESS_DUP_HANDLE = &H40
    PROCESS_CREATE_PROCESS = &H80
    PROCESS_SET_QUOTA = &H100
    PROCESS_SET_INFORMATION = &H200
    PROCESS_QUERY_INFORMATION = &H400
    PROCESS_SUSPEND_RESUME = &H800
    PROCESS_QUERY_LIMITED_INFORMATION = &H1000
    'PROCESS_ALL_ACCESS = 0x00100000L | 0x000F0000L | 0xFFFF 
End Enum

Public Enum ThreadAccess As Integer
    None = 0
    THREAD_ALL_ACCESS = (&H1F03FF)
    THREAD_DIRECT_IMPERSONATION = (&H200)
    THREAD_GET_CONTEXT = (&H8)
    THREAD_IMPERSONATE = (&H100)
    THREAD_QUERY_INFORMATION = (&H40)
    THREAD_QUERY_LIMITED_INFORMATION = (&H800)
    THREAD_SET_CONTEXT = (&H10)
    THREAD_SET_INFORMATION = (&H20)
    THREAD_SET_LIMITED_INFORMATION = (&H400)
    THREAD_SET_THREAD_TOKEN = (&H80)
    THREAD_SUSPEND_RESUME = (&H2)
    THREAD_TERMINATE = (&H1)

End Enum

<StructLayout(LayoutKind.Sequential)> _
Public Structure FLOATING_SAVE_AREA
    Public ControlWord As UInteger
    Public StatusWord As UInteger
    Public TagWord As UInteger
    Public ErrorOffset As UInteger
    Public ErrorSelector As UInteger
    Public DataOffset As UInteger
    Public DataSelector As UInteger
    Public RegisterArea As Byte()
    ' 80
    Public Cr0NpxState As UInteger
End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure CONTEXT

    '
    ' The flags values within this flag control the contents of
    ' a CONTEXT record.
    '
    ' If the context record is used as an input parameter, then
    ' for each portion of the context record controlled by a flag
    ' whose value is set, it is assumed that that portion of the
    ' context record contains valid context. If the context record
    ' is being used to modify a threads context, then only that
    ' portion of the threads context will be modified.
    '
    ' If the context record is used as an IN OUT parameter to capture
    ' the context of a thread, then only those portions of the thread's
    ' context corresponding to set flags will be returned.
    '
    ' The context record is never used as an OUT only parameter.
    '

    Public ContextFlags As UInteger

    '
    ' This section is specified/returned if CONTEXT_DEBUG_REGISTERS is
    ' set in ContextFlags.  Note that CONTEXT_DEBUG_REGISTERS is NOT
    ' included in CONTEXT_FULL.
    '

    Public Dr0 As UInteger
    Public Dr1 As UInteger
    Public Dr2 As UInteger
    Public Dr3 As UInteger
    Public Dr6 As UInteger
    Public Dr7 As UInteger

    '
    ' This section is specified/returned if the
    ' ContextFlags word contians the flag CONTEXT_FLOATING_POINT.
    '

    Public FloatSave As FLOATING_SAVE_AREA

    '
    ' This section is specified/returned if the
    ' ContextFlags word contians the flag CONTEXT_SEGMENTS.
    '

    Public SegGs As UInteger
    Public SegFs As UInteger
    Public SegEs As UInteger
    Public SegDs As UInteger

    '
    ' This section is specified/returned if the
    ' ContextFlags word contians the flag CONTEXT_INTEGER.
    '

    Public Edi As UInteger
    Public Esi As UInteger
    Public Ebx As UInteger
    Public Edx As UInteger
    Public Ecx As UInteger
    Public Eax As UInteger

    '
    ' This section is specified/returned if the
    ' ContextFlags word contians the flag CONTEXT_CONTROL.
    '

    Public Ebp As UInteger
    Public Eip As UInteger
    Public SegCs As UInteger
    ' MUST BE SANITIZED
    Public EFlags As UInteger
    ' MUST BE SANITIZED
    Public Esp As UInteger
    Public SegSs As UInteger

    '
    ' This section is specified/returned if the ContextFlags word
    ' contains the flag CONTEXT_EXTENDED_REGISTERS.
    ' The format and contexts are processor specific
    '

    Public ExtendedRegisters As Byte()
    '512
    Public Shared Function getNewContext() As CONTEXT
        Dim n As New CONTEXT()
        n.FloatSave.RegisterArea = New Byte(79) {}
        n.ExtendedRegisters = New Byte(511) {}
        Return n
    End Function
End Structure

Public Class Kernel32
    Public Declare Auto Function Wow64DisableWow64FsRedirection Lib "kernel32.dll" (ByRef OldValue As Integer) As Boolean

    Public Declare Auto Sub GetSystemInfo Lib "kernel32.dll" (<MarshalAs(UnmanagedType.Struct)> ByRef lpSystemInfo As SYSTEM_INFO)

    Public Declare Auto Function VirtualQueryEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByRef lpBuffer As MEMORY_BASIC_INFORMATION, ByVal dwLength As UIntPtr) As UInteger

    Private Declare Auto Function VirtualProtectEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flNewProtect As UInteger, ByRef lpflOldProtect As UInteger) As Boolean

    Public Declare Auto Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As UInteger, ByVal flAllocationType As UInteger, ByVal flProtect As UInteger) As IntPtr

    Public Declare Auto Function VirtualFreeEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal size As IntPtr, ByVal freeType As UInteger) As Boolean

    Public Declare Auto Function ReadProcessMemory Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer As Byte(), ByVal nSize As Integer, ByRef lpNumberOfBytesRead As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean

    Public Declare Auto Function WriteProcessMemory Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer As Byte(), ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean


    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function LoadLibrary(ByVal lpFileName As String) As IntPtr
    End Function

    Public Declare Auto Function FreeLibrary Lib "kernel32.dll" (ByVal hModule As IntPtr) As Boolean

    Public Declare Ansi Function GetProcAddress Lib "kernel32.dll" (ByVal hModule As IntPtr, ByVal procName As String) As IntPtr

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function OpenProcess(ByVal access As Integer, ByVal inherit As Boolean, ByVal processId As Integer) As IntPtr
    End Function

    Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    Public Declare Auto Function CreateRemoteThread Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpThreadAttributes As IntPtr, ByVal dwStackSize As IntPtr, ByVal lpStartAddress As IntPtr, ByVal lpParameter As IntPtr, ByVal dwCreationFlags As UInteger, _
     ByRef dwThreadId As UInteger) As IntPtr

    Public Declare Auto Function GetExitCodeThread Lib "kernel32.dll" (ByVal hThread As IntPtr, ByRef lpExitCode As UInteger) As Boolean

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function GetThreadContext(ByVal hThread As IntPtr, ByRef lpContext As Byte()) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function


    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function OpenThread(ByVal dwDesiredAccess As ThreadAccess, <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, ByVal dwThreadId As UInteger) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function SetThreadContext(ByVal hThread As IntPtr, ByVal lpContext As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function SuspendThread(ByVal hThread As IntPtr) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function ResumeThread(ByVal hThread As IntPtr) As UInteger
    End Function

    <DllImport("kernel32.dll")> _
    Public Shared Function TerminateThread(ByVal hThread As IntPtr, ByVal dwExitCode As UInteger) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function CreateProcess(ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByRef lpProcessAttributes As SECURITY_ATTRIBUTES, ByRef lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal bInheritHandles As Boolean, ByVal dwCreationFlags As UInteger, _
   ByVal lpEnvironment As IntPtr, ByVal lpCurrentDirectory As String, <[In]()> ByRef lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CallingConvention:=CallingConvention.Winapi)> _
    Public Shared Function IsWow64Process(<[In]()> ByVal hProcess As IntPtr, <Out()> ByRef Wow64Process As Boolean) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function CreateFileMapping(ByVal hFile As IntPtr, ByVal lpFileMappingAttributes As IntPtr, ByVal flProtect As UInteger, ByVal dwMaximumSizeHigh As UInteger, ByVal dwMaximumSizeLow As UInteger, ByVal lpName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function MapViewOfFile(ByVal hFileMappingObject As IntPtr, ByVal dwDesiredAccess As UInteger, ByVal dwFileOffsetHigh As UInteger, ByVal dwFileOffsetLow As UInteger, ByVal dwNumberOfBytesToMap As IntPtr) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Shared Function UnmapViewOfFile(ByVal lpBaseAddress As IntPtr) As Boolean
    End Function
End Class

#Region "NtDll"
<StructLayout(LayoutKind.Sequential)> _
Public Class NtProcessBasicInfo
    Public ExitStatus As Integer
    Public PebBaseAddress As IntPtr = IntPtr.Zero
    Public AffinityMask As IntPtr = IntPtr.Zero
    Public BasePriority As Integer
    Public UniqueProcessId As IntPtr = IntPtr.Zero
    Public InheritedFromUniqueProcessId As IntPtr = IntPtr.Zero
End Class

<StructLayout(LayoutKind.Sequential)> _
Public Class CLIENT_ID
    Private UniqueProcess As IntPtr = IntPtr.Zero
    Private UniqueThread As IntPtr = IntPtr.Zero
End Class

<StructLayout(LayoutKind.Sequential)> _
Public Class NtThreadBasicInformation
    Private ExitStatus As UInteger
    Private TebBaseAddress As IntPtr = IntPtr.Zero
    Private ClientId As CLIENT_ID
    Private AffinityMask As IntPtr
    Private Priority As IntPtr
    Private BasePriority As IntPtr
End Class

'[StructLayout(LayoutKind.Sequential)]
'public class PEB 
'{
'    byte InheritedAddressSpace; 
'    byte ReadImageFileExecOptions; 
'    byte BeingDebugged; 
'    byte Spare; 
'    HANDLE Mutant; 
'    PVOID ImageBaseAddress; 
'    PPEB_LDR_DATA LoaderData; 
'    PRTL_USER_PROCESS_PARAMETERS ProcessParameters; 
'    PVOID SubSystemData; 
'    PVOID ProcessHeap; 
'    PVOID FastPebLock; 
'    PPEBLOCKROUTINE FastPebLockRoutine; 
'    PPEBLOCKROUTINE FastPebUnlockRoutine; 
'    ULONG EnvironmentUpdateCount; 
'    PPVOID KernelCallbackTable; 
'    PVOID EventLogSection; 
'    PVOID EventLog; 
'    PPEB_FREE_BLOCK FreeList; 
'    ULONG TlsExpansionCounter; 
'    PVOID TlsBitmap; 
'    ULONG TlsBitmapBits[0x2]; 
'    PVOID ReadOnlySharedMemoryBase; 
'    PVOID ReadOnlySharedMemoryHeap; 
'    PPVOID ReadOnlyStaticServerData; 
'    PVOID AnsiCodePageData; 
'    PVOID OemCodePageData; 
'    PVOID UnicodeCaseTableData; 
'    ULONG NumberOfProcessors; 
'    ULONG NtGlobalFlag; 
'    BYTE Spare2[0x4]; 
'    LARGE_INTEGER CriticalSectionTimeout; 
'    ULONG HeapSegmentReserve; 
'    ULONG HeapSegmentCommit; 
'    ULONG HeapDeCommitTotalFreeThreshold; 
'    ULONG HeapDeCommitFreeBlockThreshold; 
'    ULONG NumberOfHeaps; 
'    ULONG MaximumNumberOfHeaps; 
'    PPVOID *ProcessHeaps; 
'    PVOID GdiSharedHandleTable; 
'    PVOID ProcessStarterHelper; 
'    PVOID GdiDCAttributeList; 
'    PVOID LoaderLock; 
'    ULONG OSMajorVersion; 
'    ULONG OSMinorVersion; 
'    ULONG OSBuildNumber; 
'    ULONG OSPlatformId; 
'    ULONG ImageSubSystem; 
'    ULONG ImageSubSystemMajorVersion; 
'    ULONG ImageSubSystemMinorVersion; 
'    ULONG GdiHandleBuffer[0x22]; 
'    ULONG PostProcessInitRoutine; 
'    ULONG TlsExpansionBitmap; 
'    BYTE TlsExpansionBitmapBits[0x80]; 
'    ULONG SessionId;
'}


Public Class NtDll
    <DllImport("ntdll.dll", SetLastError:=False, CharSet:=CharSet.Auto)> _
    Public Shared Function NtQueryInformationProcess(ByVal processHandle As IntPtr, ByVal query As Integer, ByVal info As NtProcessBasicInfo, ByVal size As Integer, ByVal returnedSize As Integer()) As Integer
    End Function

    <DllImport("ntdll.dll", SetLastError:=False, CharSet:=CharSet.Auto)> _
    Public Shared Function NtQueryInformationThread(ByVal threadHandle As IntPtr, ByVal ThreadInformationClass As Integer, ByVal info As NtThreadBasicInformation, ByVal size As Integer, ByVal returnedSize As IntPtr) As Integer
    End Function

    <DllImport("ntdll.dll", SetLastError:=False, CharSet:=CharSet.Auto)> _
    Public Shared Function NtQuerySystemInformation(ByVal query As Integer, ByVal dataPtr As IntPtr, ByVal size As Integer, ByRef returnedSize As Integer) As Integer
    End Function

    <DllImport("ntdll.dll", SetLastError:=False, CharSet:=CharSet.Auto)> _
    Public Shared Function NtSetInformationThread(ByVal threadHandle As IntPtr, ByVal ThreadInformationClass As Integer, ByVal ThreadInformation As IntPtr, ByVal size As Integer, ByVal returnedSize As IntPtr) As Integer
    End Function
End Class
#End Region