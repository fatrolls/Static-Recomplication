
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Diagnostics

Public Class LibDasm
    Public Sub New()
    End Sub
#Region "constants"

    ' Registers
    Public Const REGISTER_EAX As UInteger = 0
    Public Const REGISTER_ECX As UInteger = 1
    Public Const REGISTER_EDX As UInteger = 2
    Public Const REGISTER_EBX As UInteger = 3
    Public Const REGISTER_ESP As UInteger = 4
    Public Const REGISTER_EBP As UInteger = 5
    Public Const REGISTER_ESI As UInteger = 6
    Public Const REGISTER_EDI As UInteger = 7
    Public Const REGISTER_NOP As UInteger = 8
    ' no register defined
    ' Registers
    Public Const REG_EAX As UInteger = REGISTER_EAX
    Public Const REG_AX As UInteger = REG_EAX
    Public Const REG_AL As UInteger = REG_EAX
    Public Const REG_ES As UInteger = REG_EAX
    ' Just for reg_table consistence
    Public Const REG_ST0 As UInteger = REG_EAX
    ' Just for reg_table consistence
    Public Const REG_ECX As UInteger = REGISTER_ECX
    Public Const REG_CX As UInteger = REG_ECX
    Public Const REG_CL As UInteger = REG_ECX
    Public Const REG_CS As UInteger = REG_ECX
    Public Const REG_ST1 As UInteger = REG_ECX
    Public Const REG_EDX As UInteger = REGISTER_EDX
    Public Const REG_DX As UInteger = REG_EDX
    Public Const REG_DL As UInteger = REG_EDX
    Public Const REG_SS As UInteger = REG_EDX
    Public Const REG_ST2 As UInteger = REG_EDX
    Public Const REG_EBX As UInteger = REGISTER_EBX
    Public Const REG_BX As UInteger = REG_EBX
    Public Const REG_BL As UInteger = REG_EBX
    Public Const REG_DS As UInteger = REG_EBX
    Public Const REG_ST3 As UInteger = REG_EBX
    Public Const REG_ESP As UInteger = REGISTER_ESP
    Public Const REG_SP As UInteger = REG_ESP
    Public Const REG_AH As UInteger = REG_ESP
    ' Just for reg_table consistence;
    Public Const REG_FS As UInteger = REG_ESP
    Public Const REG_ST4 As UInteger = REG_ESP
    Public Const REG_EBP As UInteger = REGISTER_EBP
    Public Const REG_BP As UInteger = REG_EBP
    Public Const REG_CH As UInteger = REG_EBP
    Public Const REG_GS As UInteger = REG_EBP
    Public Const REG_ST5 As UInteger = REG_EBP
    Public Const REG_ESI As UInteger = REGISTER_ESI
    Public Const REG_SI As UInteger = REG_ESI
    Public Const REG_DH As UInteger = REG_ESI
    Public Const REG_ST6 As UInteger = REG_ESI
    Public Const REG_EDI As UInteger = REGISTER_EDI
    Public Const REG_DI As UInteger = REG_EDI
    Public Const REG_BH As UInteger = REG_EDI
    Public Const REG_ST7 As UInteger = REG_EDI
    Public Const REG_NOP As UInteger = REGISTER_NOP


    ' Register types
    Public Const REGISTER_TYPE_GEN As UInteger = 1
    Public Const REGISTER_TYPE_SEGMENT As UInteger = 2
    Public Const REGISTER_TYPE_DEBUG As UInteger = 3
    Public Const REGISTER_TYPE_CONTROL As UInteger = 4
    Public Const REGISTER_TYPE_TEST As UInteger = 5
    Public Const REGISTER_TYPE_XMM As UInteger = 6
    Public Const REGISTER_TYPE_MMX As UInteger = 7
    Public Const REGISTER_TYPE_FPU As UInteger = 8

    ' Instruction flags (prefixes)

    ' Group 1
    Public Const PREFIX_LOCK As UInteger = &H1000000
    ' 0xf0
    Public Const PREFIX_REPNE As UInteger = &H2000000
    ' 0xf2
    Public Const PREFIX_REP As UInteger = &H3000000
    ' 0xf3
    Public Const PREFIX_REPE As UInteger = &H3000000
    ' 0xf3
    ' Group 2
    Public Const PREFIX_ES_OVERRIDE As UInteger = &H10000
    ' 0x26
    Public Const PREFIX_CS_OVERRIDE As UInteger = &H20000
    ' 0x2e
    Public Const PREFIX_SS_OVERRIDE As UInteger = &H30000
    ' 0x36
    Public Const PREFIX_DS_OVERRIDE As UInteger = &H40000
    ' 0x3e
    Public Const PREFIX_FS_OVERRIDE As UInteger = &H50000
    ' 0x64
    Public Const PREFIX_GS_OVERRIDE As UInteger = &H60000
    ' 0x65
    ' Group 3 & 4
    Public Const PREFIX_OPERAND_SIZE_OVERRIDE As UInteger = &H100
    ' 0x66
    Public Const PREFIX_ADDR_SIZE_OVERRIDE As UInteger = &H1000
    ' 0x67
    ' Extensions
    Public Const EXT_G1_1 As UInteger = &H1
    Public Const EXT_G1_2 As UInteger = &H2
    Public Const EXT_G1_3 As UInteger = &H3
    Public Const EXT_G2_1 As UInteger = &H4
    Public Const EXT_G2_2 As UInteger = &H5
    Public Const EXT_G2_3 As UInteger = &H6
    Public Const EXT_G2_4 As UInteger = &H7
    Public Const EXT_G2_5 As UInteger = &H8
    Public Const EXT_G2_6 As UInteger = &H9
    Public Const EXT_G3_1 As UInteger = &HA
    Public Const EXT_G3_2 As UInteger = &HB
    Public Const EXT_G4 As UInteger = &HC
    Public Const EXT_G5 As UInteger = &HD
    Public Const EXT_G6 As UInteger = &HE
    Public Const EXT_G7 As UInteger = &HF
    Public Const EXT_G8 As UInteger = &H10
    Public Const EXT_G9 As UInteger = &H11
    Public Const EXT_GA As UInteger = &H12
    Public Const EXT_GB As UInteger = &H13
    Public Const EXT_GC As UInteger = &H14
    Public Const EXT_GD As UInteger = &H15
    Public Const EXT_GE As UInteger = &H16
    Public Const EXT_GF As UInteger = &H17
    Public Const EXT_G0 As UInteger = &H18

    ' Extra groups for 2 and 3-byte opcodes, and FPU stuff
    Public Const EXT_T2 As UInteger = &H20
    ' opcode table 2;
    Public Const EXT_CP As UInteger = &H30
    ' co-processor;
    ' Instruction type flags
    Public Const TYPE_3 As UInteger = &H80000000UI

    ' Operand flags
    Public Const FLAGS_NONE As UInteger = 0

    ' Process eflags
    Public Const EFL_CF As UInteger = (1 << 0)
    Public Const EFL_PF As UInteger = (1 << 2)
    Public Const EFL_AF As UInteger = (1 << 4)
    Public Const EFL_ZF As UInteger = (1 << 6)
    Public Const EFL_SF As UInteger = (1 << 7)
    Public Const EFL_TF As UInteger = (1 << 8)
    Public Const EFL_IF As UInteger = (1 << 9)
    Public Const EFL_DF As UInteger = (1 << 10)
    Public Const EFL_OF As UInteger = (1 << 11)
    Public Const EFL_MATH As UInteger = EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF Or EFL_CF
    Public Const EFL_BITWISE As UInteger = EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF
    Public Const EFL_ALL_COMMON As UInteger = EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF

    ' Implied operands
    Public Const IOP_EAX As UInteger = 1
    Public Const IOP_ECX As UInteger = (1 << REG_ECX)
    Public Const IOP_EDX As UInteger = (1 << REG_EDX)
    Public Const IOP_EBX As UInteger = (1 << REG_EBX)
    Public Const IOP_ESP As UInteger = (1 << REG_ESP)
    Public Const IOP_EBP As UInteger = (1 << REG_EBP)
    Public Const IOP_ESI As UInteger = (1 << REG_ESI)
    Public Const IOP_EDI As UInteger = (1 << REG_EDI)
    Public Const IOP_ALL As UInteger = IOP_EAX Or IOP_ECX Or IOP_EDX Or IOP_ESP Or IOP_EBP Or IOP_ESI Or IOP_EDI

    ' Operand Addressing Methods, from the Intel manual
    Public Const AM_A As UInteger = &H10000
    ' Direct address with segment prefix;
    Public Const AM_C As UInteger = &H20000
    ' MODRM reg field defines control register
    Public Const AM_D As UInteger = &H30000
    ' MODRM reg field defines debug register
    Public Const AM_E As UInteger = &H40000
    ' MODRM byte defines reg/memory address
    Public Const AM_G As UInteger = &H50000
    ' MODRM byte defines general-purpose reg
    Public Const AM_I As UInteger = &H60000
    ' Immediate data follows
    Public Const AM_J As UInteger = &H70000
    ' Immediate value is relative to EIP
    Public Const AM_M As UInteger = &H80000
    ' MODRM mod field can refer only to memory
    Public Const AM_O As UInteger = &H90000
    ' Displacement follows (without modrm/sib)
    Public Const AM_P As UInteger = &HA0000
    ' MODRM reg field defines MMX register
    Public Const AM_Q As UInteger = &HB0000
    ' MODRM defines MMX register or memory 
    Public Const AM_R As UInteger = &HC0000
    ' MODRM mod field can only refer to register
    Public Const AM_S As UInteger = &HD0000
    ' MODRM reg field defines segment register
    Public Const AM_T As UInteger = &HE0000
    ' MODRM reg field defines test register
    Public Const AM_V As UInteger = &HF0000
    ' MODRM reg field defines XMM register
    Public Const AM_W As UInteger = &H100000
    ' MODRM defines XMM register or memory 
    ' Extra addressing modes used in this implementation
    Public Const AM_I1 As UInteger = &H200000
    ' Immediate byte 1 encoded in instruction
    Public Const AM_REG As UInteger = &H210000
    ' Register encoded in instruction
    Public Const AM_IND As UInteger = &H220000
    ' Register indirect encoded in instruction
    ' Operand Types, from the intel manual
    Public Const OT_a As UInteger = &H1000000
    Public Const OT_b As UInteger = &H2000000
    ' always 1 byte
    Public Const OT_c As UInteger = &H3000000
    ' byte or ushort, depending on operand
    Public Const OT_d As UInteger = &H4000000
    ' double-ushort
    Public Const OT_q As UInteger = &H5000000
    ' quad-ushort
    Public Const OT_dq As UInteger = &H6000000
    ' double quad-ushort
    Public Const OT_v As UInteger = &H7000000
    ' ushort or double-ushort, depending on operand
    Public Const OT_w As UInteger = &H8000000
    ' always ushort
    Public Const OT_p As UInteger = &H9000000
    ' 32-bit or 48-bit pointer
    Public Const OT_pi As UInteger = &HA000000
    ' quauint MMX register
    Public Const OT_pd As UInteger = &HB000000
    ' 128-bit double-precision float
    Public Const OT_ps As UInteger = &HC000000
    ' 128-bit single-precision float
    Public Const OT_s As UInteger = &HD000000
    ' 6-byte pseudo descriptor
    Public Const OT_sd As UInteger = &HE000000
    ' Scalar of 128-bit double-precision float
    Public Const OT_ss As UInteger = &HF000000
    ' Scalar of 128-bit single-precision float
    Public Const OT_si As UInteger = &H10000000
    ' Doubleushort integer register
    Public Const OT_t As UInteger = &H11000000
    ' 80-bit packed FP data
    ' Operand permissions
    Public Const P_r As UInteger = &H4000
    ' Read
    Public Const P_w As UInteger = &H2000
    ' Write
    Public Const P_x As UInteger = &H1000
    ' Execute
    ' Additional operand flags
    Public Const F_s As UInteger = &H100
    ' sign-extend 1-byte immediate
    Public Const F_r As UInteger = &H200
    ' use segment register
    Public Const F_f As UInteger = &H400
    ' use FPU register
