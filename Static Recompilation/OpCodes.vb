
Imports System.Collections.Generic
Imports System.Text

'
' * libdasm -- simple x86 disassembly library
' * (c) 2004 - 2006  jt / nologin.org
' *
' * opcode_tables.h:
' * Opcode tables for FPU, 1, 2 and 3-byte opcodes and
' * extensions.
' *
' 


Public Class OpCodes
    Inherits LibDasm
' lock/rep prefix name table
    Public Shared rep_table As String() = New String() {"lock ", "repne ", "rep "}

' Register name table (also includes Jcc branch hint prefixes)
    Public Shared reg_table As String()() = New String()() {
        New String() {"eax", "ecx", "edx", "ebx", "esp", "ebp", "esi", "edi"}, _
        New String() {"ax", "cx", "dx", "bx", "sp", "bp", "si", "di"}, _
        New String() {"al", "cl", "dl", "bl", "ah", "ch", "dh", "bh"}, _
        New String() {"es", "cs", "ss", "ds", "fs", "gs", "??", "??"}, _
        New String() {"dr0", "dr1", "dr2", "dr3", "dr4", "dr5", "dr6", "dr7"}, _
        New String() {"cr0", "cr1", "cr2", "cr3", "cr4", "cr5", "cr6", "cr7"}, _
        New String() {"tr0", "tr1", "tr2", "tr3", "tr4", "tr5", "tr6", "tr7"}, _
        New String() {"xmm0", "xmm1", "xmm2", "xmm3", "xmm4", "xmm5", "xmm6", "xmm7"}, _
        New String() {"mm0", "mm1", "mm2", "mm3", "mm4", "mm5", "mm6", "mm7"}, _
        New String() {"st(0)", "st(1)", "st(2)", "st(3)", "st(4)", "st(5)", "st(6)", "st(7)"}, _
        New String() {"??", "(bnt)", "??", "(bt)", "??", "??", "??", "??"}}

' Name table index
    Public Const REG_GEN_DWORD As UInteger = 0
    Public Const REG_GEN_WORD As UInteger = 1
    Public Const REG_GEN_BYTE As UInteger = 2
    Public Const REG_SEGMENT As UInteger = 3
    Public Const REG_DEBUG As UInteger = 4
    Public Const REG_CONTROL As UInteger = 5
    Public Const REG_TEST As UInteger = 6
    Public Const REG_XMM As UInteger = 7
    Public Const REG_MMX As UInteger = 8
    Public Const REG_FPU As UInteger = 9
    Public Const REG_BRANCH As UInteger = 10

    'Custom added to fix a certain problem
    Public Const REG_GEN As UInteger = 11

