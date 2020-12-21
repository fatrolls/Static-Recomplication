Imports System.IO

Public Class Subroutine

    Public child_sub As Subroutine
    Public name As String
    Public address_start As UInteger
    Public address_end As UInteger
    Public instructions As Dictionary(Of UInteger, Instruction)
    Public instruction_list As List(Of Instruction)
    Public labels As List(Of UInteger)
    Public jumps As Dictionary(Of UInteger, UInteger)
    Public enders As List(Of UInteger)
    Public label_eflags As Dictionary(Of String, List(Of Instruction)) 'TODO FIX
    Public eflags_cache As Dictionary(Of String, List(Of Instruction)) 'TODO FIX

    Sub New(ByVal address As UInteger)
        Me.child_sub = Nothing
        Me.name = ""
        Me.address_start = address
        Me.instructions = New Dictionary(Of UInteger, Instruction)
        Me.instruction_list = New List(Of Instruction)
        Me.labels = New List(Of UInteger)
        Me.jumps = New Dictionary(Of UInteger, UInteger)
        Me.enders = New List(Of UInteger)

        Me.label_eflags = New Dictionary(Of String, List(Of Instruction))
        Me.eflags_cache = New Dictionary(Of String, List(Of Instruction))

        Dim i As UInteger = 0

        While True
            Dim data As Stream
            Try
                data = Translator.access_memory(address, 16)
            Catch ex As Exception
                Translator.PrintLog(String.Format("Could not get memory for address 0x{0}", Hex(address)), Color.Red)
                Throw New Exception(String.Format("Could not get memory for address 0x{0}", Hex(address)))
            End Try

            Dim raw_instruction As LibDasmInstruction = LibDasm.get_instruction(New BinaryReader(data), Mode.MODE_32)
            If raw_instruction Is Nothing Then
                MessageBox.Show(String.Format("Invalid instruction at 0x{0}", Hex(address)))
                Exit While
            End If

            Dim instruction As Instruction = New Instruction(raw_instruction, address)
            If label_eflags.ContainsKey(address) Then
                For Each pair As KeyValuePair(Of String, List(Of Instruction)) In label_eflags
                    If eflags_cache(pair.Key) Is Nothing Then 'empty list
                        eflags_cache(pair.Key) = New List(Of Instruction)
                    End If
                    If Not pair.Value Is Nothing Then
                        eflags_cache(pair.Key).AddRange(pair.Value)
                    End If
                Next
            End If

            For Each flag As String In instruction.eflags_used 'eflags_used is 100% string
                For Each inst_dep As Instruction In eflags_cache(flag)
                    Dim dependency As Dictionary(Of String, List(Of Instruction)) = inst_dep.eflags_dependency
                    If dependency(flag) Is Nothing Then
                        dependency(flag) = New List(Of Instruction)
                    End If
                    dependency(flag).Add(instruction)
                Next
            Next

            For Each flag As String In instruction.eflags_affected 'eflags_used is 100% string
                For Each inst_dep As Instruction In eflags_cache(flag)
                    If eflags_cache(flag) Is Nothing Then
                        eflags_cache(flag) = New List(Of Instruction)
                    End If
                    eflags_cache(flag).Add(instruction)
                Next
            Next

            Dim opcode As Byte = instruction.opcode
            Me.instructions(address) = instruction
            Me.instruction_list.Add(instruction)
            address += instruction.length

            Dim mnemonic As String = instruction.mnemonic
            'Dim keys As Dictionary(Of 'TODO FIX LATER

            If New String() {"jmp", "call"}.Contains(mnemonic) Then
                eflags_cache.Clear()
            End If

            If is_end(instruction, i) Then
                Me.enders.Add(instruction.address)
                If Not labels Is Nothing Then
                    If labels.Max() < address Then 'highest element in the list
                        'do we still have labels to process?
                        Exit While
                    End If
                Else
                    Exit While
                End If
            ElseIf Not opcode = 0 AndAlso _
                label_makers.ContainsKey(mnemonic) OrElse _
                label_makers.ContainsValue(opcode) Then

                add_jump(instruction.address, instruction.op1.value)

                For Each pair As KeyValuePair(Of String, List(Of Instruction)) In eflags_cache
                    If label_eflags(pair.Key) Is Nothing Then
                        label_eflags(pair.Key) = New List(Of Instruction)
                    End If
                    label_eflags(pair.Key).AddRange(pair.Value)
                Next
            End If

            i += 1
        End While

        Me.address_end = address

        ' do a second pass, checking for instructions that are never used
        'i = 0
        'Dim check_label As Boolean = False
        'Dim new_list As New List(Of Instruction)
        'For Each instruction As Instruction In instruction_list
        '    If check_label AndAlso _
        '        Not labels.Contains(instruction.address) Then
        '        instructions.Remove(instruction.address)
        '        Continue For
        '    End If
        '    check_label = False
        '    If is_end(instruction, i) Then
        '        check_label = True
        '    End If
        '    new_list.Add(instruction)
        '    i += 1
        'Next
        'instruction_list = new_list
    End Sub

    Public Function can_split_direct() As Boolean
        Return enders.Count = 1
    End Function

    Public Function can_split(ByVal address As UInteger) As Boolean
        'Can only be called after initial decode

        Dim minimum As UInteger
        Dim maximum As UInteger

        For Each pair As KeyValuePair(Of UInteger, UInteger) In jumps
            minimum = Math.Min(pair.Key, pair.Value) 'Key = src, Value = dst
            maximum = Math.Max(pair.Key, pair.Value) 'Key = src, Value = dst

            If address >= minimum AndAlso _
                address < maximum Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Sub set_instruction_list(ByVal items As List(Of Instruction), _
                                    ByVal jumps As Dictionary(Of UInteger, UInteger), _
                                    ByVal enders As List(Of UInteger))

        Me.instructions = New Dictionary(Of UInteger, Instruction)
        Me.instruction_list = New List(Of Instruction)(items)

        For Each instruction As Instruction In items
            Me.instructions(instruction.address) = instruction
        Next

        Me.address_start = items(0).address
        Me.address_end = items.Last().address + items.Last().length

        Me.jumps = New Dictionary(Of UInteger, UInteger)

        For Each pair As KeyValuePair(Of UInteger, UInteger) In jumps
            If Not Me.instructions.ContainsKey(pair.Key) Then 'pair.Key = src
                Continue For
            End If

            Me.jumps(pair.Key) = pair.Value 'pair.value = dst
        Next

        Me.labels = jumps.Values().ToList()
        Me.enders = New List(Of UInteger)

        For Each ender As UInteger In enders
            If Not Me.instructions.ContainsKey(ender) Then
                Continue For
            End If
            Me.enders.Add(ender)
        Next
    End Sub

    Public Function split(ByVal address As UInteger) As Subroutine
        Dim other_sub As Subroutine = New Subroutine(0)
        Dim instruction As Instruction = Me.instructions(address)
        Dim index As Integer = Me.instruction_list.IndexOf(instruction)
        Dim items As New List(Of Instruction)
        items.CopyTo(Me.instruction_list.ToArray(), index)
        Dim other_items As New List(Of Instruction)
        other_items.CopyTo(index, Me.instruction_list.ToArray(), 0, Me.instruction_list.Count - index)
        Dim jumps As New Dictionary(Of UInteger, UInteger)(Me.jumps)
        Dim enders As New List(Of UInteger)(Me.enders)
        set_instruction_list(items, jumps, enders)
        other_sub.set_instruction_list(other_items, jumps, enders)
        Me.child_sub = other_sub
        Return other_sub
    End Function

    Public Sub add_jump(ByVal src As UInteger, ByVal dst As UInteger)
        Me.jumps(src) = dst
        Me.labels.Add(dst)
    End Sub

    Public Function is_end(ByVal inst As Instruction, ByVal index As Integer) As Boolean
        Dim mnemonic As String = inst.mnemonic
        Dim opcode As Byte = inst.opcode
        If New String("retn", "ret", "int3").Contains(mnemonic) Then
            Return True
        End If
        If mnemonic = "jmp" AndAlso opcode = &HFF Then
            'far jumps
            Return True
        End If
        If mnemonic = "jmp" And index = 0 Then
            'wrapper functions
            Return True
        End If
        If inst.is_end() Then
            'special 'call's, checks Config.vb for function enders
            Return True
        End If
        Return False
    End Function
End Class
