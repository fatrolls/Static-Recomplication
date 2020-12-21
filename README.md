# Static-Recomplication
x86 Static Recomplication (Convert Compiled EXE Functions to Runnable/Callable Code in C++) in vb.net

Automatically creates a .cpp / .h file for every function found in EXE.

All Data sections like .rdata / .data of EXE file gets put into a huge array in `rdata_section.h` / `data_section.h`

Example of translated ASM code back to Runnable C++ code.
```
#include "main.h"
#include "functions.h"
#include "sections.h"
#include <iostream>

void sub_557380()
{
    cpu.push_dword(cpu.reg[EBP]); // [557380] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557381] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557383] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557384] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55738A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55738E] or cl,0xff
    mem.write_word(mem.translate(data_section+8920), cpu.get_reg16(AX)); // [557391] mov [0x5842d8],ax
    mem.write_byte(mem.translate(data_section+8922), cpu.get_reg8(CL)); // [557397] mov [0x5842da],cl
    mem.write_byte(mem.translate(data_section+8923), 0U); // [55739D] mov byte [0x5842db],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [5573A4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5573A6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5573A7] ret
    return; // [5573A7] ret
}
```
