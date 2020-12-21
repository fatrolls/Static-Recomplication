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

void sub_5569E0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5569E0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5569E1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5569E3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [5569E4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5569EA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [5569EE] or cl,0xff
    mem.write_word(mem.translate(data_section+7512), cpu.get_reg16(AX)); // [5569F1] mov [0x583d58],ax
    mem.write_byte(mem.translate(data_section+7514), cpu.get_reg8(CL)); // [5569F7] mov [0x583d5a],cl
    mem.write_byte(mem.translate(data_section+7515), 0U); // [5569FD] mov byte [0x583d5b],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556A04] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556A06] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556A07] ret
    return; // [556A07] ret
}
