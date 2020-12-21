/*
    Copyright (c) Mathias Kaerlev 2013-2014.

    This file is part of cuwo.

    cuwo is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    cuwo is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with cuwo.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "main.h"
#include "functions.h"
#include "sections.h"
#include <iostream>

void sub_557140()
{
    cpu.push_dword(cpu.reg[EBP]); // [557140] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557141] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557143] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557144] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55714A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55714E] or cl,0xff
    mem.write_word(mem.translate(data_section+8848), cpu.get_reg16(AX)); // [557151] mov [0x584290],ax
    mem.write_byte(mem.translate(data_section+8850), cpu.get_reg8(CL)); // [557157] mov [0x584292],cl
    mem.write_byte(mem.translate(data_section+8851), 0U); // [55715D] mov byte [0x584293],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [557164] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557166] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557167] ret
    return; // [557167] ret
}
