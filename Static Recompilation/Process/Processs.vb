
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics
Imports System.Runtime.InteropServices


Public Class Processs
    Private m_Process As Process
    Private m_Memory As MemorySnapshot
    'PEImage m_MainImage;
    'Dictionary<IntPtr, PEImage> m_PEFiles = new Dictionary<IntPtr, PEImage>();
    Private m_info As New NtProcessBasicInfo()

    Public ReadOnly Property BasicInfo() As NtProcessBasicInfo
        Get
            Return m_info
        End Get
    End Property

    Public Sub New(ByVal process As Process)
        m_Process = process
        RetrieveProcessBasicInfo()
    End Sub

    Public ReadOnly Property Process() As Process
        Get
            Return m_Process
        End Get
    End Property

    'public ProcessModuleCollection Modules
    '{
    '    get
    '    {
    '        ProcessModuleCollection mods = m_Process.Modules;
    '        mods = new ProcessModuleCollection(
    '    }
    '}

    Public ReadOnly Property Memory() As MemorySnapshot
        Get
            If m_Memory Is Nothing Then
                m_Memory = New MemorySnapshot(m_Process)
            End If
            Return m_Memory
        End Get
    End Property
    'public Dictionary<IntPtr, PEImage> PEFiles
    '{
    '    get
    '    {
    '        if (m_PEFiles == null || m_PEFiles.Count == 0)
    '            PopulatePeFiles();
    '        return m_PEFiles;
    '    }
    '}
    'public PEImage MainImage
    '{
    '    get
    '    {
    '        if (m_MainImage == null)
    '        {
    '            try
    '            {
    '                m_MainImage = new PEImage(m_Process.MainModule.FileName);
    '            }
    '            catch (Exception err)
    '            {
    '                Trace.WriteLine(err);
    '            }
    '        }
    '        return m_MainImage;
    '    }
    '}
    'public MagicType Type
    '{
    '    get
    '    {
    '        return m_MainImage.PeHeader.OptionalHeader.Magic;
    '    }
    '}

    Public Sub Refresh()
        m_Process.Refresh()
        m_Memory = Nothing
        'm_PEFiles = new Dictionary<IntPtr,PEImage>();
    End Sub

    'private void PopulatePeFiles()
    '{
    '    m_PEFiles.Clear();
    '    try
    '    {
    '        foreach (ProcessModule module in m_Process.Modules)
    '        {
    '            try
    '            {
    '                PEImage image = new PEImage(module.FileName);
    '                if (module.BaseAddress == m_Process.MainModule.BaseAddress)
    '                {
    '                    m_MainImage = image;
    '                }
    '                m_PEFiles.Add(module.BaseAddress, image);
    '            }
    '            catch (Exception err)
    '            {
    '                Trace.WriteLine(err);
    '            }
    '        }
    '    }
    '    catch (Exception err)
    '    {
    '        Trace.WriteLine(err);
    '    }
    '}

    Private Sub RetrieveProcessBasicInfo()
        Dim info As New NtProcessBasicInfo()
        Try
            If NtDll.NtQueryInformationProcess(Process.Handle, 0, info, Marshal.SizeOf(info), Nothing) = 0 Then
                m_info = info
            End If
        Catch err As Exception
            Trace.WriteLine(err)
        End Try
    End Sub
End Class