' Not registers strictly speaking..
' 1-byte opcodes
' Escape to 2-byte opcode table
' seg ES override
' seg CS override
' seg SS override
' seg DS override
' seg FS override
' seg GS override
' operand size override
' address size override
    Public Shared inst_table1 As INST() = New INST() { _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_ES Or F_r Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_ES Or F_r Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_CS Or F_r Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_SS Or F_r Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_SS Or F_r Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_MATH, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_DS Or F_r Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_DS Or F_r Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DCL), "daa", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_CF Or EFL_AF Or EFL_SF Or EFL_ZF Or EFL_PF, EFL_CF, IOP_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_MATH, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DCL), "das", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_CF Or EFL_AF Or EFL_SF Or EFL_ZF Or EFL_PF, EFL_CF, IOP_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_REG Or REG_EAX Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_BITWISE, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ASC), "aaa", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_AF Or EFL_CF, 0, IOP_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_E Or OT_b Or P_r, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_E Or OT_v Or P_r, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_G Or OT_b Or P_r, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_G Or OT_v Or P_r, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_REG Or REG_EAX Or OT_b Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_ALL_COMMON, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_REG Or REG_EAX Or OT_v Or P_r, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_ALL_COMMON, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ASC), "aas", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_AF Or EFL_CF, 0, IOP_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_EAX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_ECX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_EDX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_EBX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_ESP Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_EBP Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_ESI Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_REG Or REG_EDI Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_EAX Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_ECX Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_EDX Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_EBX Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_ESP Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_EBP Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_ESI Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_REG Or REG_EDI Or OT_v, FLAGS_NONE, FLAGS_NONE, 0, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_EAX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_ECX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_EDX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_EBX Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_ESP Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_EBP Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_ESI Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_EDI Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_EAX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_ECX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_EDX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_EBX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_ESP Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_EBP Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_ESI Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_EDI Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "pusha", FLAGS_NONE Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ALL), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "popa", FLAGS_NONE Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ALL, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "bound", AM_G Or OT_v Or P_r, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "arpl", AM_E Or OT_w Or P_r, AM_G Or OT_w Or P_r, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_I Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_EIMUL), "imul", AM_G Or OT_v Or P_r, AM_E Or OT_v Or P_r, AM_I Or OT_v Or P_r, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_EIMUL), "imul", AM_G Or OT_v Or P_r, AM_E Or OT_v Or P_r, AM_I Or OT_b Or F_s Or P_r, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "insb", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, IOP_EDI, IOP_EDX Or IOP_EDI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "insv", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, IOP_EDI, IOP_EDX Or IOP_EDI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "outsb", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, IOP_ESI, IOP_EDX Or IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "outsv", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, IOP_ESI, IOP_EDX Or IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jo", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_OF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jno", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_OF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jc", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jnc", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jz", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jnz", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jna", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF Or EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "ja", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF Or EFL_CF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "js", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jns", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jp", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_PF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jpo", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_PF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jl", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF Or EFL_OF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jnl", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF Or EFL_OF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jng", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF Or EFL_OF Or EFL_ZF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jg", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF Or EFL_OF Or EFL_ZF, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g1", AM_E Or OT_b, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g1", AM_E Or OT_v, AM_I Or OT_v, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g1", AM_E Or OT_b, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g1", AM_E Or OT_v, AM_I Or OT_b Or F_s, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_E Or OT_b Or P_r, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_E Or OT_v Or P_r, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_w, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_w, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_G Or OT_b Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVSR), "mov", AM_E Or OT_v Or P_w, AM_S Or OT_w Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LEA), "lea", AM_G Or OT_v Or P_w, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVSR), "mov", AM_S Or OT_w Or P_w, AM_E Or OT_w Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "nop", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_ECX Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_EDX Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_EBX Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_ESP Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_EBP Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_ESI Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XCHG), "xchg", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_EDI Or OT_v Or P_w, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cbw", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, REG_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cwd", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, REG_EAX Or REG_EDX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CALL), "callf", AM_A Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "wait", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "pushf", FLAGS_NONE Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "popf", FLAGS_NONE Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "sahf", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF Or EFL_CF, 0, IOP_EAX, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "lahf", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, IOP_EAX, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EAX Or OT_b Or P_w, AM_O Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EAX Or OT_v Or P_w, AM_O Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_O Or OT_v Or P_w, AM_REG Or REG_EAX Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_O Or OT_v Or P_w, AM_REG Or REG_EAX Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVS), "movsb", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, EFL_DF, IOP_EDI Or IOP_ESI, IOP_EDI Or IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVS), "movsd", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, EFL_DF, IOP_EDI Or IOP_ESI, IOP_EDI Or IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMPS), "cmpsb", FLAGS_NONE Or P_r, FLAGS_NONE Or P_r, FLAGS_NONE, 0, EFL_ALL_COMMON, EFL_DF, IOP_EDI Or IOP_ESI, IOP_EDI Or IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMPS), "cmpsd", FLAGS_NONE Or P_r, FLAGS_NONE Or P_r, FLAGS_NONE, 0, EFL_ALL_COMMON, EFL_DF, IOP_EDI Or IOP_ESI, IOP_EDI Or IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_REG Or REG_EAX Or OT_b Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_REG Or REG_EAX Or OT_v Or P_r, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_STOS), "stosb", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_DF, IOP_EDI, IOP_EAX Or IOP_EDI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_STOS), "stosd", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_DF, IOP_EDI, IOP_EAX Or IOP_EDI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LODS), "lodsb", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_DF, IOP_EAX Or IOP_ESI, IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LODS), "lodsd", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_DF, IOP_EAX Or IOP_ESI, IOP_ESI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SCAS), "scasb", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_ALL_COMMON, EFL_DF, IOP_EDI, IOP_EAX Or IOP_EDI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SCAS), "scasd", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_ALL_COMMON, EFL_DF, IOP_EDI, IOP_EAX Or IOP_EDI), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_AL Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_CL Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_DL Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_BL Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_AH Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_CH Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_DH Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_BH Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_ECX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EDX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EBX Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_ESP Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EBP Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_ESI Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_REG Or REG_EDI Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g2", AM_E Or OT_b, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g2", AM_E Or OT_v, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_RET), "retn", AM_I Or OT_w Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_RET), "ret", FLAGS_NONE Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LFP), "les", AM_G Or OT_v Or P_w, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LFP), "lds", AM_G Or OT_v Or P_w, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOV), "mov", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ENTER), "enter", AM_I Or OT_w Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, IOP_ESP Or IOP_EBP, IOP_ESP Or IOP_EBP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "leave", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP Or IOP_EBP, IOP_ESP Or IOP_EBP), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_RET), "retf", AM_I Or OT_w Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "retf", FLAGS_NONE Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INT), "int3", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INT), "int", AM_I Or OT_b Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INT), "into", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "iret", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g2", AM_E Or OT_b, AM_I1 Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g2", AM_E Or OT_v, AM_I1 Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g2", AM_E Or OT_b, AM_REG Or REG_CL Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g2", AM_E Or OT_v, AM_REG Or REG_CL Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ASC), "aam", AM_I Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 0, EFL_SF Or EFL_ZF Or EFL_PF, 0, IOP_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ASC), "aad", AM_I Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 0, EFL_SF Or EFL_ZF Or EFL_PF, 0, IOP_EAX, IOP_EAX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "salc", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XLAT), "xlat", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX, IOP_EAX Or IOP_EBX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "esc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LOOP), "loopne", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF, IOP_ECX, IOP_ECX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LOOP), "loope", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF, IOP_ECX, IOP_ECX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LOOP), "loop", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ECX, IOP_ECX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JECXZ), "jecxz", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, IOP_ECX), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "in", AM_REG Or REG_AL Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "in", AM_REG Or REG_EAX Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "out", AM_I Or OT_b Or P_w, AM_REG Or REG_AL Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "out", AM_I Or OT_b Or P_w, AM_REG Or REG_EAX Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CALL), "call", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMP), "jmp", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMP), "jmpf", AM_A Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMP), "jmp", AM_J Or OT_b Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "in", AM_REG Or REG_EAX Or OT_b Or P_w, AM_REG Or REG_EDX Or OT_w Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "in", AM_REG Or REG_EAX Or OT_v Or P_w, AM_REG Or REG_EDX Or OT_w Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "out", AM_REG Or REG_EDX Or OT_w Or P_w, AM_REG Or REG_EAX Or OT_b Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "out", AM_REG Or REG_EDX Or OT_w Or P_w, AM_REG Or REG_EAX Or OT_v Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "ext", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "int1", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "ext", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "ext", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "hlt", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cmc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_CF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g3", AM_E Or OT_b, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g3", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "clc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_CF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "stc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_CF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cli", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_IF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "sti", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_IF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CLD), "cld", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_DF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_STD), "std", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, EFL_DF, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g4", AM_E Or OT_b, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
    New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g5", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0)}

