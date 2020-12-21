' struct INSTRUCTION is used to interface the library
Public Class LibDasmInstruction
    Public length As UInteger
    ' Instruction length
    Public type As InstructionEnum
    ' Instruction type
    Public mode As Mode
    ' Addressing mode
    Public opcode As Byte
    ' Actual opcode
    Public modrm As Byte
    ' MODRM byte
    Public sib As Byte
    ' SIB byte
    Public extindex As UInteger
    ' Extension table index
    Public fpuindex As Integer
    ' FPU table index
    Public dispbytes As UInteger
    ' Displacement bytes (0 = no displacement)
    Public immbytes As UInteger
    ' Immediate bytes (0 = no immediate)
    Public sectionbytes As Integer
    ' Section prefix bytes (0 = no section prefix)
    Public op1 As LibDasmOperand
    ' First operand (if any)
    Public op2 As LibDasmOperand
    ' Second operand (if any)
    Public op3 As LibDasmOperand
    ' Additional operand (if any)
    Public ptr As INST
    ' Pointer to instruction table
    Public flags As UInteger
    ' Process eflags affected
    Public eflags_affected As UShort
    ' Processor eflags used by this instruction
    Public eflags_used As UShort
    ' mask of affected implied registers (written)
    Public iop_written As UInteger
    ' mask of affected implied registers (read)
    Public iop_read As UInteger

    ' Instruction flags
    Public Overrides Function ToString() As String
        Dim dis As String = ""
        Try
            dis = LibDasm.get_instruction_string(Me, Format.FORMAT_INTEL, 0)
        Catch
            Return "FAIL"
        End Try
        Return dis
    End Function
End Class