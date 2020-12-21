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

void sub_556F20()
{
    cpu.push_dword(cpu.reg[EBP]); // [556F20] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556F21] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556F23] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [556F24] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556F2A] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+8760), cpu.get_reg16(AX)); // [556F2E] mov [0x584238],ax
    mem.write_word(mem.translate(data_section+8762), 456U); // [556F34] mov word [0x58423a],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [556F3D] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556F3F] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556F40] ret
    return; // [556F40] ret
}
