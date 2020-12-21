' Operands for the instruction
Public Class LibDasmOperand
    Public type As OperandEnum
    ' Operand type (register, memory, etc)
    Public reg As UInteger
    ' Register (if any)
    Public basereg As UInteger
    ' Base register (if any)
    Public indexreg As UInteger
    ' Index register (if any)
    Public scale As UInteger
    ' Scale (if any)
    Public dispbytes As UInteger
    ' Displacement bytes (0 = no displacement)
    Public dispoffset As UInteger
    ' Displacement value offset
    Public immbytes As UInteger
    ' Immediate bytes (0 = no immediate)
    Public immoffset As UInteger
    ' Immediate value offset
    Public sectionbytes As UInteger
    ' Section prefix bytes (0 = no section prefix)
    Public section As UShort
    ' Section prefix value
    Public displacement As UInteger
    ' Displacement value
    Public immediate As UInteger
    ' Immediate value
    Public flags As UInteger
    ' Operand flags
End Class
