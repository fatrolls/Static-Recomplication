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

void sub_556E60()
{
    cpu.push_dword(cpu.reg[EBP]); // [556E60] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556E61] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556E63] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556E64] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556E6A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556E6E] or cl,0xff
    mem.write_word(mem.translate(data_section+7656), cpu.get_reg16(AX)); // [556E71] mov [0x583de8],ax
    mem.write_byte(mem.translate(data_section+7658), cpu.get_reg8(CL)); // [556E77] mov [0x583dea],cl
    mem.write_byte(mem.translate(data_section+7659), 0U); // [556E7D] mov byte [0x583deb],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556E84] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556E86] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556E87] ret
    return; // [556E87] ret
}