#End Region
#Region "Macros"
    Public Shared Function MASK_PREFIX_G1(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF000000UI) >> 24
    End Function

    Public Shared Function MASK_PREFIX_G2(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF0000) >> 16
    End Function

    Public Shared Function MASK_PREFIX_G3(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF00) >> 8
    End Function

    Public Shared Function MASK_PREFIX_OPERAND(ByVal x As UInteger) As UInteger
        Return ((x) And &HF00) >> 8
    End Function

    Public Shared Function MASK_PREFIX_ADDR(ByVal x As UInteger) As UInteger
        Return ((x) And &HF000) >> 12
    End Function

    Public Shared Function MASK_EXT(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF)
    End Function

    Public Shared Function MASK_TYPE_FLAGS(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF000000UI)
    End Function

    Public Shared Function MASK_TYPE_VALUE(ByVal x As UInteger) As UInteger
        Return ((x) And &HFFFFFF)
    End Function

    Public Shared Function MASK_AM(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF0000)
    End Function

    Public Shared Function MASK_OT(ByVal x As UInteger) As UInteger
        Return ((x) And &HFF000000UI)
    End Function

    Public Shared Function MASK_PERMS(ByVal x As UInteger) As UInteger
        Return ((x) And &HF000)
    End Function

    ' Operand register mask
    Public Shared Function MASK_REG(ByVal x As UInteger) As UInteger
        Return x And &HF
    End Function

    Public Shared Function MASK_FLAGS(ByVal x As UInteger) As UInteger
        Return ((x) And &HF00)
    End Function

    ' MODRM byte
    Public Shared Function MASK_MODRM_MOD(ByVal x As UInteger) As UInteger
        Return (((x) And &HC0) >> 6)
    End Function

    Public Shared Function MASK_MODRM_REG(ByVal x As UInteger) As UInteger
        Return (((x) And &H38) >> 3)
    End Function

    Public Shared Function MASK_MODRM_RM(ByVal x As UInteger) As UInteger
        Return ((x) And &H7)
    End Function
    ' SIB byte
    Public Shared Function MASK_SIB_SCALE(ByVal x As UInteger) As UInteger
        Return MASK_MODRM_MOD(x)
    End Function

    Public Shared Function MASK_SIB_INDEX(ByVal x As UInteger) As UInteger
        Return MASK_MODRM_REG(x)
    End Function

    Public Shared Function MASK_SIB_BASE(ByVal x As UInteger) As UInteger
        Return MASK_MODRM_RM(x)
    End Function

    ' Implied operands macros
    Public Shared Function IS_IOP_REG(ByVal x As UInteger, ByVal y As UInteger) As Boolean
        Return (x >> y) And 1
    End Function

    Public Shared Function IS_IOP_EAX(ByVal x As UInteger) As Boolean
        Return x And 1
    End Function

    Public Shared Function IS_IOP_ECX(ByVal x As UInteger) As Boolean
        Return (x >> REG_ECX) And 1
    End Function

    Public Shared Function IS_IOP_EDX(ByVal x As UInteger) As Boolean
        Return (x >> REG_EDX) And 1
    End Function

    Public Shared Function IS_IOP_EBX(ByVal x As UInteger) As Boolean
        Return (x >> REG_EBX) And 1
    End Function

    Public Shared Function IS_IOP_EBP(ByVal x As UInteger) As Boolean
        Return (x >> REG_EBP) And 1
    End Function

    Public Shared Function IS_IOP_ESI(ByVal x As UInteger) As Boolean
        Return (x >> REG_ESI) And 1
    End Function

    Public Shared Function IS_IOP_EDI(ByVal x As UInteger) As Boolean
        Return (x >> REG_EDI) And 1
    End Function
#End Region