' 2-byte instructions
' groups 12-14

' XXX: check addressing mode, Intel manual is a little bit confusing...

' XXX: group 10 / invalid opcode?

' XXX: check operand types
    Public Shared inst_table2 As INST() = New INST() { _
  New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER),  "g6",        AM_E or OT_w,                   FLAGS_NONE,                FLAGS_NONE,   1, 0, 0, 0, 0 ), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g7", AM_M Or OT_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "lar", AM_G Or OT_v Or P_w, AM_E Or OT_w Or P_r, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "lsl", AM_G Or OT_v Or P_w, AM_E Or OT_w Or P_r, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "loadall286", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "clts", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "loadall", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "invd", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "wbinvd", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "ud2", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movups", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movups", AM_W Or OT_ps Or P_w, AM_V Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movlps", AM_V Or OT_q Or P_w, AM_M Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movlps", AM_M Or OT_q Or P_w, AM_V Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "unpcklps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "unpcklps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movhps", AM_V Or OT_q Or P_w, AM_M Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movhps", AM_M Or OT_q Or P_w, AM_V Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "mov", AM_R Or OT_d Or P_w, AM_C Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "mov", AM_R Or OT_d Or P_w, AM_D Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "mov", AM_C Or OT_d Or P_w, AM_R Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "mov", AM_D Or OT_d Or P_w, AM_R Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "mov", AM_R Or OT_d Or P_w, AM_T Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "mov", AM_T Or OT_d Or P_w, AM_R Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movaps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movaps", AM_W Or OT_ps Or P_w, AM_V Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "cvtpi2ps", AM_V Or OT_ps Or P_r, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movntps", AM_M Or OT_ps Or P_w, AM_V Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "cvttps2pi", AM_P Or OT_q Or P_r, AM_W Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "cvtps2pi", AM_P Or OT_q Or P_r, AM_W Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "ucomiss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "comiss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_w, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "wrmsr", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_EDX, IOP_ECX), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "rdtsc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_EDX, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "rdmsr", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_EDX, IOP_ECX), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "rdpmc", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_EDX, IOP_ECX), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SYSENTER), "sysenter", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "sysexit", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovo", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovno", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovb", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovae", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmove", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovne", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovbe", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmova", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovs", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovns", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovp", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovnp", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovl", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovge", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovle", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVC), "cmovg", AM_G Or OT_v Or P_w, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "movmskps", AM_G Or OT_d Or P_w, AM_V Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "sqrtps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "rsqrtps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "rcpps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "andps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "andnps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "orps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "xorps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "addps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "mulps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "cvtps2pd", AM_V Or OT_pd Or P_r, AM_W Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "cvtdq2ps", AM_V Or OT_ps Or P_r, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "subps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "minps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "divps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "maxps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "punpcklbw", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "punpcklwd", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "punpckldq", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "packusdw", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pcmpgtb", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pcmpgtw", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pcmpgtd", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "packsswb", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "punpckhbw", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "punpckhbd", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "punpckhdq", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "packssdw", AM_P Or OT_q Or P_w, AM_Q Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "movd", AM_P Or OT_d Or P_w, AM_E Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "movq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pshufw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "g12", AM_P Or OT_q, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "g13", AM_P Or OT_q, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "g14", AM_P Or OT_q, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pcmpeqb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pcmpeqw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pcmpeqd", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "emms", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "movd", AM_E Or OT_d Or P_w, AM_P Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "movq", AM_Q Or OT_q Or P_w, AM_P Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jo", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jno", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jc", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_CF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jnc", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_CF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jz", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jnz", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jna", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_CF Or EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "ja", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_CF Or EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "js", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jns", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jp", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_PF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jpo", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_PF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jl", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jnl", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jng", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF Or EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMPC), "jg", AM_J Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 0, 0, EFL_ZF Or EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "seto", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setno", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setb", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_CF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setnb", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_CF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setz", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setnz", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setbe", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_CF Or EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setnbe", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_CF Or EFL_ZF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "sets", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_SF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setns", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_SF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setp", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_PF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setnp", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_PF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setl", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setnl", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setle", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_ZF Or EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SETC), "setnle", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, EFL_ZF Or EFL_SF Or EFL_OF, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_FS Or F_r Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_FS Or F_r Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cpuid", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_EBX Or IOP_ECX Or IOP_EDX, IOP_EAX), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BT), "bt", AM_E Or OT_v Or P_r, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "shld", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, AM_I Or OT_b Or P_r, 1, EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF Or EFL_OF Or EFL_AF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "shld", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, AM_REG Or REG_ECX Or OT_b Or P_r, 1, EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF Or EFL_OF Or EFL_AF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_REG Or REG_GS Or F_r Or P_r, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_POP), "pop", AM_REG Or REG_GS Or F_r Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_ESP, IOP_ESP), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "rsm", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BTS), "bts", AM_E Or OT_v Or P_r, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "shrd", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, AM_I Or OT_b Or P_r, 1, EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF Or EFL_OF Or EFL_AF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "shrd", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_r, AM_REG Or REG_ECX Or OT_b Or P_r, 1, EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF Or EFL_OF Or EFL_AF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "grp15", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_EIMUL), "imul", AM_G Or OT_v Or P_r, AM_E Or OT_v Or P_r, FLAGS_NONE Or P_r, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cmpxchg", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_w, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, IOP_EAX, IOP_EAX), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "cmpxchg", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_w, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, IOP_EAX, IOP_EAX), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LFP), "lss", AM_G Or OT_v Or P_w, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BTR), "btr", AM_E Or OT_v Or P_r, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LFP), "lfs", AM_G Or OT_v Or P_w, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_LFP), "lgs", AM_G Or OT_v Or P_w, AM_M Or OT_v Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVZX), "movzx", AM_G Or OT_v Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVZX), "movzx", AM_G Or OT_v Or P_w, AM_E Or OT_w Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g8", AM_E Or OT_v, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BTC), "btc", AM_E Or OT_v Or P_r, AM_G Or OT_v Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSF), "bsf", AM_G Or OT_v Or P_r, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSR), "bsr", AM_G Or OT_v Or P_r, AM_E Or OT_v Or P_r, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVSX), "movsx", AM_G Or OT_v Or P_w, AM_E Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MOVSX), "movsx", AM_G Or OT_v Or P_w, AM_E Or OT_w Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XADD), "xadd", AM_E Or OT_b Or P_w, AM_G Or OT_b Or P_w, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XADD), "xadd", AM_E Or OT_v Or P_w, AM_G Or OT_v Or P_w, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "cmpps", AM_V Or OT_ps Or P_r, AM_W Or OT_ps Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "movnti", AM_M Or OT_d Or P_w, AM_G Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "pinsrw", AM_P Or OT_w Or P_w, AM_E Or OT_w Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "pextrv", AM_G Or OT_w Or P_w, AM_P Or OT_w Or P_r, AM_I Or OT_b Or P_r, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE Or TYPE_3), "shufps", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, AM_I Or OT_b Or P_r, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "g9", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_EAX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_ECX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_EDX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_EBX Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_ESP Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_EBP Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_ESI Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BSWAP), "bswap", AM_REG Or REG_EDI Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psrlw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psrld", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psrlq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmullw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmovmskb", AM_G Or OT_q Or P_w, AM_P Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubusb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubusw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pminub", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pand", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddusb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddusw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmaxsw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pandn", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pavgb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psraw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psrad", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pavgw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmulhuw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmulhw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "movntq", AM_M Or OT_q Or P_w, AM_V Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubsb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubsw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pminsw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "por", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddsb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddsw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmaxsw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pxor", AM_P Or OT_q, AM_Q Or OT_q, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER Or TYPE_3), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psllw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pslld", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psllq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmuludq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "pmaddwd", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psadbw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "maskmovq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubd", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "psubq", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddb", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddw", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX Or TYPE_3), "paddd", AM_P Or OT_q Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' 3-byte instructions, prefix 0x66

    ' Yeah, I know, it's waste to use a full 256-instruction table but now
    ' I'm prepared for future Intel extensions ;-)
    ' groups 12-14
    Public Shared inst_table3_66 As INST() = New INST() { _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movupd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movupd", AM_W Or OT_pd Or P_w, AM_V Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movlpd", AM_V Or OT_q Or P_w, AM_M Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movlpd", AM_M Or OT_q Or P_w, AM_V Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "unpcklpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "unpcklpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movhpd", AM_V Or OT_q Or P_w, AM_M Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movhpd", AM_M Or OT_q Or P_w, AM_V Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movapd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movapd", AM_W Or OT_pd Or P_w, AM_V Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtpi2pd", AM_V Or OT_pd Or P_r, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movntpd", AM_M Or OT_pd Or P_w, AM_V Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvttpd2pi", AM_P Or OT_q Or P_r, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtpd2pi", AM_P Or OT_q Or P_r, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "ucomisd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "comisd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_w, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movmskpd", AM_G Or OT_d Or P_w, AM_V Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "sqrtpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "andpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "andnpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "orpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "xorpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "addpd", AM_V Or OT_pd Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "mulpd", AM_V Or OT_pd Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtpd2ps", AM_V Or OT_pd Or P_r, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtps2dq", AM_V Or OT_pd Or P_r, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "subpd", AM_V Or OT_pd Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "minpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "divpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "maxpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpcklbw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpcklwd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punockldq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "packusdw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pcmpgtb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pcmpgtw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pcmpgtd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "packsswb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpckhbw", AM_V Or OT_dq Or P_w, AM_Q Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpckhbd", AM_V Or OT_dq Or P_w, AM_Q Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpckhdq", AM_V Or OT_dq Or P_w, AM_Q Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "packssdw", AM_V Or OT_dq Or P_w, AM_Q Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpcklqdq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "punpckhqd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movd", AM_V Or OT_d Or P_w, AM_E Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movdqa", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pshufd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "g12", AM_P Or OT_dq, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "g13", AM_W Or OT_dq, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "g14", AM_W Or OT_dq, AM_I Or OT_b, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pcmpeqb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pcmpeqw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pcmpeqd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "haddpd", AM_V Or OT_pd, AM_W Or OT_pd, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "hsubpd", AM_V Or OT_pd, AM_W Or OT_pd, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movd", AM_E Or OT_d Or P_w, AM_V Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movdqa", AM_W Or OT_dq Or P_w, AM_V Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cmppd", AM_V Or OT_pd Or P_r, AM_W Or OT_pd Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pinsrw", AM_V Or OT_w Or P_w, AM_E Or OT_w Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pextrv", AM_G Or OT_w Or P_w, AM_V Or OT_w Or P_r, AM_I Or OT_b Or P_r, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "shufpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, AM_I Or OT_b Or P_r, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "addsubpd", AM_V Or OT_pd Or P_w, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrlw", AM_V Or OT_dq Or P_w, AM_Q Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrld", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrlq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmullw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movq", AM_W Or OT_q Or P_w, AM_V Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmovmskb", AM_G Or OT_d Or P_w, AM_V Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubusb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubusw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pminub", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pand", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddusb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddusw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmaxsw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pandn", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pavgb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psraw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrad", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pavgw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmulhuw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmulhw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvttpd2dq", AM_V Or OT_dq Or P_r, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movntq", AM_M Or OT_dq Or P_w, AM_V Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubsb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubsw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pminsw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "por", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddsb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddsw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmaxsw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pxor", AM_V Or OT_dq, AM_W Or OT_dq, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psllw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pslld", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psllq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmuludq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pmaddwd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psadbw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "maskmovdqu", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psubq", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddb", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "paddd", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' 3-byte instructions, prefix 0xf2
    Public Shared inst_table3_f2 As INST() = New INST() { _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movsd", AM_W Or OT_sd Or P_w, AM_V Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movddup", AM_V Or OT_q Or P_w, AM_W Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtsi2sd", AM_V Or OT_sd Or P_r, AM_E Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvttsd2si", AM_G Or OT_d Or P_r, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtsd2si", AM_G Or OT_d Or P_r, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "sqrtsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "addsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "mulsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtsd2ss", AM_V Or OT_ss Or P_r, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "subsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "minsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "divsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "maxsd", AM_V Or OT_sd Or P_w, AM_W Or OT_sd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pshuflw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "haddps", AM_V Or OT_ps, AM_W Or OT_ps, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "hsubps", AM_V Or OT_ps, AM_W Or OT_ps, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cmpsd", AM_V Or OT_sd Or P_r, AM_W Or OT_sd Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "addsubpd", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movdq2q", AM_P Or OT_q Or P_w, AM_V Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtpd2dq", AM_V Or OT_dq Or P_r, AM_W Or OT_pd Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "lddqu", AM_V Or OT_dq, AM_M Or OT_dq, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' 3-byte instructions, prefix 0xf3
    Public Shared inst_table3_f3 As INST() = New INST() { _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movss", AM_W Or OT_ss Or P_w, AM_V Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movsldup", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movshdup", AM_V Or OT_ps Or P_w, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtsi2ss", AM_V Or OT_ss Or P_r, AM_E Or OT_d Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvttss2si", AM_G Or OT_d Or P_r, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtss2si", AM_G Or OT_d Or P_r, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "sqrtss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "rsqrtss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "rcpss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "addss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "mulss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtsd2sd", AM_V Or OT_sd Or P_r, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvttps2dq", AM_V Or OT_dq Or P_r, AM_W Or OT_ps Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "subss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "minss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "divss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "maxss", AM_V Or OT_ss Or P_w, AM_W Or OT_ss Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movdqu", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pshufhw", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movq", AM_V Or OT_q Or P_w, AM_W Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movdqu", AM_V Or OT_dq Or P_w, AM_W Or OT_dq Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cmpss", AM_V Or OT_ss Or P_r, AM_W Or OT_ss Or P_r, AM_I Or OT_b, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "movq2dq", AM_V Or OT_dq Or P_w, AM_Q Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cvtdq2pd", AM_V Or OT_pd Or P_r, AM_W Or OT_q Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
 New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' Extension tables

    Public Shared inst_table_ext1_1 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_E Or OT_b Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0)}

    Public Shared inst_table_ext1_2 As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_E Or OT_v Or P_w, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_E Or OT_v Or P_r, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0)}

    Public Shared inst_table_ext1_3 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADD), "add", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OR), "or", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ADC), "adc", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SBB), "sbb", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_AND), "and", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SUB), "sub", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_MATH, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_XOR), "xor", AM_E Or OT_v Or P_w, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_BITWISE, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CMP), "cmp", AM_E Or OT_v Or P_r, AM_I Or OT_b Or F_s Or P_r, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, 0, 0)}

    Public Shared inst_table_ext2_1 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rol", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "ror", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcl", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcr", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shl", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shr", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sal", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sar", AM_E Or OT_b Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0)}

    Public Shared inst_table_ext2_2 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rol", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "ror", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcl", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcr", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shl", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shr", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sal", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sar", AM_E Or OT_v Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0)}

    Public Shared inst_table_ext2_3 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rol", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "ror", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcl", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcr", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shl", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shr", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sal", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sar", AM_E Or OT_b Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0)}

    Public Shared inst_table_ext2_4 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rol", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "ror", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcl", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcr", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shl", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shr", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sal", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sar", AM_E Or OT_v Or P_w, AM_I1 Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0)}

    Public Shared inst_table_ext2_5 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rol", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "ror", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcl", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcr", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shl", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shr", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sal", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sar", AM_E Or OT_b Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0)}

    Public Shared inst_table_ext2_6 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rol", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "ror", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcl", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_ROX), "rcr", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shl", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "shr", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sal", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SHX), "sar", AM_E Or OT_v Or P_w, AM_REG Or REG_CL Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0)}

    Public Shared inst_table_ext3_1 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_E Or OT_b Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_E Or OT_b Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_NOT), "not", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_NEG), "neg", AM_E Or OT_b Or P_w, FLAGS_NONE, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MUL), "mul", AM_E Or OT_b Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, IOP_EAX, IOP_EAX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_IMUL), "imul", AM_E Or OT_b Or P_r, FLAGS_NONE Or P_r, FLAGS_NONE Or P_r, 1, EFL_CF Or EFL_OF, 0, IOP_EAX, IOP_EAX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DIV), "div", AM_E Or OT_b Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, IOP_EAX, IOP_EAX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_IDIV), "idiv", AM_E Or OT_b Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, IOP_EAX, IOP_EAX)}

    Public Shared inst_table_ext3_2 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_E Or OT_v Or P_r, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_TEST), "test", AM_E Or OT_v Or P_r, AM_I Or OT_v Or P_r, FLAGS_NONE, 1, EFL_OF Or EFL_CF Or EFL_SF Or EFL_ZF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_NOT), "not", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_NEG), "neg", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MUL), "mul", AM_E Or OT_v Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, EFL_CF Or EFL_OF, 0, IOP_EAX Or IOP_EDX, IOP_EAX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_IMUL), "imul", AM_E Or OT_v Or P_r, FLAGS_NONE Or P_r, FLAGS_NONE Or P_r, 1, EFL_CF Or EFL_OF, 0, IOP_EAX Or IOP_EDX, IOP_EAX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DIV), "div", AM_E Or OT_v Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, IOP_EAX Or IOP_EDX, IOP_EAX Or IOP_EDX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_IDIV), "idiv", AM_E Or OT_v Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, IOP_EAX Or IOP_EDX, IOP_EAX Or IOP_EDX)}

    Public Shared inst_table_ext4 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_E Or OT_b Or P_r, FLAGS_NONE, FLAGS_NONE, 1, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_E Or OT_b, FLAGS_NONE, FLAGS_NONE, 1, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_ext5 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_INC), "inc", AM_E Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 1, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_DEC), "dec", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, EFL_OF Or EFL_SF Or EFL_ZF Or EFL_AF Or EFL_PF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CALL), "call", AM_E Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, IOP_ESP, IOP_ESP), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_CALL), "callf", AM_E Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, IOP_ESP, IOP_ESP), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMP), "jmp", AM_E Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_JMP), "jmpf", AM_E Or OT_v Or P_x, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PUSH), "push", AM_E Or OT_v Or P_r, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, IOP_ESP, IOP_ESP), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_ext6 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SLDT), "sldt", AM_E Or OT_w Or P_r, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "str", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "lldt", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "ltr", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "verr", AM_E Or OT_w Or P_r, FLAGS_NONE, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "verw", AM_E Or OT_w Or P_r, FLAGS_NONE, FLAGS_NONE, 1, EFL_ZF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_ext7 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SGDT), "sgdt", AM_M Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SIDT), "sidt", AM_M Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "lgdt", AM_M Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "lidt", AM_M Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "smsw", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
            New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "lmsw", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, IOP_EAX, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_PRIV), "invlpg", AM_M Or OT_b Or P_r, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0)}

    Public Shared inst_monitor As New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "monitor", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_ECX Or IOP_EDX, IOP_EAX Or IOP_ECX Or IOP_EDX)
    Public Shared inst_mwait As New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "mwait", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, IOP_EAX Or IOP_ECX, IOP_EAX Or IOP_ECX)

    Public Shared inst_table_ext8 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BT), "bt", AM_E Or OT_v Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BTS), "bts", AM_E Or OT_v Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BTR), "btr", AM_E Or OT_v Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_BTC), "btc", AM_E Or OT_v Or P_r, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, EFL_CF, 0, 0, 0)}

    Public Shared inst_table_ext9 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "cmpxch8b", AM_M Or OT_q, FLAGS_NONE, FLAGS_NONE, 1, EFL_ALL_COMMON, 0, IOP_EAX Or IOP_EDX, IOP_EAX Or IOP_EBX Or IOP_ECX Or IOP_EDX), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_ext10 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: not used yet
    Public Shared inst_table_ext11 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: intel manual says AM_P.. but that seems to produce wrong disasm
    Public Shared inst_table_ext12 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psrlw", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psraw", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psllw", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: intel manual says AM_P.. but that seems to produce wrong disasm
    Public Shared inst_table_ext12_66 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrlw", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psraw", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psllw", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: intel manual says AM_P.. but that seems to produce wrong disasm
    Public Shared inst_table_ext13 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psrld", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psrad", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "pslld", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: intel manual says AM_P.. but that seems to produce wrong disasm
    Public Shared inst_table_ext13_66 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrld", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrad", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pslld", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: intel manual says AM_P.. but that seems to produce wrong disasm
    Public Shared inst_table_ext14 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psrlq", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_MMX), "psllq", AM_Q Or OT_q Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: intel manual says AM_P.. but that seems to produce wrong disasm
    Public Shared inst_table_ext14_66 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrlq", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psrldq", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "psllq", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_SSE), "pslldq", AM_W Or OT_dq Or P_w, AM_I Or OT_b Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0)}

    Public Shared inst_table_ext15 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "fxsave", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "fxrstor", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "ldmxcsr", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "stmxcsr", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), "sfence", AM_E Or OT_v, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0)}

    Public Shared inst_table_ext16 As INST() = New INST() { _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
      New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_OTHER), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' Table of extension tables
    Public Shared inst_table_ext As INST()() = New INST()() {inst_table_ext1_1, inst_table_ext1_2, inst_table_ext1_3, inst_table_ext2_1, inst_table_ext2_2, inst_table_ext2_3, _
 inst_table_ext2_4, inst_table_ext2_5, inst_table_ext2_6, inst_table_ext3_1, inst_table_ext3_2, inst_table_ext4, _
 inst_table_ext5, inst_table_ext6, inst_table_ext7, inst_table_ext8, inst_table_ext9, inst_table_ext10, _
 inst_table_ext11, inst_table_ext12, inst_table_ext13, inst_table_ext14, inst_table_ext15, inst_table_ext16}

