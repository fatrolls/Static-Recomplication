Public Class Instruction
    Public address As UInteger
    Public has_reloc As Boolean
    Public condition_value As String
    Public length As UInteger
    Public type As InstructionEnum
    Public mode As Mode
    Public opcode As Byte
    Public modrm As Byte
    Public sib As Byte
    Public extindex As UInteger
    Public fpuindex As Integer
    Public dispbytes As UInteger
    Public immbytes As UInteger
    Public sectionbytes As Integer
    Public flags As UInteger
    Public op1 As Operand
    Public op2 As Operand
    Public op3 As Operand
    Public mnemonic As String
    Public eflags_affected As String()
    Public eflags_used As String()
    Public eflags_dependency As Dictionary(Of String, List(Of Instruction))
    Public disasm As String
    Public prefix As String



    Sub New(ByVal i As LibDasmInstruction, ByVal offset As UInteger)
        Me.address = offset

        Me.has_reloc = False
        Me.condition_value = ""

        For pos As UInteger = offset To offset + i.length
            If Translator.reloc_addresses.Contains(pos) Then
                Me.has_reloc = True
                Exit For
            End If
        Next

        Me.length = i.length
        Me.type = i.type
        Me.mode = i.mode
        Me.opcode = i.opcode
        Me.modrm = i.modrm
        Me.sib = i.sib
        Me.extindex = i.extindex
        Me.fpuindex = i.fpuindex
        Me.dispbytes = i.dispbytes
        Me.immbytes = i.immbytes
        Me.sectionbytes = i.sectionbytes
        Me.flags = i.flags

        If i.op1.type <> OperandEnum.OPERAND_TYPE_NONE Then
            Me.op1 = New Operand(Me, i.op1, offset)
        Else
            Me.op1 = Nothing
        End If

        If i.op2.type <> OperandEnum.OPERAND_TYPE_NONE Then
            Me.op2 = New Operand(Me, i.op2, offset)
        Else
            Me.op2 = Nothing
        End If

        If i.op3.type <> OperandEnum.OPERAND_TYPE_NONE Then
            Me.op3 = New Operand(Me, i.op3, offset)
        Else
            Me.op3 = Nothing
        End If

        mnemonic = LibDasm.get_mnemonic_string(i, Format.FORMAT_INTEL)
        mnemonic = mnemonic.Trim()
        If mnemonic = "rep retn" Then
            mnemonic = "retn"
        ElseIf mnemonic = "rep ret" Then
            mnemonic = "ret"
        End If
        Dim mnemonicSplit As String() = mnemonic.Split(Nothing)
        If New String() {"rep", "repe", "repne", "lock"}.Contains(mnemonicSplit(0)) Then
            Me.prefix = mnemonic(0)
            mnemonicSplit = mnemonicSplit.Skip(1).ToArray()
        Else
            Me.prefix = ""
        End If

        If mnemonicSplit.Length > 2 Then
            If mnemonicSplit(0) <> "bts" AndAlso mnemonicSplit(1) <> "dword" Then
                MsgBox(Join(mnemonicSplit))
                Throw New Exception("Not Implemented Error")
            End If
        End If

        Me.mnemonic = mnemonicSplit(0)

        Dim affected As UShort = i.eflags_affected
        If Translator.eflags_affected.ContainsKey(mnemonic) Then
            affected = Translator.eflags_affected(mnemonic)
        End If
        Me.eflags_affected = get_eflags(affected)

        Dim used As UShort = i.eflags_used
        If Translator.eflags_used.ContainsKey(mnemonic) Then
            used = Translator.eflags_used(mnemonic)
        End If
        Me.eflags_used = get_eflags(used)

        Me.eflags_dependency = New Dictionary(Of String, List(Of Instruction))
        Me.disasm = LibDasm.get_instruction_string(i, Format.FORMAT_INTEL, 0).TrimEnd(New Char() {" "})
    End Sub

    Public Function is_end() As Boolean
        If Me.mnemonic <> "call" Then
            Return False
        End If
        If Not Me.op1.is_immediate Then
            Return False
        End If
        Dim name As String = Translator.get_function_name(Me.op1.value)
        Return function_enders.contains(name)
    End Function

    Public Function get_disasm() As String
        Return Me.disasm
    End Function

    Public Function group2() As UInteger
        Return (Me.flags & &HFF0000) >> 16
    End Function

    Public Function operand_so() As Boolean
        Return (Me.flags And &H100)
    End Function

    Public Function get_operand_mode() As Integer
        If operand_so() Then
            Return MODE_16
        Else
            Return MODE_32
        End If
    End Function

    Public Function address_so() As Boolean
        Return (Me.flags And &H1000)
    End Function

    Public Function get_address_mode() As Integer
        If address_so() Then
            Return MODE_16
        Else
            Return MODE_32
        End If
    End Function

    Public Function address_size() As Integer
        If address_so() Then
            Return 2
        Else
            Return 4
        End If
    End Function

    Public Function get_condition() As String
        If New String() {"sbb", "adc", "rcr"}.Contains(mnemonic) Then
            Return "c"
        ElseIf mnemonic.StartsWith("j") Then
            Return mnemonic.Substring(1)
        ElseIf mnemonic.StartsWith("cmov") Then
            Return mnemonic.Substring(4)
        ElseIf mnemonic.StartsWith("set") Then
            Return mnemonic.Substring(3)
        End If
        Return ""
    End Function

    Public Function get_dependees() As Instruction()
        Dim dependees As New List(Of Instruction)

        For Each value As List(Of Instruction) In Me.eflags_dependency.Values()
            dependees.AddRange(value)
        Next

        Return dependees.ToArray()
    End Function

    Public Function get_condition_value() As String
        If condition_value = "" Then
            Throw New Exception("Not Implemented Error")
        End If
        Return condition_value
    End Function
End Class