#Region "Methods"
    ' Fetch instruction
    '
    '         * The operation is quite straightforward:
    '         *
    '         * - determine actual opcode (skip prefixes etc.)
    '         * - figure out which instruction table to use
    '         * - index the table with opcode
    '         * - parse operands
    '         * - fill instruction structure
    '         *
    '         * Only point where this gets hairy is those *brilliant*
    '         * opcode extensions....
    '         *
    '         


    Public Shared Function GetByteAt(ByVal s As BinaryReader, ByVal index As UInteger) As Byte
        Dim lPos As Long = s.BaseStream.Position

        s.BaseStream.Position += index
        Dim b As Byte = CByte(s.ReadByte())

        s.BaseStream.Position = lPos
        Return b
    End Function

    Public Shared Function get_instruction(ByVal s As BinaryReader, ByVal mode As Mode) As LibDasmInstruction
        Dim lPos As Long = s.BaseStream.Position
        Try
            Dim ptr As INST = Nothing
            Dim index As UInteger = 0
            Dim flags As UInteger = 0

            'memset(inst, 0, sizeof(INSTRUCTION));
            Dim inst As LibDasmInstruction = New LibDasmInstruction()

            ' Parse flags, skip prefixes etc.
            get_real_instruction(s, index, flags)

            ' Select instruction table 

            ' No extensions - normal 1-byte opcode:
            If MASK_EXT(flags) = 0 Then
                'inst.opcode = *(addr + index);
                inst.opcode = GetByteAt(s, index)


                ' FPU opcodes
                ptr = OpCodes.inst_table1(inst.opcode)
            ElseIf MASK_EXT(flags) = EXT_CP Then
                'if (*(addr + index) < 0xc0)
                If GetByteAt(s, index) < &HC0 Then
                    ' MODRM byte adds the additional byte
                    index -= 1

                    'inst.fpuindex = *(addr + index) - 0xd8;
                    inst.fpuindex = GetByteAt(s, index) - &HD8

                    'inst.opcode = *(addr + index + 1);
                    inst.opcode = GetByteAt(s, index + 1)

                    ptr = OpCodes.inst_table4(inst.fpuindex)(MASK_MODRM_REG(inst.opcode))
                Else
                    'inst.fpuindex = *(addr + index - 1) - 0xd8;
                    inst.fpuindex = GetByteAt(s, index - 1) - &HD8

                    'inst.opcode = *(addr + index);
                    inst.opcode = GetByteAt(s, index)

                    ptr = OpCodes.inst_table4(inst.fpuindex)(inst.opcode - &HB8)
                    ' 2 or 3-byte opcodes
                End If
            ElseIf MASK_EXT(flags) = EXT_T2 Then
                'inst.opcode = *(addr + index);
                inst.opcode = GetByteAt(s, index)


                ' Parse flags, skip prefixes etc. (again)
                s.BaseStream.Position += index
                ' TODO
                'get_real_instruction2(addr + index, ref flags);
                get_real_instruction2(s, flags)
                s.BaseStream.Position -= index


                ' 2-byte opcode table
                ptr = OpCodes.inst_table2(inst.opcode)

                ' 3-byte opcode tables
                If MASK_TYPE_FLAGS(ptr.type) = TYPE_3 Then
                    ' prefix 0x66
                    If MASK_PREFIX_OPERAND(flags) = 1 Then

                        ' prefix 0xf2
                        ptr = OpCodes.inst_table3_66(inst.opcode)
                    ElseIf MASK_PREFIX_G1(flags) = 2 Then

                        ' prefix 0xf3
                        ptr = OpCodes.inst_table3_f2(inst.opcode)
                    ElseIf MASK_PREFIX_G1(flags) = 3 Then

                        ptr = OpCodes.inst_table3_f3(inst.opcode)
                    End If
                End If
            End If
            ' Opcode extension tables
            If MASK_EXT(flags) <> 0 AndAlso (MASK_EXT(flags) < EXT_T2) Then
                'inst.opcode = *(addr + index);
                inst.opcode = GetByteAt(s, index)

                'inst.extindex = MASK_MODRM_REG(*(addr + index + 1));
                inst.extindex = MASK_MODRM_REG(GetByteAt(s, index + 1))

                Select Case MASK_EXT(flags)
                    Case EXT_GC
                        ' prefix 0x66
                        If MASK_PREFIX_OPERAND(flags) = 1 Then
                            ptr = OpCodes.inst_table_ext12_66(inst.extindex)
                        Else
                            ptr = OpCodes.inst_table_ext12(inst.extindex)
                        End If
                        Exit Select

                    Case EXT_GD
                        ' prefix 0x66
                        If MASK_PREFIX_OPERAND(flags) = 1 Then
                            ptr = OpCodes.inst_table_ext13_66(inst.extindex)
                        Else
                            ptr = OpCodes.inst_table_ext13(inst.extindex)
                        End If
                        Exit Select

                    Case EXT_GE
                        ' prefix 0x66
                        If MASK_PREFIX_OPERAND(flags) = 1 Then
                            ptr = OpCodes.inst_table_ext14_66(inst.extindex)
                        Else
                            ptr = OpCodes.inst_table_ext14(inst.extindex)
                        End If
                        Exit Select

                        ' monitor/mwait
                        ' XXX: hack.....
                    Case EXT_G7
                        'if (MASK_MODRM_MOD(*(addr + index + 1)) == 3)
                        If MASK_MODRM_MOD(GetByteAt(s, index + 1)) = 3 Then
                            If inst.extindex <> 1 Then
                                Return Nothing
                            End If

                            'if (MASK_MODRM_RM(*(addr + index + 1)) == 0)
                            If MASK_MODRM_RM(GetByteAt(s, index + 1)) = 0 Then
                                ptr = OpCodes.inst_monitor
                                ' index is incremented to get
                                ' correct instruction len
                                index += 1
                                'else if (MASK_MODRM_RM(*(addr + index + 1)) == 1)
                            ElseIf MASK_MODRM_RM(GetByteAt(s, index + 1)) = 1 Then
                                ptr = OpCodes.inst_mwait
                                index += 1
                            Else
                                Return Nothing

                            End If
                        Else
                            ptr = OpCodes.inst_table_ext7(inst.extindex)
                        End If
                        Exit Select
                    Case Else
                        ptr = OpCodes.inst_table_ext((MASK_EXT(flags)) - 1)(inst.extindex)
                        Exit Select
                End Select
            End If
            ' Index points now to first byte after prefixes/escapes
            index += 1

            ' Illegal instruction
            If ptr Is Nothing Then
                Return Nothing
            End If

            If ptr.mnemonic Is Nothing Then
                Return Nothing
            End If

            ' Copy instruction type
            inst.type = CType(MASK_TYPE_VALUE(ptr.type), InstructionEnum)

            ' Eflags affected by this instruction
            inst.eflags_affected = ptr.eflags_affected
            inst.eflags_used = ptr.eflags_used

            ' Pointer to instruction table
            inst.ptr = ptr


            ' Parse operands
            If get_operand(ptr, ptr.flags1, inst, inst.op1, s, index, _
             mode, flags) = 0 Then
                Return Nothing
            End If

            If get_operand(ptr, ptr.flags2, inst, inst.op2, s, index, _
             mode, flags) = 0 Then
                Return Nothing
            End If

            If get_operand(ptr, ptr.flags3, inst, inst.op3, s, index, _
             mode, flags) = 0 Then
                Return Nothing
            End If

            ' Implied operands
            inst.iop_read = ptr.iop_read
            inst.iop_written = ptr.iop_written

            ' Add modrm/sib, displacement and immediate bytes in size
            inst.length += index + inst.immbytes + inst.dispbytes

            ' Copy addressing mode
            inst.mode = mode

            ' Copy instruction flags
            inst.flags = flags

            Return inst
        Finally
            s.BaseStream.Position = lPos
        End Try
    End Function


    Public Shared Function get_instruction_string(ByVal inst As LibDasmInstruction, ByVal format As Format, ByVal offset As UInteger) As String
        Dim builder As New StringBuilder(20)

        ' Print the actual instruction string with possible prefixes etc.
        builder.Append(get_mnemonic_string(inst, format))

        builder.Append(" ")

        ' Print operands
        If get_operands_string(inst, format, offset, builder) = 0 Then
            'Return builder.ToString() & " FAIL"
        End If

        Return builder.ToString()
    End Function

    Public Shared Function get_mnemonic_string(ByVal inst As LibDasmInstruction, ByVal format__1 As Format) As String
        Dim mode__2 As Mode
        Dim builder As New StringBuilder

        ' Segment override, branch hint
        If MASK_PREFIX_G2(inst.flags) <> 0 AndAlso (inst.op1.type <> OperandEnum.OPERAND_TYPE_MEMORY) AndAlso (inst.op2.type <> OperandEnum.OPERAND_TYPE_MEMORY) Then
            ' Branch hint
            If inst.type = InstructionEnum.INSTRUCTION_TYPE_JMPC Then
                builder.Append(String.Format("{0} ", OpCodes.reg_table(OpCodes.REG_BRANCH)((MASK_PREFIX_G2(inst.flags)) - 1)))
            Else
                ' Segment override for others
                builder.Append(String.Format("{0} ", OpCodes.reg_table(OpCodes.REG_SEGMENT)((MASK_PREFIX_G2(inst.flags)) - 1)))
            End If
        End If

        ' Rep, lock etc.
        If MASK_PREFIX_G1(inst.flags) <> 0 AndAlso (MASK_EXT(inst.flags) <> EXT_T2) Then
            builder.Append(OpCodes.rep_table((MASK_PREFIX_G1(inst.flags)) - 1))
        End If

        ' Mnemonic
        ' XXX: quick hack for jcxz/jecxz.. check if there are more
        ' of these opcodes that have different mnemonic in same opcode
        If ((inst.type = InstructionEnum.INSTRUCTION_TYPE_JMPC) AndAlso (inst.opcode = &HE3)) AndAlso (MASK_PREFIX_ADDR(inst.flags) = 1) Then
            builder.Append("jcxz")
        Else
            builder.Append(inst.ptr.mnemonic)
        End If


        ' memory operation size in push/pop:
        If inst.type = InstructionEnum.INSTRUCTION_TYPE_PUSH Then
            If inst.op1.type = OperandEnum.OPERAND_TYPE_IMMEDIATE Then
                Select Case inst.op1.immbytes
                    Case 1
                        builder.Append(If(format__1 = Format.FORMAT_ATT, "b", " byte"))
                        Exit Select
                    Case 2
                        builder.Append(If(format__1 = Format.FORMAT_ATT, "w", " word"))
                        Exit Select
                    Case 4
                        builder.Append(If(format__1 = Format.FORMAT_ATT, "l", " dword"))
                        Exit Select

                End Select
            ElseIf inst.op1.type = OperandEnum.OPERAND_TYPE_MEMORY Then
                mode__2 = MODE_CHECK_OPERAND(inst.mode, inst.flags)

                If mode__2 = Mode.MODE_16 Then
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "w", " word"))
                ElseIf mode__2 = Mode.MODE_32 Then
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "l", " dword"))

                End If
            End If
            Return builder.ToString()
        End If

        If inst.type = InstructionEnum.INSTRUCTION_TYPE_POP Then
            If inst.op1.type = OperandEnum.OPERAND_TYPE_MEMORY Then
                mode__2 = MODE_CHECK_OPERAND(inst.mode, inst.flags)

                If mode__2 = Mode.MODE_16 Then
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "w", " word"))
                ElseIf mode__2 = Mode.MODE_32 Then
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "l", " dword"))
                End If
            End If
            Return builder.ToString()
        End If

        ' memory operation size in immediate to memory operations
        If inst.ptr.modrm <> 0 AndAlso (MASK_MODRM_MOD(inst.modrm) <> 3) AndAlso (MASK_AM(inst.op2.flags) = AM_I) Then

            Select Case MASK_OT(inst.op1.flags)
                Case OT_b
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "b", " byte"))
                    Exit Select
                Case OT_w
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "w", " word"))
                    Exit Select
                Case OT_d
                    builder.Append(If(format__1 = Format.FORMAT_ATT, "l", " dword"))
                    Exit Select
                Case OT_v
                    If ((inst.mode = Mode.MODE_32) AndAlso (MASK_PREFIX_OPERAND(inst.flags) = 0)) OrElse ((inst.mode = Mode.MODE_16) AndAlso (MASK_PREFIX_OPERAND(inst.flags) = 1)) Then
                        builder.Append(If(format__1 = Format.FORMAT_ATT, "l", " dword"))
                    Else
                        builder.Append(If(format__1 = Format.FORMAT_ATT, "w", " word"))
                    End If
                    Exit Select
            End Select
        End If

        ' XXX: there might be some other cases where size is needed..
        Return builder.ToString()
    End Function

    ' Print operands
    Public Shared Function get_operands_string(ByVal inst As LibDasmINSTRUCTION, ByVal format__1 As Format, ByVal offset As UInteger, ByVal builder As StringBuilder) As Integer
        If format__1 = Format.FORMAT_ATT Then
            If inst.op3.type <> OperandEnum.OPERAND_TYPE_NONE Then
                get_operand_string(inst, inst.op3, format__1, offset, builder)
                builder.Append(",")
            End If
            If inst.op2.type <> OperandEnum.OPERAND_TYPE_NONE Then
                get_operand_string(inst, inst.op2, format__1, offset, builder)
                builder.Append(",")
            End If
            If inst.op1.type <> OperandEnum.OPERAND_TYPE_NONE Then
                get_operand_string(inst, inst.op1, format__1, offset, builder)
            End If
        ElseIf format__1 = Format.FORMAT_INTEL Then
            If inst.op1.type <> OperandEnum.OPERAND_TYPE_NONE Then
                get_operand_string(inst, inst.op1, format__1, offset, builder)
            End If
            If inst.op2.type <> OperandEnum.OPERAND_TYPE_NONE Then
                builder.Append(",")
                get_operand_string(inst, inst.op2, format__1, offset, builder)
            End If
            If inst.op3.type <> OperandEnum.OPERAND_TYPE_NONE Then
                builder.Append(",")
                get_operand_string(inst, inst.op3, format__1, offset, builder)
            End If
        Else
            Return 0
        End If

        Return 1
    End Function

    Public Shared Function get_operand_string(ByVal inst As LibDasmINSTRUCTION, ByVal op As LibDasmOperand, ByVal format__1 As Format, ByVal offset As UInteger, ByVal builder As StringBuilder) As Integer
        Dim mode__2 As Mode
        Dim regtype As Integer = 0
        Dim tmp As UInteger = 0

        If op.type = OperandEnum.OPERAND_TYPE_REGISTER Then
            ' check mode
            mode__2 = MODE_CHECK_OPERAND(inst.mode, inst.flags)

            If format__1 = Format.FORMAT_ATT Then
                builder.Append("%%")
            End If

            ' Determine register type
            Select Case MASK_AM(op.flags)
                Case AM_REG
                    If MASK_FLAGS(op.flags) = F_r Then
                        regtype = OpCodes.REG_SEGMENT
                    ElseIf MASK_FLAGS(op.flags) = F_f Then
                        regtype = OpCodes.REG_FPU
                    Else
                        regtype = OpCodes.REG_GEN_DWORD
                    End If
                    Exit Select
                Case AM_E, AM_G, AM_R
                    regtype = OpCodes.REG_GEN_DWORD
                    Exit Select
                    ' control register encoded in MODRM
                Case AM_C
                    regtype = OpCodes.REG_CONTROL
                    Exit Select
                    ' debug register encoded in MODRM
                Case AM_D
                    regtype = OpCodes.REG_DEBUG
                    Exit Select
                    ' Segment register encoded in MODRM
                Case AM_S
                    regtype = OpCodes.REG_SEGMENT
                    Exit Select
                    ' TEST register encoded in MODRM
                Case AM_T
                    regtype = OpCodes.REG_TEST
                    Exit Select
                    ' MMX register encoded in MODRM
                Case AM_P, AM_Q
                    regtype = OpCodes.REG_MMX
                    Exit Select
                    ' XMM register encoded in MODRM
                Case AM_V, AM_W
                    regtype = OpCodes.REG_XMM
                    Exit Select
            End Select
            If regtype = OpCodes.REG_GEN_DWORD Then
                Select Case MASK_OT(op.flags)
                    Case OT_b
                        builder.Append(OpCodes.reg_table(OpCodes.REG_GEN_BYTE)(op.reg))
                        Exit Select
                    Case OT_v
                        builder.Append(If((mode__2 = Mode.MODE_32), OpCodes.reg_table(OpCodes.REG_GEN_DWORD)(op.reg), OpCodes.reg_table(OpCodes.REG_GEN_WORD)(op.reg)))
                        Exit Select
                    Case OT_w
                        builder.Append(OpCodes.reg_table(OpCodes.REG_GEN_WORD)(op.reg))
                        Exit Select
                    Case OT_d
                        builder.Append(OpCodes.reg_table(OpCodes.REG_GEN_DWORD)(op.reg))
                        Exit Select
                End Select
            Else
                builder.Append(OpCodes.reg_table(regtype)(op.reg))
            End If
        ElseIf op.type = OperandEnum.OPERAND_TYPE_MEMORY Then
            ' check mode
            mode__2 = MODE_CHECK_ADDR(inst.mode, inst.flags)

            ' Operand-specific segment override 
            If MASK_PREFIX_G2(inst.flags) <> 0 Then
                builder.Append(String.Format("{0}{1}:", If((format__1 = Format.FORMAT_ATT), "%", ""), OpCodes.reg_table(OpCodes.REG_SEGMENT)((MASK_PREFIX_G2(inst.flags)) - 1)))
            End If

            ' Some ATT stuff we need to check at this point
            If format__1 = Format.FORMAT_ATT Then
                ' "executable" operand
                If MASK_PERMS(op.flags) = P_x Then
                    builder.Append("*")
                End If

                ' displacement in front of brackets
                If op.dispbytes <> 0 Then
                    builder.Append(op.displacement.ToString("X"))
                End If

                ' no empty brackets - we're ready
                If (op.basereg = REG_NOP) AndAlso (op.indexreg = REG_NOP) Then
                    Return 1
                End If
            End If

            ' Open memory addressing brackets
            builder.Append(If(format__1 = Format.FORMAT_ATT, "(", "["))

            ' Base register
            If op.basereg <> REG_NOP Then
                builder.Append(String.Format("{0}{1}", If((format__1 = Format.FORMAT_ATT), "%", ""), If((mode__2 = Mode.MODE_32), OpCodes.reg_table(OpCodes.REG_GEN_DWORD)(op.basereg), OpCodes.reg_table(OpCodes.REG_GEN_WORD)(op.basereg))))
            End If

            ' Index register
            If op.indexreg <> REG_NOP Then
                If op.basereg <> REG_NOP Then
                    builder.Append(String.Format("{0}{1}", If((format__1 = Format.FORMAT_ATT), ",%", "+"), If((mode__2 = Mode.MODE_32), OpCodes.reg_table(OpCodes.REG_GEN_DWORD)(op.indexreg), OpCodes.reg_table(OpCodes.REG_GEN_WORD)(op.indexreg))))
                Else

                    builder.Append(String.Format("{0}{1}", If((format__1 = Format.FORMAT_ATT), "%", ""), If((mode__2 = Mode.MODE_32), OpCodes.reg_table(OpCodes.REG_GEN_DWORD)(op.indexreg), OpCodes.reg_table(OpCodes.REG_GEN_WORD)(op.indexreg))))
                End If
                Select Case op.scale
                    Case 2
                        builder.Append(If(format__1 = Format.FORMAT_ATT, ",2", "*2"))
                        Exit Select
                    Case 4
                        builder.Append(If(format__1 = Format.FORMAT_ATT, ",4", "*4"))
                        Exit Select
                    Case 8
                        builder.Append(If(format__1 = Format.FORMAT_ATT, ",8", "*8"))
                        Exit Select
                End Select
            End If

            ' INTEL displacement
            If inst.dispbytes <> 0 AndAlso (format__1 <> Format.FORMAT_ATT) Then
                If (op.basereg <> REG_NOP) OrElse (op.indexreg <> REG_NOP) Then
                    ' Negative displacement
                    Dim i As Integer = 1
                    Dim j As Integer = (i << CInt(op.dispbytes * 8 - 1))
                    If (op.displacement And j) <> 0 Then
                        tmp = op.displacement
                        Select Case op.dispbytes
                            Case 1
                                tmp = Not tmp And &HFF
                                Exit Select
                            Case 2
                                tmp = Not tmp And &HFFFF
                                Exit Select
                            Case 4
                                tmp = Not tmp
                                Exit Select
                        End Select
                        tmp += 1

                        ' Positive displacement
                        builder.Append(String.Format("-{0}", tmp.ToString("X")))
                    Else
                        builder.Append(String.Format("+{0}", op.displacement.ToString("X")))

                        ' Plain displacement
                    End If
                Else
                    builder.Append(op.displacement.ToString("X"))
                End If
            End If

            ' Close memory addressing brackets
            'snprintf(string + strlen(string), length - strlen(string), "%s", (format == FORMAT_ATT) ? ")" : "]"); 
            builder.Append(If(format__1 = Format.FORMAT_ATT, ")", "]"))
        ElseIf op.type = OperandEnum.OPERAND_TYPE_IMMEDIATE Then

            Select Case MASK_AM(op.flags)
                Case AM_J
                    Dim iTemp As UInteger = op.immediate + inst.length + offset
                    builder.Append(iTemp.ToString("X"))
                    Exit Select
                Case AM_I1, AM_I
                    If format__1 = Format.FORMAT_ATT Then
                        builder.Append("$")
                    End If

                    builder.Append(op.immediate.ToString("X"))
                    Exit Select
                    ' 32-bit or 48-bit address
                Case AM_A
                    builder.Append(String.Format("{0}{1}:{2}{3}", If((format__1 = Format.FORMAT_ATT), "$", ""), op.section.ToString("X"), If((format__1 = Format.FORMAT_ATT), "$", ""), op.displacement.ToString("X")))
                    Exit Select
            End Select
        Else
            Return 0
        End If

        Return 1
    End Function


    ' Parse operand and fill OPERAND structure
    '
    '         * This function is quite complex.. I'm not perfectly happy
    '         * with the logic yet. Anyway, the idea is to
    '         *
    '         * - check out modrm and sib
    '         * - based on modrm/sib and addressing method (AM_X),
    '         *   figure out the operand members and fill the struct
    '         *
    '         


    'int get_operand(PINST inst, int oflags, PINSTRUCTION instruction, POPERAND op, BYTE *data, int offset, enum Mode mode, int iflags)
    Public Shared Function get_operand(ByVal inst As INST, ByVal oflags As UInteger, ByVal instruction As LibDasmInstruction, ByRef op As LibDasmOperand, ByVal data As BinaryReader, ByVal offset As UInteger, _
     ByVal mode__1 As Mode, ByVal iflags As UInteger) As Integer
        Dim lPos As Long = data.BaseStream.Position
        Try
            op = New LibDasmOperand()
            data.BaseStream.Position += offset
            Dim addr As Byte = GetByteAt(data, 0)

            Dim index As UInteger = 0, sib As UInteger = 0, scale As UInteger = 0
            Dim reg As UInteger = REG_NOP
            Dim basereg As UInteger = REG_NOP
            Dim indexreg As UInteger = REG_NOP
            Dim dispbytes As UInteger = 0
            Dim pmode As Mode

            ' Is this valid operand?
            If oflags = FLAGS_NONE Then
                op.type = OperandEnum.OPERAND_TYPE_NONE
                Return 1
            End If

            ' Copy flags
            op.flags = oflags

            ' Set operand registers
            op.reg = REG_NOP
            op.basereg = REG_NOP
            op.indexreg = REG_NOP

            ' Offsets
            op.dispoffset = 0
            op.immoffset = 0

            ' Parse modrm and sib
            If inst.modrm <> 0 Then
                pmode = MODE_CHECK_ADDR(mode__1, iflags)

                ' Update length only once!
                If instruction.length = 0 Then
                    'instruction.modrm = *addr;
                    instruction.modrm = addr
                    instruction.length += 1
                End If

                ' Register
                'reg =  MASK_MODRM_REG(*addr);
                reg = MASK_MODRM_REG(addr)

                ' Displacement bytes
                ' SIB can also specify additional displacement, see below
                'if (MASK_MODRM_MOD(*addr) == 0) 
                If MASK_MODRM_MOD(addr) = 0 Then
                    'if ((pmode == MODE_32) && (MASK_MODRM_RM(*addr) == REG_EBP))
                    If (pmode = Mode.MODE_32) AndAlso (MASK_MODRM_RM(addr) = REG_EBP) Then
                        dispbytes = 4
                    End If

                    'if ((pmode == MODE_16) && (MASK_MODRM_RM(*addr) == REG_ESI))
                    If (pmode = Mode.MODE_16) AndAlso (MASK_MODRM_RM(addr) = REG_ESI) Then
                        dispbytes = 2

                    End If
                    'else if (MASK_MODRM_MOD(*addr) == 1) 
                ElseIf MASK_MODRM_MOD(addr) = 1 Then

                    dispbytes = 1
                    'else if (MASK_MODRM_MOD(*addr) == 2) 
                ElseIf MASK_MODRM_MOD(addr) = 2 Then
                    dispbytes = If((pmode = Mode.MODE_32), CUInt(4), CUInt(2))
                End If
                ' Base and index registers

                ' 32-bit mode
                If pmode = Mode.MODE_32 Then
                    'if ((MASK_MODRM_RM(*addr) == REG_ESP) && (MASK_MODRM_MOD(*addr) != 3)) 
                    If (MASK_MODRM_RM(addr) = REG_ESP) AndAlso (MASK_MODRM_MOD(addr) <> 3) Then
                        sib = 1
                        'instruction.sib = *(addr + 1);
                        instruction.sib = GetByteAt(data, 1)

                        ' Update length only once!
                        If instruction.length = 1 Then
                            'instruction.sib = *(addr + 1);
                            instruction.sib = GetByteAt(data, 1)
                            instruction.length += 1
                        End If

                        '
                        '                            basereg  = MASK_SIB_BASE( *(addr + 1));
                        '                            indexreg = MASK_SIB_INDEX(*(addr + 1));
                        '                            scale    = MASK_SIB_SCALE(*(addr + 1)) * 2;
                        '                            

                        basereg = MASK_SIB_BASE(GetByteAt(data, 1))
                        indexreg = MASK_SIB_INDEX(GetByteAt(data, 1))
                        scale = MASK_SIB_SCALE(GetByteAt(data, 1)) * 2

                        ' Fix scale *8
                        If scale = 6 Then
                            scale += 2
                        End If

                        ' Special case where base=ebp and MOD = 0
                        'if ((basereg == REG_EBP) && !MASK_MODRM_MOD(*addr)) 
                        If (basereg = REG_EBP) AndAlso MASK_MODRM_MOD(addr) = 0 Then
                            basereg = REG_NOP
                            dispbytes = 4
                        End If
                        If indexreg = REG_ESP Then
                            indexreg = REG_NOP
                        End If
                    Else
                        'if (!MASK_MODRM_MOD(*addr) && (MASK_MODRM_RM(*addr) == REG_EBP))
                        If MASK_MODRM_MOD(addr) = 0 AndAlso (MASK_MODRM_RM(addr) = REG_EBP) Then
                            basereg = REG_NOP
                        Else
                            'basereg = MASK_MODRM_RM(*addr);
                            basereg = MASK_MODRM_RM(addr)
                        End If
                        ' 16-bit
                    End If
                Else
                    'switch (MASK_MODRM_RM(*addr)) 
                    Select Case MASK_MODRM_RM(addr)
                        Case 0
                            basereg = REG_EBX
                            indexreg = REG_ESI
                            Exit Select
                        Case 1
                            basereg = REG_EBX
                            indexreg = REG_EDI
                            Exit Select
                        Case 2
                            basereg = REG_EBP
                            indexreg = REG_ESI
                            Exit Select
                        Case 3
                            basereg = REG_EBP
                            indexreg = REG_EDI
                            Exit Select
                        Case 4
                            basereg = REG_ESI
                            indexreg = REG_NOP
                            Exit Select
                        Case 5
                            basereg = REG_EDI
                            indexreg = REG_NOP
                            Exit Select
                        Case 6
                            'if (!MASK_MODRM_MOD(*addr))
                            If MASK_MODRM_MOD(addr) = 0 Then
                                basereg = REG_NOP
                            Else
                                basereg = REG_EBP
                            End If
                            indexreg = REG_NOP
                            Exit Select
                        Case 7
                            basereg = REG_EBX
                            indexreg = REG_NOP
                            Exit Select
                    End Select

                    'if (MASK_MODRM_MOD(*addr) == 3) 
                    If MASK_MODRM_MOD(addr) = 3 Then
                        'basereg  = MASK_MODRM_RM(*addr);
                        basereg = MASK_MODRM_RM(addr)
                        indexreg = REG_NOP
                    End If
                End If
            End If

            ' Operand addressing method -specific parsing
            Select Case MASK_AM(oflags)
                ' Register encoded in instruction
                Case AM_REG
                    op.type = OperandEnum.OPERAND_TYPE_REGISTER
                    op.reg = MASK_REG(oflags)
                    Exit Select

                    ' Register indirect encoded in instruction
                Case AM_IND
                    op.type = OperandEnum.OPERAND_TYPE_MEMORY
                    op.basereg = MASK_REG(oflags)
                    Exit Select

                    ' Register/memory encoded in MODRM
                Case AM_M
                    'if (MASK_MODRM_MOD(*addr) == 3)
                    If MASK_MODRM_MOD(addr) = 3 Then
                        Return 0
                    End If

                    'goto skip_rest;
                    GoTo case_AM_Q
                Case AM_R
                    'if (MASK_MODRM_MOD(*addr) != 3)
                    If MASK_MODRM_MOD(addr) <> 3 Then
                        Return 0
                    End If
                    GoTo case_AM_Q
                    'skip_rest:
                Case AM_Q, AM_W, AM_E
