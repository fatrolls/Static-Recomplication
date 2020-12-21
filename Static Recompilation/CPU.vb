Imports System.Reflection

Public Class CPU
    Dim eip As UInteger
    Dim [sub] As Subroutine
    Dim eflags_id As UInteger
    Dim result_id As UInteger
    Public mnemonic_stats As Dictionary(Of String, UInteger)
    Dim index As UInteger
    Dim remaining_labels As List(Of UInteger)
    Dim unimplemented As Boolean
    Dim writer As CodeWriter

    Private Delegate Function Handler(ByVal instruction As Instruction)
    Dim instruction_functions As New Dictionary(Of String, Handler) From { _
        {"on_push", New Handler(AddressOf on_push)},
        {"on_pop", New Handler(AddressOf on_pop)},
        {"on_mov", New Handler(AddressOf on_mov)},
        {"on_movsx", New Handler(AddressOf on_movsx)},
        {"on_movd", New Handler(AddressOf on_movd)},
        {"on_mulsd", New Handler(AddressOf on_mulsd)},
        {"on_mulss", New Handler(AddressOf on_mulss)},
        {"on_addss", New Handler(AddressOf on_addss)},
        {"on_subss", New Handler(AddressOf on_subss)},
        {"on_divss", New Handler(AddressOf on_divss)},
        {"on_addsd", New Handler(AddressOf on_addsd)},
        {"on_divsd", New Handler(AddressOf on_divsd)},
        {"on_subsd", New Handler(AddressOf on_subsd)},
        {"on_andpd", New Handler(AddressOf on_andpd)},
        {"on_cvttsd2si", New Handler(AddressOf on_cvttsd2si)},
        {"on_cvtsd2sd", New Handler(AddressOf on_cvtsd2sd)},
        {"on_cvtsd2ss", New Handler(AddressOf on_cvtsd2ss)},
        {"on_cvttss2si", New Handler(AddressOf on_cvttss2si)},
        {"on_cvtdq2pd", New Handler(AddressOf on_cvtdq2pd)},
        {"on_cvtps2pd", New Handler(AddressOf on_cvtps2pd)},
        {"on_cvtpd2ps", New Handler(AddressOf on_cvtpd2ps)},
        {"on_cvtdq2ps", New Handler(AddressOf on_cvtdq2ps)},
        {"on_fst", New Handler(AddressOf on_fst)},
        {"on_fistp", New Handler(AddressOf on_fistp)},
        {"on_fsubp", New Handler(AddressOf on_fsubp)},
        {"on_fld1", New Handler(AddressOf on_fld1)},
        {"on_fldz", New Handler(AddressOf on_fldz)},
        {"on_fld", New Handler(AddressOf on_fld)},
        {"on_fldl", New Handler(AddressOf on_fldl)},
        {"on_fild", New Handler(AddressOf on_fild)},
        {"on_flds", New Handler(AddressOf on_flds)},
        {"on_fstp", New Handler(AddressOf on_fstp)},
        {"on_fstpl", New Handler(AddressOf on_fstpl)},
        {"on_comiss", New Handler(AddressOf on_comiss)},
        {"on_comisd", New Handler(AddressOf on_comisd)},
        {"on_movapd", New Handler(AddressOf on_movapd)},
        {"on_movaps", New Handler(AddressOf on_movaps)},
        {"on_movlpd", New Handler(AddressOf on_movlpd)},
        {"on_movq", New Handler(AddressOf on_movq)},
        {"on_movss", New Handler(AddressOf on_movss)},
        {"on_movsd_sse", New Handler(AddressOf on_movsd_sse)},
        {"on_movsd", New Handler(AddressOf on_movsd)},
        {"on_stosd", New Handler(AddressOf on_stosd)},
        {"on_sbb", New Handler(AddressOf on_sbb)},
        {"on_sub", New Handler(AddressOf on_sub)},
        {"on_adc", New Handler(AddressOf on_adc)},
        {"on_dec", New Handler(AddressOf on_dec)},
        {"on_inc", New Handler(AddressOf on_inc)},
        {"on_and", New Handler(AddressOf on_and)},
        {"on_or", New Handler(AddressOf on_or)},
        {"on_xor", New Handler(AddressOf on_xor)},
        {"on_not", New Handler(AddressOf on_not)},
        {"on_xorps", New Handler(AddressOf on_xorps)},
        {"on_lea", New Handler(AddressOf on_lea)},
        {"on_call", New Handler(AddressOf on_call)},
        {"on_xchg", New Handler(AddressOf on_xchg)},
        {"on_cbw", New Handler(AddressOf on_cbw)},
        {"on_setcc", New Handler(AddressOf on_setcc)},
        {"on_setnz", New Handler(AddressOf on_setcc)},
        {"on_sets", New Handler(AddressOf on_setcc)},
        {"on_setl", New Handler(AddressOf on_setcc)},
        {"on_setnle", New Handler(AddressOf on_setcc)},
        {"on_setnbe", New Handler(AddressOf on_setcc)},
        {"ob_setbe", New Handler(AddressOf on_setcc)},
        {"on_seto", New Handler(AddressOf on_setcc)},
        {"on_setns", New Handler(AddressOf on_setcc)},
        {"on_setz", New Handler(AddressOf on_setcc)},
        {"on_movzx", New Handler(AddressOf on_movzx)},
        {"on_cmovcc", New Handler(AddressOf on_cmovcc)},
        {"on_cmovae", New Handler(AddressOf on_cmovcc)},
        {"on_cmova", New Handler(AddressOf on_cmovcc)},
        {"on_cmovbe", New Handler(AddressOf on_cmovcc)},
        {"on_cmovb", New Handler(AddressOf on_cmovcc)},
        {"on_cmovg", New Handler(AddressOf on_cmovcc)},
        {"on_cmovge", New Handler(AddressOf on_cmovcc)},
        {"on_cmovl", New Handler(AddressOf on_cmovcc)},
        {"on_cmovle", New Handler(AddressOf on_cmovcc)},
        {"on_cmove", New Handler(AddressOf on_cmovcc)},
        {"on_cmovne", New Handler(AddressOf on_cmovcc)},
        {"on_cmovs", New Handler(AddressOf on_cmovcc)},
        {"on_int3", New Handler(AddressOf on_int3)},
        {"on_nop", New Handler(AddressOf on_nop)},
        {"on_fstcw", New Handler(AddressOf on_fstcw)},
        {"on_jmp", New Handler(AddressOf on_jmp)},
        {"on_jcc", New Handler(AddressOf on_jcc)},
        {"on_jz", New Handler(AddressOf on_jcc)},
        {"on_jc", New Handler(AddressOf on_jcc)},
        {"on_js", New Handler(AddressOf on_jcc)},
        {"on_jns", New Handler(AddressOf on_jcc)},
        {"on_jnl", New Handler(AddressOf on_jcc)},
        {"on_jl", New Handler(AddressOf on_jcc)},
        {"on_jng", New Handler(AddressOf on_jcc)},
        {"on_jnc", New Handler(AddressOf on_jcc)},
        {"on_ja", New Handler(AddressOf on_jcc)},
        {"on_jg", New Handler(AddressOf on_jcc)},
        {"on_jna", New Handler(AddressOf on_jcc)},
        {"on_jnz", New Handler(AddressOf on_jcc)},
        {"on_shl", New Handler(AddressOf on_shl)},
        {"on_shld", New Handler(AddressOf on_shld)},
        {"on_shr", New Handler(AddressOf on_shr)},
        {"on_rcr", New Handler(AddressOf on_rcr)},
        {"on_rol", New Handler(AddressOf on_rol)},
        {"on_sar", New Handler(AddressOf on_sar)},
        {"on_add", New Handler(AddressOf on_add)},
        {"on_xadd", New Handler(AddressOf on_xadd)},
        {"on_neg", New Handler(AddressOf on_neg)},
        {"on_test", New Handler(AddressOf on_test)},
        {"on_cwd", New Handler(AddressOf on_cwd)},
        {"on_div", New Handler(AddressOf on_div)},
        {"on_idiv", New Handler(AddressOf on_idiv)},
        {"on_imul", New Handler(AddressOf on_imul)},
        {"on_mul", New Handler(AddressOf on_mul)},
        {"on_retn", New Handler(AddressOf on_retn)},
        {"on_ret", New Handler(AddressOf on_ret)},
        {"on_leave", New Handler(AddressOf on_leave)},
        {"on_cmp", New Handler(AddressOf on_cmp)}}

    Sub New()
        Me.eflags_id = 0 'just a plain counter starting at 0   eflags_id.next() = *use* eflags_id then do eflags_id +=1
        Me.result_id = 0 'just a plain counter starting at 0  result_id.next() = *use* result_id then do result_id +=1
        Me.mnemonic_stats = New Dictionary(Of String, UInteger)  'Counter stats for each mnemonic opcode
    End Sub

    Public Sub set_sub(ByVal [sub] As Subroutine, ByVal writer As CodeWriter)
        Me.index = 0
        Me.[sub] = [sub]
        Me.eip = [sub].address_start
        Me.remaining_labels = New List(Of UInteger)([sub].labels)
        Me.unimplemented = False
        Me.writer = writer
    End Sub

    Public Function find_instruction(ByVal mnemonic As String, ByVal steps As Integer) As Instruction
        Dim keys As New List(Of UInteger)(Me.sub.instructions.Keys)
        keys.Sort()

        Dim index As Integer = keys.IndexOf(Me.eip)
        Dim direction As Integer = Int(1 * Math.Sign(steps)) 'math.copysign(1, steps)
        Dim address As UInteger
        Dim instruction As Instruction

        While steps <> 0
            index += direction
            steps -= direction
            address = keys(index)
            instruction = Me.[sub].instructions(address)
            If instruction.mnemonic = mnemonic Then
                Return instruction
            End If
        End While
        Return Nothing
    End Function

    Public Sub on_fail(ByVal message As String, ByVal i As Instruction)
        Me.writer.comment = ""
        If Me.unimplemented = False Then
            Me.unimplemented = True
        End If
        Translator.PrintLog(String.Format("Conversion skipped 0x{0}", Hex(Me.sub.address_start)), Color.Purple)
        Translator.PrintLog(String.Format("{0}: [0x{1}][0x{2}] {3}", message, Me.eip, i.opcode, i.get_disasm()), Color.Purple)
    End Sub

    Public Function [next]() As Boolean
        Dim instruction As Instruction = Me.[sub].instruction_list(Me.index)
        Me.eip = instruction.address

        'Writes label to file, if eip is currently on the address.
        If Me.[sub].labels.Contains(Me.eip) Then
            Me.writer.put_label(get_label_name(Me.eip))
            Me.remaining_labels.Remove(Me.eip)
        End If

        Dim name As String = String.Format("on_{0}", instruction.mnemonic)
        Me.mnemonic_stats(instruction.mnemonic) += 1

        If Not instruction_functions.ContainsKey(name) Then
            Me.on_fail("Not implemented instruction function call", instruction)
            Return False
        End If

        Dim rep_reg As String = ""
        If instruction.prefix = "rep" Then
            If instruction.get_address_mode() <> MODE_32 Then
                Throw New Exception("Not Implemented Error")
            End If
            rep_reg = get_register(REG_ECX, OpCodes.REG_GEN_DWORD)
        ElseIf instruction.prefix <> "" Then
            Throw New Exception("Not Implemented Error")
        End If

        'Creates a loop
        If rep_reg <> "" Then
            Me.writer.putln(String.Format("while ({0} > 0) {", rep_reg))
            Me.writer.indent()
        End If

        'Comment with eip and instruction
        Me.writer.comment = String.Format("[{0}] {1}", Me.eip, instruction.get_disasm())

        'Calls the instruction translation function
        If instruction_functions(name)(instruction) = False Then
            Me.on_fail("Handler skipped", instruction)
            If rep_reg Then
                Me.writer.end_brace()
            End If
            Return False
        End If

        Me.writer.comment = ""

        'Loop decreaser
        If rep_reg Then
            Me.writer.putln(String.Format("{0} -= 1;", rep_reg))
            Me.writer.end_brace()
        End If

        Me.index += 1

        'Hit the end of instructions for this sub
        If Me.index = Me.[sub].instruction_list.Count Then
            Me.remaining_labels.Clear()
            Return False
        End If

        Return True
    End Function

    Public Sub set_memory(ByVal dest As String, ByVal src As String, ByVal size As UInteger)
        Dim func As String = String.Format("mem.write_{0}", size_names(size))
        Dim type As String = size_types(size)
        Me.writer.putlnc("{0}({1}, {2});", func, dest, src)
    End Sub

    Public Sub set_register(ByVal dest As String, ByVal src As String)
        Me.writer.putlnc("{0} = {1};", dest, src)
    End Sub

    Public Sub set_op(ByVal op As Operand, ByVal value As String)
        If op.is_memory Then
            If op.is_reloc Then
                Me.set_memory(op.reloc_value, value, op.size)
            Else
                Me.set_memory(op.value, value, op.size)
            End If
        Else
            Me.set_register(op.ToString(), value)
        End If
    End Sub

    Public Function set_op_eflags(ByVal i As Instruction, ByVal op As Operand, ByVal value As String, ByVal ParamArray args() As String) As String()
        If Not i.eflags_dependency Is Nothing Then
            If args IsNot Nothing Then
                value = String.Format(value, args)
            End If
            Me.set_op(op, value)
            Return Nothing
        End If
        Dim size_type As UInteger = size_types(op.size)
        Dim names As New List(Of String)
        Dim name As String
        If args IsNot Nothing Then
            For Each arg As String In args
                name = String.Format("temp_{0}", Me.result_id)
                Me.result_id += 1
                Me.writer.putlnc("{0} {1};", size_type, name)
                Me.writer.putlnc("{0} = {1};", name, arg)
                names.Add(name)
            Next
        End If
        value = String.Format(value, names) 'TODO: maybe names.ToArray()
        name = String.Format("temp_{0}", Me.result_id)
        Me.result_id += 1
        Me.writer.putlnc("{0} {1};", size_type, name)
        Me.writer.putlnc("{0} = {1};", name, value)
        Me.set_op(op, name)
        If args IsNot Nothing Then
            names.Insert(0, name)
            Return names.ToArray()
        End If
        Return New String() {name}
    End Function

    Public Sub set_eflags(ByVal i As Instruction, Optional ByVal a As String = "", Optional ByVal b As String = "", Optional ByVal res As UInteger = 0, Optional ByVal size As UInteger = 0)
        Dim name As String = i.mnemonic
        Dim dependees As New List(Of Instruction)(i.get_dependees())
        Dim signed_type As String = signed_types(size)
        Dim signed_res As String = String.Format("({0}){1}", signed_type, res)
        Dim create_bool As Boolean
        Dim cond_name As String

        For Each instruction As Instruction In dependees
            create_bool = False
            If instruction.condition_value Then
                cond_name = instruction.condition_value
            Else
                cond_name = String.Format("cond_%s", Me.eflags_id)
                Me.eflags_id += 1
                instruction.condition_value = cond_name
                create_bool = True
            End If
            Dim c As String = instruction.get_condition()
            If c = "" Then
                Throw New Exception(instruction.mnemonic & " Not Implemented Error")
            End If
            Dim value As String
            If New String() {"nz", "ne"}.Contains(c) AndAlso name = "cmp" Then
                value = String.Format("{0} != {1}", a, b)
            ElseIf New String() {"nz", "ne"}.Contains(c) Then
                value = String.Format("{0} != 0", res)
            ElseIf New String() {"z", "e"}.Contains(c) AndAlso name = "cmp" Then
                value = String.Format("{0} == {1}", a, b)
            ElseIf New String() {"z", "e"}.Contains(c) Then
                value = String.Format("{0} == 0", res)
            ElseIf c = "s" Then
                value = String.Format("{0} < 0", signed_res)
            ElseIf c = "ns" Then
                value = String.Format("{0} >= 0", signed_res)
            ElseIf New String() {"g", "nle"}.Contains(c) AndAlso New String() {"and", "or", "xor", "test"}.Contains(name) Then
                value = String.Format("{0} > 0", signed_res)
            ElseIf New String() {"g", "nle"}.Contains(c) AndAlso name = "cmp" Then
                value = String.Format("({0}){1} > ({2}){3}", signed_type, a, signed_type, b)
            ElseIf c = "l" AndAlso New String() {"and", "or", "xor", "test"}.Contains(name) Then
                value = String.Format("{0} < 0", signed_res)
            ElseIf New String() {"ng", "le"}.Contains(c) And name = "inc" Then
                value = String.Format("{0} <= -1", signed_res)
            ElseIf New String() {"ng", "le"}.Contains(c) AndAlso name = "dec" Then
                value = String.Format("%s <= 1", signed_res)
            ElseIf New String() {"ng", "le"}.Contains(c) AndAlso New String() {"and", "or", "xor", "test"}.Contains(name) Then
                value = String.Format("{0} <= 0", signed_res)
            ElseIf New String() {"ng", "le"}.Contains(c) AndAlso name = "cmp" Then
                value = String.Format("({0}){1} <= ({2}){3}", signed_type, a, signed_type, b)
            ElseIf New String() {"ge", "nl"}.Contains(c) AndAlso New String() {"and", "or", "xor", "test"}.Contains(name) Then
                value = String.Format("{0} >= 0", signed_res)
            ElseIf New String() {"ge", "nl"}.Contains(c) AndAlso name = "cmp" Then
                value = String.Format("({0}){1} >= ({2}){3}", signed_type, a, signed_type, b)
            ElseIf New String() {"c", "b"}.Contains(c) AndAlso name = "xor" Then
                value = "false"
            ElseIf New String() {"c", "b"}.Contains(c) AndAlso name = "shr" Then
                value = String.Format("({0} >> ({1} - 1)) & 1", a, b)
            ElseIf New String() {"c", "b"}.Contains(c) AndAlso name = "add" Then
                value = String.Format("{0} < {1}", res, a)
            ElseIf New String() {"c", "b"}.Contains(c) AndAlso New String() {"neg", "sub", "cmp", "comiss", "comisd"}.Contains(name) Then
                value = String.Format("{0} < {1}", a, b)
            ElseIf New String() {"c", "b"}.Contains(c) AndAlso name = "adc" Then
                value = String.Format("{0} ? {1} <= {2} : {3} < {4}", i.get_condition_value(),
                                                     res, a, res, a)
            ElseIf New String() {"c", "b"}.Contains(c) AndAlso name = "sbb" Then
                value = String.Format("{0} ? {1} <= {2} : {3} < {4}", i.get_condition_value(),
                                                     a, b, a, b)
            ElseIf New String() {"nc", "ae"}.Contains(c) AndAlso name = "add" Then
                value = String.Format("{0} >= {1}", res, a)
            ElseIf New String() {"nc", "ae"}.Contains(c) AndAlso New String() {"neg", "sub", "cmp", "comiss", "comisd"}.Contains(name) Then
                value = String.Format("{0} >= {1}", a, b)
            ElseIf New String() {"nc", "ae"}.Contains(c) AndAlso New String() {"and", "or", "xor", "test"}.Contains(name) Then
                value = "true"
            ElseIf c = "o" AndAlso name = "mul" Then
                value = String.Format("{0} != 0", res)
            ElseIf New String() {"na", "be"}.Contains(c) AndAlso New String() {"neg", "sub"}.Contains(name) Then
                value = String.Format("{0} == 0 || {1} < {2}", res, a, b)
            ElseIf New String() {"na", "be"}.Contains(c) AndAlso New String() {"cmp", "comiss", "comisd"}.Contains(name) Then
                value = String.Format("{0} <= {1}", a, b)
            ElseIf New String() {"a", "nbe"}.Contains(c) AndAlso New String() {"neg", "sub"}.Contains(name) Then
                value = String.Format("{0} != 0 && {1} >= {2}", res, a, b)
            ElseIf New String() {"a", "nbe"}.Contains(c) AndAlso New String() {"cmp", "comiss", "comisd"}.Contains(name) Then
                value = String.Format("{0} > {1}", a, b)
            ElseIf c = "l" AndAlso name = "cmp" Then
                value = String.Format("({0}){1} < ({2}){3}", signed_type, a, signed_type, b)
            ElseIf c = "nl" AndAlso name = "cmp" Then
                value = String.Format("({0}){1} >= ({2}){3}", signed_type, a, signed_type, b)
            Else
                Throw New Exception("name: " + name + " c: " + c + " Not Implemented Error")
            End If
            If create_bool Then
                Me.writer.putlnc("bool {0};", cond_name)
            End If
            Me.writer.putlnc("{0} = {1};", cond_name, value)
        Next
    End Sub

    Public Sub [goto](ByVal test As String, ByVal address As UInteger)
        Dim is_call As Boolean
        Dim [call] As String

        If address < Me.[sub].address_start OrElse _
           address >= Me.[sub].address_end Then
            is_call = True
            [call] = String.Format("{0}()", Translator.get_function_name(address))
        Else
            is_call = False
            [call] = String.Format("goto {0}", get_label_name(address))
        End If

        Me.writer.putlnc("if ({0}) {", test)
        Me.writer.indent()
        Me.writer.putlnc("{0};", [call])

        If is_call Then
            Me.writer.putln("return;")
        End If
        Me.writer.end_brace()
    End Sub

    Public Sub call_memory(ByVal op As Operand)
        Dim addr As UInteger
        Try
            addr = eval(op.value)
        Catch ex As Exception
            Me.call_dynamic(op.ToString())
            Return
        End Try
        'TODO FIX
        Dim bytes As Byte() = Translator.access_memory(addr, 4)
        Dim value As UInteger = struct.unpack("<I", bytes)

        Dim can_deref As Boolean = False

        Try
            Translator.access_memory(value, 0)
            can_deref = True
        Catch ex As Exception
            can_deref = False
        End Try

        If can_deref = False Then
            If Translator.[imports].ContainsKey(addr) Then
                value = addr
            Else
                Me.call_dynamic(op.ToString())
                Return
            End If
        End If
        Translator.add_function(value)

        Me.writer.putln("{0}();", Translator.get_function_name(value))
    End Sub

    Public Sub call_dynamic(ByVal value As String)
        Me.writer.putln("cpu.call_dynamic({0});", value)
    End Sub

    Public Sub push_dword(ByVal value As String)
        Me.writer.putlnc("cpu.push_dword(0x{0});", value)
    End Sub

    Public Sub pop_dword(ByVal value As String)
        Me.writer.putlnc("cpu.pop_dword(&0x{0});", value)
    End Sub

    Public Sub push_float(ByVal value As String)
        Me.writer.putlnc("cpu.push_fpu({0});", value)
    End Sub

    Public Sub pop_float()
        Me.writer.putln("cpu.pop_fpu();")
    End Sub

    'Function commands below!
    Public Function on_push(ByVal i As Instruction) As Boolean
        Me.push_dword(i.op1.ToString())
        Return True
    End Function

    Public Function on_pop(ByVal i As Instruction) As Boolean
        If Not i.op1.is_register Then
            Return False
        End If
        Me.pop_dword(i.op1.value)
        Return True
    End Function

    Public Function on_mov(ByVal i As Instruction) As Boolean
        i.op2.size = i.op1.size
        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movsx(ByVal i As Instruction) As Boolean
        Dim signed1 As String = signed_types(i.op1.size)
        Dim unsigned1 As String = size_types(i.op1.size)
        Dim signed2 As String = signed_types(i.op2.size)
        Me.set_op(i.op1, String.Format("({0})(({1})(({2})({3})))", _
                                       unsigned1, signed1, signed2, _
                                        i.op2.ToString()))
        Return True
    End Function

    Public Function on_movd(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_mulss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Me.set_op(i.op1, String.Format("to_ss({0}) * to_ss({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_addss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Me.set_op(i.op1, String.Format("to_ss({0}) + to_ss({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_subss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Me.set_op(i.op1, String.Format("to_ss({0}) - to_ss({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_divss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Me.set_op(i.op1, String.Format("to_ss({0}) / to_ss({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_mulsd(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8
        Me.set_op(i.op1, String.Format("to_sd({0}) * to_sd({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_divsd(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8
        Me.set_op(i.op1, String.Format("to_sd({0}) / to_sd({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_addsd(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8
        Me.set_op(i.op1, String.Format("to_sd({0}) + to_sd({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_subsd(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8
        Me.set_op(i.op1, String.Format("to_sd({0}) - to_sd({1})", _
                                       i.op1.ToString(), _
                                       i.op2.ToString()))
        Return True
    End Function

    Public Function on_andpd(ByVal i As Instruction) As Boolean
        Me.writer.putlnc("and_pd({0}, {1});", i.op1.ToString(), i.op2.ToString())
        Return True
    End Function

    Public Function on_cvttsd2si(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 8
        Me.set_op(i.op1, String.Format("sd_to_si(to_sd({0}))", i.op2.ToString()))
        Return True
    End Function

    Public Function on_cvtsd2sd(ByVal i As Instruction) As Boolean
        ' actually cvtss2sd, probably libdasm messing up
        i.op1.size = 8
        i.op2.size = 4
        Me.set_op(i.op1, String.Format("ss_to_sd(to_ss({0}))", i.op2.ToString()))
        Return True
    End Function

    Public Function on_cvtsd2ss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 8
        Me.set_op(i.op1, String.Format("sd_to_ss(to_sd({0}))", i.op2.ToString()))
        Return True
    End Function

    Public Function on_cvttss2si(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Me.set_op(i.op1, String.Format("ss_to_si(to_ss({0}))", i.op2.ToString()))
        Return True
    End Function

    Public Function on_cvtdq2pd(ByVal i As Instruction) As Boolean
        Me.writer.putlnc("dq_to_pd({0}, {1});", i.op1.ToString(), i.op2.ToString())
        Return True
    End Function

    Public Function on_cvtps2pd(ByVal i As Instruction) As Boolean
        Me.writer.putlnc("ps_to_pd({0}, {1});", i.op1.ToString(), i.op2.ToString())
        Return True
    End Function

    Public Function on_cvtpd2ps(ByVal i As Instruction) As Boolean
        Me.writer.putlnc("pd_to_ps({0}, {1});", i.op1.ToString(), i.op2.ToString())
        Return True
    End Function

    Public Function on_cvtdq2ps(ByVal i As Instruction) As Boolean
        Me.writer.putlnc("dq_to_ps({0}, {1});", i.op1.ToString(), i.op2.ToString())
        Return True
    End Function

    Public Function on_fst(ByVal i As Instruction) As Boolean
        If i.op1.reg_type = OpCodes.REG_FPU Then
            Return False
        End If
        Dim st0 As String = get_register(REG_ST0, OpCodes.REG_FPU)
        Me.set_op(i.op1, String.Format("to_dword(float({0}))", st0))
        Return True
    End Function

    Public Function on_fistp(ByVal i As Instruction) As Boolean
        If i.fpuindex <> 7 Then
            Return False
        End If
        i.op1.size = 8
        Me.set_op(i.op1, String.Format("int64_t({0})", get_fpu()))
        Me.pop_float()
        Return True
    End Function

    Public Function on_fsubp(ByVal i As Instruction) As Boolean
        If i.op2 IsNot Nothing Then
            Return False
        End If
        Me.set_op(i.op1, String.Format("{0} - {1}", i.op1.ToString(), i.op2.ToString()))
        Me.pop_float()
        Return True
    End Function

    Public Function on_fld1(ByVal i As Instruction) As Boolean
        Me.push_float("1.0")
        Return True
    End Function

    Public Function on_fldz(ByVal i As Instruction) As Boolean
        Me.push_float("0.0")
        Return True
    End Function

    Public Function on_fld(ByVal i As Instruction) As Boolean
        Me.push_float(i.op1.ToString())
        Return True
    End Function

    Public Function on_fldl(ByVal i As Instruction) As Boolean
        Dim size_type As String = size_types(i.op1.size)
        Me.push_float(String.Format("to_ld({0}({1}))", size_type, i.op1.ToString()))
        Return True
    End Function

    Public Function on_fild(ByVal i As Instruction) As Boolean
        If i.fpuindex <> 7 Then
            Return False
        End If
        Me.push_float(String.Format("si_to_ld({0})", i.op1.ToString()))
        Return True
    End Function

    Public Function on_flds(ByVal i As Instruction) As Boolean
        Dim size_type As String = size_types(i.op1.size)
        Me.push_float(String.Format("to_ld({0}({1}))", size_type, i.op1.ToString()))
        Return True
    End Function

    Public Function on_fstp(ByVal i As Instruction) As Boolean
        Dim value As String
        Dim func As String
        If i.op1.reg_type = OpCodes.REG_FPU Then
            value = get_fpu()
        Else
            func = String.Format("ld_to_{0}", size_names(i.op1.size))
            value = String.Format("{0}({1})", func, get_fpu())
        End If
        Me.set_op(i.op1, value)
        Me.pop_float()
        Return True
    End Function

    Public Function on_fstpl(ByVal i As Instruction) As Boolean
        If i.op1.reg_type = OpCodes.REG_FPU Then
            'import code
            'code.interact(local=locals())
        End If
        Dim func As String = String.Format("ld_to_{0}", size_names(i.op1.size))
        Me.set_op(i.op1, String.Format("{0}({1})", func, get_fpu()))
        Me.pop_float()
        Return True
    End Function

    Public Function on_comiss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4
        Dim a As String = String.Format("to_ss({0})", i.op1.ToString())
        Dim b As String = String.Format("to_ss({0})", i.op2.ToString())

        Me.set_eflags(i, a, b, , i.op1.size)
        Return True
    End Function

    Public Function on_comisd(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8
        Dim a As String = String.Format("to_sd({0})", i.op1.ToString())
        Dim b As String = String.Format("to_sd({0})", i.op2.ToString())

        Me.set_eflags(i, a, b, , i.op1.size)
        Return True
    End Function

    Public Function on_movapd(ByVal i As Instruction) As Boolean
        i.op1.size = 16
        i.op2.size = 16

        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movaps(ByVal i As Instruction) As Boolean
        i.op1.size = 16
        i.op2.size = 16

        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movlpd(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8

        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movq(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8

        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movss(ByVal i As Instruction) As Boolean
        i.op1.size = 4
        i.op2.size = 4

        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movsd_sse(ByVal i As Instruction) As Boolean
        i.op1.size = 8
        i.op2.size = 8

        Me.set_op(i.op1, i.op2.ToString())
        Return True
    End Function

    Public Function on_movsd(ByVal i As Instruction) As Boolean
        If New Byte() {16, 17}.Contains(i.opcode) Then
            Return on_movsd_sse(i)
        End If

        Dim mode As Integer = i.get_address_mode()
        Dim reg_type As UInteger
        Dim size As UInteger

        If mode = MODE_32 Then
            reg_type = OpCodes.REG_GEN_DWORD
            size = 4
        Else
            reg_type = OpCodes.REG_GEN_WORD
            size = 2
        End If

        Dim ds As String = get_register(REG_DS, OpCodes.REG_SEGMENT)
        Dim esi As String = get_register(REG_ESI, reg_type)
        Dim es As String = get_register(REG_DS, OpCodes.REG_SEGMENT)
        Dim edi As String = get_register(REG_EDI, reg_type)
        Me.set_memory(String.Format("{0}+{1}", es, edi), _
                      get_memory(String.Format("{0}+{1}", ds, esi), size), size)
        Dim df As String = String.Format("(int(!{0})*2-1)*{1}", get_flag("DF"), size)

        Me.set_register(esi, String.Format("{0} + {1}", esi, df))
        Me.set_register(edi, String.Format("{0} + {1}", edi, df))

        Return True
    End Function

    Public Function on_stosd(ByVal i As Instruction) As Boolean
        Dim mode As Integer = i.get_address_mode()
        Dim reg_type As UInteger
        Dim size As UInteger

        If mode = MODE_32 Then
            reg_type = OpCodes.REG_GEN_DWORD
            size = 4
        Else
            reg_type = OpCodes.REG_GEN_WORD
            size = 2
        End If

        Dim eax As String = get_register(REG_EAX, reg_type)
        Dim es As String = get_register(REG_ES, OpCodes.REG_SEGMENT)
        Dim edi As String = get_register(REG_EDI, reg_type)


        Me.set_memory(String.Format("{0}+{1}", es, edi), eax, size)
        Dim df As String = String.Format("(int(!{0})*2-1)*{1}", get_flag("DF"), size)
        Me.set_register(edi, String.Format("{0} + {1}", edi, df))

        Return True
    End Function

    Public Function on_sbb(ByVal i As Instruction) As Boolean
        'TODO LOOK AT
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} - {1} - int({2})", _
                                              i.get_condition_value(), _
                                              i.op1.ToString(), _
                                              i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, ret(1), ret(2), ret(0), i.op1.size)

        Return True
    End Function


    Public Function on_adc(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} + {1} + int({2})", _
                                              i.get_condition_value(), _
                                              i.op1.ToString(), _
                                              i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, ret(1), ret(2), ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_sub(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} - {1}", _
                                               i.op1.ToString(), _
                                               i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, ret(1), ret(2), ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_dec(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} - 1", i.op1.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, , , ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_inc(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} + 1", i.op1.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, , , ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_and(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} & {1}", _
                                             i.op1.ToString(), i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, , , ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_or(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} | {1}", _
                                             i.op1.ToString(), i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, , , ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_xor(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} ^ {1}", _
                                             i.op1.ToString(), i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return False
        End If

        Me.set_eflags(i, , , ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_not(ByVal i As Instruction) As Boolean
        ' eflags not affected
        Dim size_type As String = size_types(i.op1.size)

        Me.set_op(i.op1, String.Format("{0}(~{1})", size_type, i.op1.ToString()))

        Return True
    End Function

    Public Function on_xorps(ByVal i As Instruction) As Boolean
        ' eflags not affected

        Me.set_op(i.op1, String.Format("{0} ^ {1}", i.op1.ToString(), i.op2.ToString()))

        Return True
    End Function

    Public Function on_lea(ByVal i As Instruction) As Boolean
        ' eflags not affected
        If i.get_operand_mode() <> MODE_32 OrElse i.get_address_mode() <> MODE_32 Then
            Throw New Exception("Check this please")
        End If

        Me.set_op(i.op1, i.op2.value)

        Return True
    End Function

    Public Function on_call(ByVal i As Instruction) As Boolean
        Dim test_ebp As Boolean = False 'TODO useless code?

        Dim esp As String = get_register(REG_ESP, OpCodes.REG_GEN_DWORD)
        Dim ebp As String = get_register(REG_EBP, OpCodes.REG_GEN_DWORD)

        Dim test_name As String  'TODO useless?
        If test_ebp Then 'TODO useless?
            test_name = String.Format("test_ebp_{0}", eip)
            Me.writer.putln("uint32_t {0} = {1};", test_name, ebp)
        End If

        Me.set_register(esp, String.Format("{0}-4", esp))

        If i.op1.is_register Then
            Me.call_dynamic(i.op1.ToString())
        ElseIf i.op1.is_immediate Then
            Translator.add_function(i.op1.value)
            Me.writer.putln(String.Format("{0}();", Translator.get_function_name(i.op1.value)))
            If i.op1.value = &H412FE0 Then 'TODO test code?
                ' startthread(), lets stop here
                'print "Found StartThread()"
                Me.unimplemented = False
                Return False
            End If
        Else
            Me.call_memory(i.op1)
        End If

        If test_ebp Then 'TODO more test code useless?
            Me.writer.putln(String.Format("if ({0} != {1}) {", ebp, test_name))
            Me.writer.indent()
            Translator.PrintLog(String.Format("EBP not preserved at 0x{0}", Hex(eip)), Color.Red)
            Me.writer.end_brace()
        End If

        Return True
    End Function

    Public Function on_xchg(ByVal i As Instruction) As Boolean
        Me.writer.start_brace()
        Dim size As String = size_types(i.op1.size)
        Me.writer.putln(String.Format("{0} swap_value = {1};", size, i.op1.ToString()))
        Me.set_op(i.op1, i.op2.ToString())
        Me.set_op(i.op2, "swap_value")
        Me.writer.end_brace()

        Return True
    End Function

    Public Function on_cbw(ByVal i As Instruction) As Boolean
        Dim ax As String

        If i.get_operand_mode() <> MODE_32 Then
            ax = get_register(REG_AX, OpCodes.REG_GEN_WORD)
            Dim al As String = get_register(REG_AL, OpCodes.REG_GEN_BYTE)
            Me.set_register(ax, String.Format("(uint16_t)int16_t(int8_t({0}))", al))
            Return True
        End If

        Dim eax As String = get_register(REG_EAX, OpCodes.REG_GEN_DWORD)
        ax = get_register(REG_AX, OpCodes.REG_GEN_WORD)
        Me.set_register(eax, String.Format("uint32_t(int32_t(int16_t({0})))", ax))

        Return True
    End Function

    Public Function on_setcc(ByVal i As Instruction) As Boolean
        Me.set_op(i.op1, String.Format("int({0})", i.get_condition_value()))

        Return True
    End Function

    Public Function on_movzx(ByVal i As Instruction) As Boolean
        Dim src As Operand = i.op2

        If src.is_memory And src.displacement <> "" Then
            'test for jumptable in text segment
            'let on_jmp handle this
            Try
                Dim displacement As String = eval(src.displacement)
                Dim section As Object = Translator.get_section(displacement)
                If section.section_name = "text" Then
                    Me.writer.putln("// movzx ignored here")
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End If
        Me.set_register(i.op1.value, i.op2.ToString())

        Return True
    End Function

    Public Function on_cmovcc(ByVal i As Instruction) As Boolean
        Me.writer.putlnc("if ({0})", i.get_condition_value())
        Me.writer.indent()
        Me.set_op(i.op1, i.op2.ToString())
        Me.writer.dedent()
        Return True
    End Function

    Public Function on_int3(ByVal i As Instruction) As Boolean
        Translator.PrintLog(String.Format("int3 at {0}", eip), Color.Red)
        Return False
    End Function

    Public Function on_nop(ByVal i As Instruction) As Boolean
        Return False
    End Function

    Public Function on_fstcw(ByVal i As Instruction) As Boolean
        Translator.PrintLog("fstcw nop", Color.Red)
        Return True
    End Function

    Public Function on_jmp(ByVal i As Instruction) As Boolean
        If i.op1.is_register Then
            Me.call_dynamic(i.op1.ToString())
            Me.writer.putln("return;")
            Return True
        End If

        If New Byte() {&HEB, &HE9}.Contains(i.opcode) AndAlso _
            i.op1.value >= Me.[sub].address_start AndAlso _
            i.op1.value < Me.[sub].address_end Then
            'jump short
            Me.writer.putln(String.Format("goto {0};", get_label_name(i.op1.value)))
            Return True
        End If

        If i.op1.is_immediate Then
            Translator.add_function(i.op1.value)
            Me.writer.putln(String.Format("{0}();", Me.get_function_name(i.op1.value)))
        Else
            'detect jumptable
            Dim reg As String = i.op1.index_reg

            If i.op1.scale <> "" Then
                Dim comp As Instruction = Me.find_instruction("cmp", -4)
                Dim indirect As Instruction = Me.find_instruction("movzx", -2)
                If comp Is Nothing Then
                    Throw New Exception(i.get_disasm() + " Not Implemented Error")
                End If
                Dim table_size As UInteger = comp.op2.value + 1
                If indirect IsNot Nothing Then
                    If indirect.op1.value <> reg Then
                        Throw New Exception("Not implemented error")
                    End If
                    Dim ind_addr As UInteger = eval(indirect.op2.displacement)
                    Dim ind_reg As String = i.op1.index_reg
                    If indirect.op2.size <> 1 Then
                        Throw New Exception("Not Implemented error")
                    End If
                    Dim indexes As New Dictionary(Of Byte, List(Of UInteger))
                    Dim index As Byte
                    'find the max value
                    For ii As UInteger = 0 To table_size
                        Dim data As String = Translator.access_memory(ind_addr + ii, 1)
                        index = struct.unpack("<B", data) 'TODO FIX
                        If indexes(index) Is Nothing Then
                            indexes(index) = New List(Of UInteger)
                        End If
                        indexes(index).Add(ii)
                    Next
                    Me.writer.putln(String.Format("switch ({0}) {", ind_reg))
                    Me.writer.indent()
                    For Each pair As KeyValuePair(Of Byte, List(Of UInteger)) In indexes
                        Dim to_index As UInteger = pair.Key
                        Dim from_list As List(Of UInteger) = pair.Value
                        For Each table_index As UInteger In from_list
                            Me.writer.putln(String.Format("case {0}:", table_index))
                        Next
                        Me.writer.indent()
                        Me.set_register(reg, String.Format("{0}", to_index))
                        Me.writer.putln("break;")
                        Me.writer.dedent()
                    Next
                    Me.writer.end_brace()
                    table_size = indexes.Max() + 1
                End If
                Dim table_addr As UInteger = eval(i.op1.displacement)

                Me.writer.putln(String.Format("switch ({0}) {", reg))
                Me.writer.indent()
                For ii As UInteger = 0 To table_size
                    Dim data As String = Translator.access_memory(table_addr + ii * i.op1.scale, 4)
                    Dim addr As UInteger = struct.unpack("<I", data)
                    If addr > Me.[sub].address_end OrElse _
                        addr < Me.[sub].address_start Then
                        Throw New Exception("Not implemented error")
                    End If
                    Dim loc As String = get_label_name(addr)
                    Me.writer.putln(String.Format("case {0}: goto {1};", ii, loc))
                    Me.[sub].add_jump(i.address, addr)
                    Me.remaining_labels.Add(addr)
                Next
                Me.writer.end_brace()
                Return True
            End If
            Me.call_memory(i.op1)
            Me.writer.putln("return;")
        End If

        Return True
    End Function

    Public Function on_jcc(ByVal i As Instruction) As Boolean
        Me.goto(i.get_condition_value, i.op1.value)
        Return True
    End Function

    Public Function on_shl(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("Not implemented error")
        End If
        Me.set_op(i.op1, String.Format("{0} << {1}", i.op1.ToString(), i.op2.ToString()))
        Return True
    End Function

    Public Function on_shld(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("Not implemented error")
        End If
        Dim func As String = String.Format("cpu.shld_{0}", size_names(i.op1.size))
        Me.set_op(i.op1, String.Format("{0}({1}, {2}, {3})", func, i.op1.ToString(), _
                                       i.op2.ToString(), i.op3.ToString()))
        Return True
    End Function

    Public Function on_shr(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} >> {1}", _
                                               i.op1.ToString(), _
                                               i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return True
        End If
        Me.set_eflags(i, ret(1), ret(2), ret(0), i.op1.size)
        Return True
    End Function

    Public Function on_rcr(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("Not implemented error")
        End If
        Dim func As String = String.Format("cpu.rcr_{0}", size_names(i.op1.size))
        Me.set_op(i.op1, String.Format("{0}({1}, {2}, {3})", func, i.op1.ToString(), _
                                       i.op2.ToString(), i.get_condition_value()))
        Return True
    End Function

    Public Function on_rol(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("Not implemented error")
        End If
        Dim func As String = String.Format("cpu.rol_{0}", size_names(i.op1.size))
        Me.set_op(i.op1, String.Format("{0}({1}, {2})", func, i.op1.ToString(), i.op2.ToString()))
        Return True
    End Function

    Public Function on_sar(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("Not implemented error")
        End If
        Dim signed_type As String = signed_types(i.op1.size)
        Me.set_op(i.op1, String.Format("(({0}){1}) >> {2}", signed_type, i.op1.ToString(), i.op2.ToString()))
        Return True
    End Function

    Public Function on_add(ByVal i As Instruction) As Boolean
        Dim ret As String() = Me.set_op_eflags(i, i.op1, "{0} + {1}", _
                                             i.op1.ToString(), _
                                             i.op2.ToString())
        If ret Is Nothing OrElse ret.Length = 0 Then
            Return True
        End If

        Me.set_eflags(i, ret(1), ret(2), ret(0), i.op1.size)
        Return True
    End Function

    Public Function on_xadd(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("Not implemented error")
        End If
        Dim func As String = String.Format("cpu.xadd_{0}", size_names(i.op1.size))
        Me.set_op(i.op1, String.Format("{0}({1}, {2})", func, i.op1.ToString(), i.op2.ToString()))
        Return True
    End Function

    Public Function on_neg(ByVal i As Instruction) As Boolean
        Dim signed_type As String = signed_types(i.op1.size)
        Dim func As String = String.Format("-({0})", signed_type) 'TODO: check ugly quickfix
        Dim ret As String() = Me.set_op_eflags(i, i.op1, func + "({0})", i.op1.ToString())  'TODO: check ugly quickfix

        If ret Is Nothing OrElse ret.Length = 0 Then
            Return True
        End If

        Me.set_eflags(i, 0, ret(1), ret(0), i.op1.size)

        Return True
    End Function

    Public Function on_test(ByVal i As Instruction) As Boolean
        Dim res As String = ""
        If i.op1.ToString() = i.op2.ToString() Then
            res = i.op1.ToString()
        Else
            res = String.Format("({0} & {1})", i.op1.ToString(), i.op2.ToString())
        End If

        Me.set_eflags(i, , , res, i.op1.size)

        Return True
    End Function

    Public Function on_cwd(ByVal i As Instruction) As Boolean
        If i.get_operand_mode() <> MODE_32 Then
            Throw New Exception("Check this please")
        End If
        Dim eax As String = get_register(REG_EAX, OpCodes.REG_GEN_DWORD)
        Dim edx As String = get_register(REG_EDX, OpCodes.REG_GEN_DWORD)
        Me.set_register(edx, String.Format("cdq_x86({0})", eax))
        Return True
    End Function

    Public Function on_div(ByVal i As Instruction) As Boolean
        If i.op2 IsNot Nothing Or i.op3 IsNot Nothing Then
            Return False
        End If
        Dim func As String = String.Format("cpu.div_{0}", size_names(i.op1.size))
        Me.writer.putlnc("{0}({0});", func, i.op1.ToString())

        Return True
    End Function

    Public Function on_idiv(ByVal i As Instruction) As Boolean
        If i.op2 IsNot Nothing Or i.op3 IsNot Nothing Then
            Return False
        End If
        Dim func As String = String.Format("cpu.idiv_{0}", size_names(i.op1.size))
        Me.writer.putlnc("{0}({0});", func, i.op1.ToString())

        Return True
    End Function

    Public Function on_imul(ByVal i As Instruction) As Boolean
        If i.eflags_dependency IsNot Nothing Then
            Throw New Exception("not implemented error")
        End If

        Dim func As String = ""
        Dim [call] As String = ""

        If i.op3 IsNot Nothing Then
            func = String.Format("cpu.imul_{0}", size_names(i.op1.size))
            [call] = String.Format("{0}({0}, {1})", func, i.op2.ToString(), i.op3.ToString())
            Me.set_op(i.op1, [call])
        ElseIf i.op2 IsNot Nothing Then
            func = String.Format("cpu.imul_{0}", size_names(i.op1.size))
            [call] = String.Format("{0}({0}, {1})", func, i.op1.ToString(), i.op2.ToString())
            Me.set_op(i.op1, [call])
        Else
            func = String.Format("cpu.imul_{0}", size_names(i.op1.size))
            Me.writer.putlnc("{0}({0});", func, i.op1.ToString())

        End If

        Return True
    End Function

    Public Function on_mul(ByVal i As Instruction) As Boolean
        Dim func As String = String.Format("cpu.mul_{0}", size_names(i.op1.size))
        Me.writer.putlnc("{0}({1});", func, i.op1.ToString())
        If i.eflags_dependency Is Nothing Then
            Return True
        End If
        Dim eax As String = get_register(REG_EAX, OpCodes.REG_GEN_DWORD)
        Dim edx As String = get_register(REG_EDX, OpCodes.REG_GEN_DWORD)
        Me.set_eflags(i, eax, , edx, i.op1.size)
        Return True
    End Function

    Public Function on_retn(ByVal i As Instruction) As Boolean
        Dim esp As String = get_register(REG_ESP, OpCodes.REG_GEN_DWORD)

        Me.set_register(esp, String.Format("{0}+{0}", esp, i.op1.value + 4))
        Me.writer.putln("return;")

        Return True
    End Function

    Public Function on_ret(ByVal i As Instruction) As Boolean
        Dim esp As String = get_register(REG_ESP, OpCodes.REG_GEN_DWORD)

        Me.set_register(esp, String.Format("{0}+4", esp))
        Me.writer.putln("return;")

        Return True
    End Function

    Public Function on_leave(ByVal i As Instruction) As Boolean
        Dim esp As String = get_register(REG_ESP, OpCodes.REG_GEN_DWORD)
        Dim ebp As String = get_register(REG_EBP, OpCodes.REG_GEN_DWORD)

        Me.set_register(esp, ebp)
        Me.set_register(ebp, "cpu.pop_dword();")

        Return True
    End Function

    Public Function on_cmp(ByVal i As Instruction) As Boolean
        Me.set_eflags(i, i.op1.ToString(), i.op2.ToString(), , i.op1.size)

        Return True
    End Function
End Class