' FPU instruction tables

'
'         * Tables are composed in two parts:
'         *
'         * - 1st part (index 0-7) are identified by the reg field of MODRM byte
'         *   if the MODRM is < 0xc0. reg field can be used directly as an index to table.
'         *
'         * - 2nd part (8 - 0x47) are identified by the MODRM byte itself. In that case,
'         *   the index can be calculated by "index = MODRM - 0xb8"
'         *
'         

    Public Shared inst_table_fpu_d8 As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadds", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmuls", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcoms", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomps", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsubs", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubrs", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdivs", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivrs", AM_E Or OT_d Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcomp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_fpu_d9 As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "flds", AM_E Or OT_d, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fldenv", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fldcw", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fstenv", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fstcw", AM_E Or OT_v Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fld", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FXCH), "fxch", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fnop", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fchs", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fabs", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "ftst", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fxam", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fld1", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fldl2t", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fldl2e", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fldpi", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fldlg2", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fldln2", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fldz", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "f2xm1", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fyl2x", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fptan", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fpatan", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fxtract", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fprem1", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fdecstp", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fincstp", FLAGS_NONE Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fprem", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fyl2xp1", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fsqrt", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fsincos", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "frndint", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fscale", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fsin", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fcos", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_fpu_da As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIADD), "fiaddl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIMUL), "fimull", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FICOM), "ficoml", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FICOMP), "ficompl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISUB), "fisubl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISUBR), "fisubrl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIDIV), "fidivl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIDIVR), "fidivrl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmove", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucompp", FLAGS_NONE Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' XXX: fsetpm??
    Public Shared inst_table_fpu_db As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FILD), "fildl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISTTP), "fisttp", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIST), "fistl", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISTP), "fistp", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fldt", AM_E Or OT_t, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstpl", AM_E Or OT_t Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnb", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovne", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnbe", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCMOVC), "fcmovnu", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fclex", FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "finit", FLAGS_NONE Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMI), "fucomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMI), "fcomi", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_fpu_dc As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "faddl", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmull", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOM), "fcoml", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMP), "fcompl", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsubl", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubrl", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdivl", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivrl", AM_E Or OT_q Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADD), "fadd", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMUL), "fmul", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBR), "fsubr", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUB), "fsub", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVR), "fdivr", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIV), "fdiv", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_fpu_dd As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FLD), "fldl", AM_E Or OT_q, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISTTP), "fisttp", AM_E Or OT_q Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fstl", AM_E Or OT_q Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstpl", AM_E Or OT_q Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "frstor", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fsave", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU_CTRL), "fstsw", AM_E Or OT_d Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST0 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST1 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST2 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST3 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST4 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST5 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST6 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREE), "ffree", AM_REG Or REG_ST7 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST0 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST1 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST2 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST3 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST4 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST5 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST6 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FST), "fst", AM_REG Or REG_ST7 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST0 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST1 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST2 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST3 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST4 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST5 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST6 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSTP), "fstp", AM_REG Or REG_ST7 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOM), "fucom", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST0 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST1 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST2 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST3 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST4 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST5 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST6 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMP), "fucomp", AM_REG Or REG_ST7 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_fpu_de As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIADD), "fiadd", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIMUL), "fimul", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FICOM), "ficom", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FICOMP), "ficomp", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISUB), "fisub", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISUBR), "fisubr", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIDIV), "fidiv", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIDIVR), "fidivr", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FADDP), "faddp", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FMULP), "fmulp", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMPP), "fcompp", FLAGS_NONE Or P_w, FLAGS_NONE Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBRP), "fsubrp", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FSUBP), "fsubp", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVRP), "fdivrp", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST1 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST2 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST3 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST4 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST5 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST6 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FDIVP), "fdivp", AM_REG Or REG_ST7 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0)}

    Public Shared inst_table_fpu_df As INST() = New INST() { _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FILD), "fild", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISTTP), "fisttp", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FIST), "fist", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISTP), "fistp", AM_E Or OT_w Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fbld", AM_E Or OT_t Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FILD), "fild", AM_E Or OT_t Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fbstp", AM_E Or OT_t Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FISTP), "fistp", AM_E Or OT_t Or P_w, FLAGS_NONE, FLAGS_NONE, 1, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST0 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST1 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST2 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST3 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST4 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST5 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST6 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FFREEP), "ffreep", AM_REG Or REG_ST7 Or F_f Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), "fstsw", FLAGS_NONE Or P_w, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FUCOMIP), "fucomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST0 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST1 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST2 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST3 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST4 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST5 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST6 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FCOMIP), "fcomip", AM_REG Or REG_ST0 Or F_f Or P_w, AM_REG Or REG_ST7 Or F_f Or P_r, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0), _
     New INST(CUInt(InstructionEnum.INSTRUCTION_TYPE_FPU), Nothing, FLAGS_NONE, FLAGS_NONE, FLAGS_NONE, 0, 0, 0, 0, 0)}

    ' Table of FPU instruction tables

'
'         * These tables are accessed by the following way:
'         *
'         * INST *fpuinst = inst_table4[opcode - 0xd8][index];
'         * where index is determined by the MODRM byte.
'         *
'         

    Public Shared inst_table4 As INST()() = New INST()() {
 inst_table_fpu_d8, inst_table_fpu_d9, _
 inst_table_fpu_da, inst_table_fpu_db, _
 inst_table_fpu_dc, inst_table_fpu_dd, _
 inst_table_fpu_de, inst_table_fpu_df}
End Class