case_AM_Q:
                    op.type = OperandEnum.OPERAND_TYPE_MEMORY
                    op.dispbytes = dispbytes
                    instruction.dispbytes = dispbytes
                    op.basereg = basereg
                    op.indexreg = indexreg
                    op.scale = scale

                    index = If((sib <> 0), CUInt(1), CUInt(0))

                    If dispbytes <> 0 Then
                        op.dispoffset = index + 1 + offset
                    End If

                    Select Case dispbytes
                        Case 0
                            Exit Select
                        Case 1
                            'op.displacement = FETCH8(addr + 1 + index);
                            op.displacement = FETCH8(data, 1 + index)
                            ' Always sign-extend
                            If op.displacement >= &H80 Then
                                op.displacement = op.displacement Or &HFFFFFF00UI
                            End If
                            Exit Select
                        Case 2
                            ' op.displacement = FETCH16(addr + 1 + index);
                            op.displacement = FETCH16(data, 1 + index)
                            Exit Select
                        Case 4
                            ' op.displacement = FETCH32(addr + 1 + index);
                            op.displacement = FETCH32(data, 1 + index)
                            Exit Select
                    End Select

                    ' MODRM defines register
                    'if ((basereg != REG_NOP) && (MASK_MODRM_MOD(*addr) == 3)) 
                    If (basereg <> REG_NOP) AndAlso (MASK_MODRM_MOD(addr) = 3) Then
                        op.type = OperandEnum.OPERAND_TYPE_REGISTER
                        op.reg = basereg
                    End If
                    Exit Select

                    ' Immediate byte 1 encoded in instruction
                Case AM_I1
                    op.type = OperandEnum.OPERAND_TYPE_IMMEDIATE
                    op.immbytes = 1
                    op.immediate = 1
                    Exit Select
                    ' Immediate value
                Case AM_J
                    op.type = OperandEnum.OPERAND_TYPE_IMMEDIATE
                    ' Always sign-extend
                    oflags = oflags Or F_s
                    GoTo case_AM_I
                Case AM_I
