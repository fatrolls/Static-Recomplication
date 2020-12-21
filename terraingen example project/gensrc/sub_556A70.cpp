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

void sub_556A70()
{
    cpu.push_dword(cpu.reg[EBP]); // [556A70] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556A71] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556A73] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556A74] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556A7A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556A7E] or cl,0xff
    mem.write_word(mem.translate(data_section+7536), cpu.get_reg16(AX)); // [556A81] mov [0x583d70],ax
    mem.write_byte(mem.translate(data_section+7538), cpu.get_reg8(CL)); // [556A87] mov [0x583d72],cl
    mem.write_byte(mem.translate(data_section+7539), 0U); // [556A8D] mov byte [0x583d73],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556A94] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556A96] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556A97] ret
    return; // [556A97] ret
}
