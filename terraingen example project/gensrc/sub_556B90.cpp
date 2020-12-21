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

void sub_556B90()
{
    cpu.push_dword(cpu.reg[EBP]); // [556B90] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556B91] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556B93] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556B94] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556B9A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556B9E] or cl,0xff
    mem.write_word(mem.translate(data_section+7572), cpu.get_reg16(AX)); // [556BA1] mov [0x583d94],ax
    mem.write_byte(mem.translate(data_section+7574), cpu.get_reg8(CL)); // [556BA7] mov [0x583d96],cl
    mem.write_byte(mem.translate(data_section+7575), 0U); // [556BAD] mov byte [0x583d97],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556BB4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556BB6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556BB7] ret
    return; // [556BB7] ret
}