case_AM_I:
                    op.type = OperandEnum.OPERAND_TYPE_IMMEDIATE
                    index = If((inst.modrm <> 0), CUInt(1), CUInt(0))
                    index += If((sib <> 0), CUInt(1), CUInt(0))
                    index += instruction.immbytes
                    index += instruction.dispbytes
                    op.immoffset = index + offset

                    ' check mode
                    mode__1 = MODE_CHECK_OPERAND(mode__1, iflags)

                    Select Case MASK_OT(oflags)
                        Case OT_b
                            op.immbytes = 1
                            ' op.immediate = FETCH8(addr + index);
                            op.immediate = FETCH8(data, index)

                            If (op.immediate >= &H80) AndAlso (MASK_FLAGS(oflags) = F_s) Then
                                op.immediate = op.immediate Or &HFFFFFF00UI
                            End If
                            Exit Select

                        Case OT_v
                            op.immbytes = If((mode__1 = Mode.MODE_32), CUInt(4), CUInt(2))
                            ' op.immediate = (mode == MODE_32) ? FETCH32(addr + index) : FETCH16(addr + index); 
                            op.immediate = If((mode__1 = Mode.MODE_32), FETCH32(data, index), FETCH16(data, index))
                            Exit Select

                        Case OT_w
                            op.immbytes = 2
                            ' op.immediate =	FETCH16(addr + index);
                            op.immediate = FETCH16(data, index)
                            Exit Select
                    End Select
                    instruction.immbytes += op.immbytes
                    Exit Select

                    ' 32-bit or 48-bit address
                Case AM_A
                    op.type = OperandEnum.OPERAND_TYPE_IMMEDIATE
                    ' check mode
                    mode__1 = MODE_CHECK_OPERAND(mode__1, iflags)

                    op.dispbytes = If((mode__1 = Mode.MODE_32), CUInt(6), CUInt(4))
                    ' op.displacement = (mode == MODE_32) ? FETCH32(addr) : FETCH16(addr);
                    op.displacement = If((mode__1 = Mode.MODE_32), FETCH32(data, 0), FETCH16(data, 0))
                    ' op.section = FETCH16(addr + op.dispbytes - 2);
                    op.section = CUShort(FETCH16(data, op.dispbytes - 2))

                    instruction.dispbytes = op.dispbytes
                    instruction.sectionbytes = 2
                    Exit Select

                    ' Plain displacement without MODRM/SIB
                Case AM_O
                    op.type = OperandEnum.OPERAND_TYPE_MEMORY
                    Select Case MASK_OT(oflags)
                        Case OT_b
                            op.dispbytes = 1
                            ' op.displacement = FETCH8(addr);
                            op.displacement = FETCH8(data, 0)
                            Exit Select
                        Case OT_v
                            op.dispbytes = If((mode__1 = Mode.MODE_32), CUInt(4), CUInt(2))
                            'op.displacement = (mode == MODE_32) ? FETCH32(addr) : FETCH16(addr);
                            op.displacement = If((mode__1 = Mode.MODE_32), FETCH32(data, 0), FETCH16(data, 0))
                            Exit Select
                    End Select
                    instruction.dispbytes = op.dispbytes
                    op.dispoffset = offset
                    Exit Select

                    ' General-purpose register encoded in MODRM
                Case AM_G
                    op.type = OperandEnum.OPERAND_TYPE_REGISTER
                    op.reg = reg
                    Exit Select

                    ' control register encoded in MODRM
                    ' debug register encoded in MODRM
                    ' Segment register encoded in MODRM
                    ' TEST register encoded in MODRM
                    ' MMX register encoded in MODRM
                    ' XMM register encoded in MODRM
                Case AM_C, AM_D, AM_S, AM_T, AM_P, AM_V
                    op.type = OperandEnum.OPERAND_TYPE_REGISTER
                    op.reg = MASK_MODRM_REG(instruction.modrm)
                    Exit Select
                Case Else
                    Exit Select
            End Select
            Return 1
        Finally
            data.BaseStream.Position = lPos
        End Try
    End Function


    ''' Parse instruction flags, get opcode index
    Public Shared Function get_real_instruction(ByVal s As BinaryReader, ByRef index As UInteger, ByRef flags As UInteger) As Integer
        Dim lPos As Long = s.BaseStream.Position
        ' Backup position
        Dim addr As Byte = GetByteAt(s, 0)

        Select Case addr
            ' 2-byte opcode
            Case &HF
                index += 1
                flags = flags Or EXT_T2
                Exit Select

                ' Prefix group 2
            Case &H2E
                index += 1
                ' Clear previous flags from same group (undefined effect)
                flags = flags And &HFF00FFFFUI
                flags = flags Or PREFIX_CS_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &H36
                index += 1
                flags = flags And &HFF00FFFFUI
                flags = flags Or PREFIX_SS_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &H3E
                index += 1
                flags = flags And &HFF00FFFFUI
                flags = flags Or PREFIX_DS_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &H26
                index += 1
                flags = flags And &HFF00FFFFUI
                flags = flags Or PREFIX_ES_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &H64
                index += 1
                flags = flags And &HFF00FFFFUI
                flags = flags Or PREFIX_FS_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &H65
                index += 1
                flags = flags And &HFF00FFFFUI
                flags = flags Or PREFIX_GS_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select

                ' Prefix group 3 or 3-byte opcode
            Case &H66
                ' Do not clear flags from the same group!!!!
                index += 1
                flags = flags Or PREFIX_OPERAND_SIZE_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select

                ' Prefix group 4
            Case &H67
                ' Do not clear flags from the same group!!!!
                index += 1
                flags = flags Or PREFIX_ADDR_SIZE_OVERRIDE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)

                Exit Select

                ' Extension group 1
            Case &H80
                flags = flags Or EXT_G1_1
                Exit Select
            Case &H81
                flags = flags Or EXT_G1_2
                Exit Select
            Case &H82
                flags = flags Or EXT_G1_1
                Exit Select
            Case &H83
                flags = flags Or EXT_G1_3
                Exit Select

                ' Extension group 2
            Case &HC0
                flags = flags Or EXT_G2_1
                Exit Select
            Case &HC1
                flags = flags Or EXT_G2_2
                Exit Select
            Case &HD0
                flags = flags Or EXT_G2_3
                Exit Select
            Case &HD1
                flags = flags Or EXT_G2_4
                Exit Select
            Case &HD2
                flags = flags Or EXT_G2_5
                Exit Select
            Case &HD3
                flags = flags Or EXT_G2_6
                Exit Select

                ' Escape to co-processor
            Case &HD8, &HD9, &HDA, &HDB, &HDC, &HDD, _
             &HDE, &HDF
                index += 1
                flags = flags Or EXT_CP
                Exit Select

                ' Prefix group 1 or 3-byte opcode
            Case &HF0
                index += 1
                flags = flags And &HFFFFFF
                flags = flags Or PREFIX_LOCK

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &HF2
                index += 1
                flags = flags And &HFFFFFF
                flags = flags Or PREFIX_REPNE

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select
            Case &HF3
                index += 1
                flags = flags And &HFFFFFF
                flags = flags Or PREFIX_REP

                'get_real_instruction(addr + 1, index, flags);
                s.BaseStream.Position += 1
                get_real_instruction(s, index, flags)
                Exit Select

                ' Extension group 3
            Case &HF6
                flags = flags Or EXT_G3_1
                Exit Select
            Case &HF7
                flags = flags Or EXT_G3_2
                Exit Select

                ' Extension group 4
            Case &HFE
                flags = flags Or EXT_G4
                Exit Select

                ' Extension group 5
            Case &HFF
                flags = flags Or EXT_G5
                Exit Select
            Case Else
                Exit Select
        End Select
        s.BaseStream.Position = lPos
        ' Restore
        Return 0
    End Function
    Public Shared Function get_real_instruction2(ByVal s As BinaryReader, ByRef flags As UInteger) As Integer
        Dim addr As Byte = CByte(s.ReadByte())
        s.BaseStream.Position -= 1

        Select Case addr
            ' opcode extensions for 2-byte opcodes
            Case &H0
                ' Clear extension
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_G6
                Exit Select
            Case &H1
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_G7
                Exit Select
            Case &H71
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_GC
                Exit Select
            Case &H72
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_GD
                Exit Select
            Case &H73
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_GE
                Exit Select
            Case &HAE
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_GF
                Exit Select
            Case &HBA
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_G8
                Exit Select
            Case &HC7
                flags = flags And &HFFFFFF00UI
                flags = flags Or EXT_G9
                Exit Select
            Case Else
                Exit Select
        End Select
        Return 0
    End Function

#End Region

#Region "Helper functions"

    Public Shared Function FETCH8(ByVal addr As BinaryReader, ByVal offset As UInteger) As UInteger
        'Debug.WriteLine(addr.BaseStream.Position.ToString());

        ' So far byte cast seems to work on all tested platforms
        Dim lPos As Long = addr.BaseStream.Position
        addr.BaseStream.Position += offset
        Dim res As Byte = addr.ReadByte()
        addr.BaseStream.Position = lPos

        Return CUInt(res)
    End Function

    Public Shared Function FETCH16(ByVal addr As BinaryReader, ByVal offset As UInteger) As UInteger
        'Debug.WriteLine(addr.BaseStream.Position.ToString());

        Dim lPos As Long = addr.BaseStream.Position
        addr.BaseStream.Position += offset
        Dim res As UInteger = CUInt(addr.ReadInt16())

        If res >= &H8000 Then
            res = res And &HFFFF
        End If

        addr.BaseStream.Position = lPos
        Return CUInt(res)
    End Function

    Public Shared Function FETCH32(ByVal addr As BinaryReader, ByVal offset As UInteger) As UInteger
        'Debug.WriteLine(addr.BaseStream.Position.ToString());

        Dim lPos As Long = addr.BaseStream.Position
        addr.BaseStream.Position += offset
        Dim res As Integer = addr.ReadInt32()
        addr.BaseStream.Position = lPos

        Return CUInt(res)
    End Function

    Public Shared Function MODE_CHECK_ADDR(ByVal mode__1 As Mode, ByVal flags As UInteger) As Mode
        If ((mode__1 = Mode.MODE_32) AndAlso (MASK_PREFIX_ADDR(flags) = 0)) OrElse ((mode__1 = Mode.MODE_16) AndAlso (MASK_PREFIX_ADDR(flags) = 1)) Then
            Return Mode.MODE_32
        Else
            Return Mode.MODE_16
        End If
    End Function

    Public Shared Function MODE_CHECK_OPERAND(ByVal mode__1 As Mode, ByVal flags As UInteger) As Mode
        If ((mode__1 = Mode.MODE_32) AndAlso (MASK_PREFIX_OPERAND(flags) = 0)) OrElse ((mode__1 = Mode.MODE_16) AndAlso (MASK_PREFIX_OPERAND(flags) = 1)) Then
            Return Mode.MODE_32
        Else
            Return Mode.MODE_16
        End If
    End Function

    Public Shared Function get_register_type(ByVal op As LibDasmOperand) As UInteger
        If op.type <> OperandEnum.OPERAND_TYPE_REGISTER Then
            Return 0
        End If

        Select Case MASK_AM(op.flags)
            Case AM_REG
                If MASK_FLAGS(op.flags) = F_r Then
                    Return REGISTER_TYPE_SEGMENT
                ElseIf MASK_FLAGS(op.flags) = F_f Then
                    Return REGISTER_TYPE_FPU
                Else
                    Return REGISTER_TYPE_GEN
                End If
            Case AM_E, AM_G, AM_R
                Return REGISTER_TYPE_GEN
            Case AM_C
                Return REGISTER_TYPE_CONTROL
            Case AM_D
                Return REGISTER_TYPE_DEBUG
            Case AM_S
                Return REGISTER_TYPE_SEGMENT
            Case AM_T
                Return REGISTER_TYPE_TEST
            Case AM_P, AM_Q
                Return REGISTER_TYPE_MMX
            Case AM_V, AM_W
                Return REGISTER_TYPE_XMM
            Case Else
                Exit Select
        End Select
        Return 0
    End Function

    Public Shared Function get_operand_type(ByVal op As LibDasmOperand) As UInteger
        Return CUInt(op.type)
    End Function

    Public Shared Function get_operand_register(ByVal op As LibDasmOperand) As UInteger
        Return op.reg
    End Function

    Public Shared Function get_operand_basereg(ByVal op As LibDasmOperand) As UInteger
        Return op.basereg
    End Function

    Public Shared Function get_operand_indexreg(ByVal op As LibDasmOperand) As UInteger
        Return op.indexreg
    End Function

    Public Shared Function get_operand_scale(ByVal op As LibDasmOperand) As UInteger
        Return op.scale
    End Function

    Public Shared Function get_operand_immediate(ByVal op As LibDasmOperand, ByRef imm As UInteger) As UInteger
        If op.immbytes <> 0 Then
            imm = op.immediate
            Return 1
        Else
            Return 0
        End If
    End Function

    Public Shared Function get_operand_displacement(ByVal op As LibDasmOperand, ByRef disp As UInteger) As Integer
        If op.dispbytes <> 0 Then
            disp = op.displacement
            Return 1
        Else
            Return 0
        End If
    End Function

    ' XXX: note that source and destination are not always literal
    Public Shared Function get_source_operand(ByVal inst As LibDasmInstruction) As LibDasmOperand
        If inst.op2.type <> OperandEnum.OPERAND_TYPE_NONE Then
            Return inst.op2
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function get_destination_operand(ByVal inst As LibDasmInstruction) As LibDasmOperand
        If inst.op1.type <> OperandEnum.OPERAND_TYPE_NONE Then
            Return inst.op1
        Else
            Return Nothing
        End If
    End Function
#End Region

End Class

' Disassembling mode
Public Enum Mode
    MODE_32
    ' 32-bit
    MODE_16
    ' 16-bit
End Enum

' Disassembling format
Public Enum Format
    FORMAT_ATT
    FORMAT_INTEL
End Enum

' Instruction types (just the most common ones atm)
Public Enum InstructionEnum As UInteger
    ' Integer instructions
    INSTRUCTION_TYPE_ASC ' aaa, aam, etc.
    INSTRUCTION_TYPE_DCL ' daa, das
    INSTRUCTION_TYPE_MOV
    INSTRUCTION_TYPE_MOVSR ' segment register
    INSTRUCTION_TYPE_ADD
    INSTRUCTION_TYPE_XADD
    INSTRUCTION_TYPE_ADC
    INSTRUCTION_TYPE_SUB
    INSTRUCTION_TYPE_SBB
    INSTRUCTION_TYPE_INC
    INSTRUCTION_TYPE_DEC
    INSTRUCTION_TYPE_DIV
    INSTRUCTION_TYPE_IDIV
    INSTRUCTION_TYPE_NOT
    INSTRUCTION_TYPE_NEG
    INSTRUCTION_TYPE_STOS
    INSTRUCTION_TYPE_LODS
    INSTRUCTION_TYPE_SCAS
    INSTRUCTION_TYPE_MOVS
    INSTRUCTION_TYPE_MOVSX
    INSTRUCTION_TYPE_MOVZX
    INSTRUCTION_TYPE_CMPS
    INSTRUCTION_TYPE_SHX ' signed/unsigned shift left/right
    INSTRUCTION_TYPE_ROX ' signed/unsigned rot left/right
    INSTRUCTION_TYPE_MUL
    INSTRUCTION_TYPE_IMUL
    INSTRUCTION_TYPE_EIMUL ' "extended" imul with 2-3 operands
    INSTRUCTION_TYPE_XOR
    INSTRUCTION_TYPE_LEA
    INSTRUCTION_TYPE_XCHG
    INSTRUCTION_TYPE_CMP
    INSTRUCTION_TYPE_TEST
    INSTRUCTION_TYPE_PUSH
    INSTRUCTION_TYPE_AND
    INSTRUCTION_TYPE_OR
    INSTRUCTION_TYPE_POP
    INSTRUCTION_TYPE_JMP
    INSTRUCTION_TYPE_JMPC ' conditional jump
    INSTRUCTION_TYPE_JECXZ
    INSTRUCTION_TYPE_SETC ' conditional byte set
    INSTRUCTION_TYPE_MOVC ' conditional mov
    INSTRUCTION_TYPE_LOOP
    INSTRUCTION_TYPE_CALL
    INSTRUCTION_TYPE_RET
    INSTRUCTION_TYPE_ENTER
    INSTRUCTION_TYPE_INT ' interrupt
    INSTRUCTION_TYPE_BT ' bit tests
    INSTRUCTION_TYPE_BTS
    INSTRUCTION_TYPE_BTR
    INSTRUCTION_TYPE_BTC
    INSTRUCTION_TYPE_BSF
    INSTRUCTION_TYPE_BSR
    INSTRUCTION_TYPE_BSWAP
    INSTRUCTION_TYPE_SGDT
    INSTRUCTION_TYPE_SIDT
    INSTRUCTION_TYPE_SLDT
    INSTRUCTION_TYPE_LFP
    INSTRUCTION_TYPE_CLD
    INSTRUCTION_TYPE_STD
    INSTRUCTION_TYPE_XLAT
    ' FPU instructions
    INSTRUCTION_TYPE_FCMOVC ' float conditional mov
    INSTRUCTION_TYPE_FADD
    INSTRUCTION_TYPE_FADDP
    INSTRUCTION_TYPE_FIADD
    INSTRUCTION_TYPE_FSUB
    INSTRUCTION_TYPE_FSUBP
    INSTRUCTION_TYPE_FISUB
    INSTRUCTION_TYPE_FSUBR
    INSTRUCTION_TYPE_FSUBRP
    INSTRUCTION_TYPE_FISUBR
    INSTRUCTION_TYPE_FMUL
    INSTRUCTION_TYPE_FMULP
    INSTRUCTION_TYPE_FIMUL
    INSTRUCTION_TYPE_FDIV
    INSTRUCTION_TYPE_FDIVP
    INSTRUCTION_TYPE_FDIVR
    INSTRUCTION_TYPE_FDIVRP
    INSTRUCTION_TYPE_FIDIV
    INSTRUCTION_TYPE_FIDIVR
    INSTRUCTION_TYPE_FCOM
    INSTRUCTION_TYPE_FCOMP
    INSTRUCTION_TYPE_FCOMPP
    INSTRUCTION_TYPE_FCOMI
    INSTRUCTION_TYPE_FCOMIP
    INSTRUCTION_TYPE_FUCOM
    INSTRUCTION_TYPE_FUCOMP
    INSTRUCTION_TYPE_FUCOMPP
    INSTRUCTION_TYPE_FUCOMI
    INSTRUCTION_TYPE_FUCOMIP
    INSTRUCTION_TYPE_FST
    INSTRUCTION_TYPE_FSTP
    INSTRUCTION_TYPE_FIST
    INSTRUCTION_TYPE_FISTP
    INSTRUCTION_TYPE_FISTTP
    INSTRUCTION_TYPE_FLD
    INSTRUCTION_TYPE_FILD
    INSTRUCTION_TYPE_FICOM
    INSTRUCTION_TYPE_FICOMP
    INSTRUCTION_TYPE_FFREE
    INSTRUCTION_TYPE_FFREEP
    INSTRUCTION_TYPE_FXCH
    INSTRUCTION_TYPE_SYSENTER
    INSTRUCTION_TYPE_FPU_CTRL 'FPU control instruction
    INSTRUCTION_TYPE_FPU ' Other FPU instructions
    INSTRUCTION_TYPE_MMX ' Other MMX instructions
    INSTRUCTION_TYPE_SSE ' Other SSE instructions
    INSTRUCTION_TYPE_OTHER ' Other instructions :-)
    INSTRUCTION_TYPE_PRIV ' Privileged instruction
End Enum

' Operand types
Public Enum OperandEnum
    OPERAND_TYPE_NONE ' operand not present
    OPERAND_TYPE_MEMORY ' memory operand ([eax], [0], etc.)
    OPERAND_TYPE_REGISTER ' register operand (eax, mm0, etc.)
    OPERAND_TYPE_IMMEDIATE ' immediate operand (0x1234)
End Enum


' Structure definitions
' struct INST is used internally by the library
Public Class INST
    Public type As UInteger          ' Instruction type and flags
    Public mnemonic As String        ' Instruction mnemonic
    Public flags1 As UInteger        ' First operand flags (if any)
    Public flags2 As UInteger        ' Second operand flags (if any)
    Public flags3 As UInteger        ' Additional operand flags (if any)
    Public modrm As UInteger         ' Is MODRM byte present?
    Public eflags_affected As UShort ' Processor eflags affected
    Public eflags_used As UShort     ' Processor eflags used by this instruction
    Public iop_written As UInteger   ' mask of affected implied registers (written)
    Public iop_read As UInteger      ' mask of affected implied registers (read)

    Public Sub New(ByVal ptype As UInteger, ByVal pmnemonic As String, ByVal pflags1 As UInteger, ByVal pflags2 As UInteger, ByVal pflags3 As UInteger, ByVal pmodrm As UInteger, ByVal eflags_affected As UShort, ByVal eflags_used As UShort, ByVal iop_written As UInteger, ByVal iop_read As UInteger)
        Me.type = ptype
        Me.mnemonic = pmnemonic
        Me.flags1 = pflags1
        Me.flags2 = pflags2
        Me.flags3 = pflags3
        Me.modrm = pmodrm
        Me.eflags_affected = eflags_affected
        Me.eflags_used = eflags_used
        Me.iop_written = iop_written
        Me.iop_read = iop_read
    End Sub
End Class