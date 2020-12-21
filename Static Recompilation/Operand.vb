Public Class Operand
    Public is_memory As Boolean
    Public is_register As Boolean
    Public is_immediate As Boolean
    Public is_reloc As Boolean
    Public has_import_alias As Boolean
    Public index_reg As String
    Public scale As String
    Public base_reg As String
    Public displacement As String
    Public reg_type As UInteger
    Public type As OperandEnum
    Public mode As UInteger
    Public mask_ot As UInteger
    Public size As UInteger
    Public value As String
    Public reloc_value As String
    Public reloc_displacement As String 'TODO FIX

    Sub New(ByVal inst As Instruction, ByVal op As LibDasmOperand, ByVal offset As UInteger)
        Me.is_memory = False
        Me.is_register = False
        Me.is_immediate = False
        Me.is_reloc = False
        Me.has_import_alias = False
        Me.index_reg = ""
        Me.scale = ""
        Me.base_reg = ""
        Me.displacement = ""
        Me.reg_type = 0

        Me.type = op.type

        Me.mode = inst.get_operand_mode()
        Me.mask_ot = get_mask_ot(op.flags)
        Me.size = Translator.operand_sizes(mask_ot)(mode)

        If Me.type = OperandEnum.OPERAND_TYPE_REGISTER Then
            Me.is_register = True
            Me.reg_type = LibDasm.get_register_type(op)

            If reg_type = OpCodes.REG_GEN Then
                If mask_ot = Translator.OT_b Then
                    Me.reg_type = OpCodes.REG_GEN_BYTE
                ElseIf mask_ot = Translator.OT_v Then
                    If mode = Translator.MODE_32 Then
                        Me.reg_type = OpCodes.REG_GEN_DWORD
                    Else
                        Me.reg_type = OpCodes.REG_GEN_WORD
                    End If
                ElseIf mask_ot = Translator.OT_w Then
                    Me.reg_type = OpCodes.REG_GEN_WORD
                ElseIf mask_ot = Translator.OT_d Then
                    Me.reg_type = OpCodes.REG_GEN_DWORD
                ElseIf mask_ot = Translator.OT_dq Then
                    Me.reg_type = OpCodes.REG_GEN_DWORD
                Else
                    Throw New Exception("Not Implemented Error")
                End If
            End If

            Me.value = get_register(op.reg, reg_type)
        ElseIf type = OperandEnum.OPERAND_TYPE_MEMORY Then
            Me.is_memory = True
            Dim sum_list As New List(Of String)
            Me.mode = inst.get_address_mode()
            Dim group2 As UInteger = inst.group2()
            If group2 Then
                sum_list.Add(get_register(group2 - 1, OpCodes.REG_SEGMENT))
            End If

            'Base register
            If op.basereg <> REG_NOP Then
                If mode = Translator.MODE_32 Then
                    Me.reg_type = OpCodes.REG_GEN_DWORD
                Else
                    Me.reg_type = OpCodes.REG_GEN_WORD
                End If
                Me.base_reg = get_register(op.basereg, reg_type)
                sum_list.Add(base_reg)
            End If

            'Index register
            If op.indexreg <> REG_NOP Then
                If mode = Translator.MODE_32 Then
                    Me.reg_type = OpCodes.REG_GEN_DWORD
                Else
                    Me.reg_type = OpCodes.REG_GEN_WORD
                End If
                Me.index_reg = get_register(op.indexreg, reg_type)
                Dim index As String = index_reg
                If New UInteger() {2, 4, 8}.Contains(op.scale) Then
                    Me.scale = op.scale
                    index += String.Format("*{0}", scale)
                End If
                sum_list.Add(index)
            End If

            'INTEL displacement
            If inst.dispbytes Then
                Dim addr As UInteger = op.displacement
                If inst.has_reloc AndAlso Translator.reloc_values.Contains(addr) Then
                    Me.reloc_displacement = Translator.get_reloc(addr)
                    Me.is_reloc = True
                End If
                If op.displacement And (1 << (op.dispbytes * 8 - 1)) Then
                    Dim tmp As UInteger = op.displacement
                    If op.dispbytes = 1 Then
                        tmp = Not tmp And &HFF
                    ElseIf op.dispbytes = 2 Then
                        tmp = Not tmp And &HFFFF
                    ElseIf op.dispbytes = 4 Then
                        tmp = Not tmp And &HFFFFFFFF
                    Else
                        Throw New Exception("Not Implemented Error")
                    End If
                    Me.displacement = String.Format("(-0x{0})", Hex((tmp + 1)))
                Else
                    'Positive displacement
                    Me.displacement = String.Format("0x{0}", Hex(op.displacement))
                End If
                sum_list.Add(displacement)
            End If

            If sum_list.Count = 1 And inst.dispbytes Then
                Dim addr As UInteger = op.displacement
                If Translator.[imports].ContainsKey(addr) _
                AndAlso Not Translator.import_addresses.Values().Contains(addr) Then
                    Me.displacement = Str(addr)
                    Me.is_memory = False
                    Me.is_immediate = True
                    Me.is_reloc = False
                    Me.value = get_import_alias(addr, inst)
                    Return
                End If
            End If

            Me.value = Join(sum_list.ToArray(), "+")
            If Me.is_reloc Then
                sum_list.RemoveAt(sum_list.Count - 1) ' [:-1]
                Me.reloc_value = Join(sum_list.ToArray(), "+") _
                                 + Me.reloc_displacement
            End If
        ElseIf type = OperandEnum.OPERAND_TYPE_IMMEDIATE Then
            Me.is_immediate = True
            Dim mask As UInteger = get_mask_am(op.flags)
            Dim maskvalue As UInteger
            If mask = AM_J Then
                maskvalue = uint32_to_int32(op.immediate) + inst.length + offset
            ElseIf New UInteger() {AM_I1, AM_I}.Contains(mask) Then
                maskvalue = op.immediate And &HFFFFFFFF
            ElseIf mask = AM_A Then '32-bit or 48-bit address
                Throw New Exception("Not Implemented Error")
                Dim stringg As String
                stringg += String.Format("0x{0}:0x{1}", op.section, op.displacement) 'TODO....
            End If

            If inst.has_reloc AndAlso _
                Translator.reloc_values.Contains(maskvalue) Then
                Me.reloc_value = Translator.get_reloc(maskvalue)
                Me.is_reloc = True
            End If

            Me.value = Str(maskvalue)
        End If
    End Sub

    Public Function get_import_alias(ByVal addr As UInteger, ByVal inst As Instruction) As UInteger
        Dim name As String = Translator.[imports](addr).Key.ImportName
        Dim dll_addr As String
        Try
            dll_addr = Translator.import_addresses(name)
        Catch ex As Exception
            Return addr
        End Try
        Dim section As Section 'TODO it's not Section class :P fix later
        Try
            section = Translator.get_section(dll_addr)
        Catch ex As Exception
            Return addr
        End Try
        If section.section_name = "text" Then
            'function alias
            Return addr
        End If
        'data alias
        Return dll_addr
    End Function

    Public Overloads Function ToString(Optional ByVal as_reloc As Boolean = True) As String
        as_reloc = Me.is_reloc AndAlso as_reloc
        Dim value As String
        If as_reloc Then
            value = Me.reloc_value
        Else
            value = Me.value
        End If
        If Me.is_memory Then
            Return get_memory(value, Me.size)
        End If
        If Me.is_immediate AndAlso Not as_reloc Then
            Return String.Format("{0}U", value)
        End If
        Return value
    End Function
End Class
