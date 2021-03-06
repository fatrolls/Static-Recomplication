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

void sub_556E30()
{
    cpu.push_dword(cpu.reg[EBP]); // [556E30] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556E31] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556E33] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556E34] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556E3A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556E3E] or cl,0xff
    mem.write_word(mem.translate(data_section+7636), cpu.get_reg16(AX)); // [556E41] mov [0x583dd4],ax
    mem.write_byte(mem.translate(data_section+7638), cpu.get_reg8(CL)); // [556E47] mov [0x583dd6],cl
    mem.write_byte(mem.translate(data_section+7639), 130U); // [556E4D] mov byte [0x583dd7],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [556E54] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556E56] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556E57] ret
    return; // [556E57] ret
}
