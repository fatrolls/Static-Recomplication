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

void sub_556D40()
{
    cpu.push_dword(cpu.reg[EBP]); // [556D40] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556D41] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556D43] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556D44] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556D4A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556D4E] or cl,0xff
    mem.write_word(mem.translate(data_section+7620), cpu.get_reg16(AX)); // [556D51] mov [0x583dc4],ax
    mem.write_byte(mem.translate(data_section+7622), cpu.get_reg8(CL)); // [556D57] mov [0x583dc6],cl
    mem.write_byte(mem.translate(data_section+7623), 0U); // [556D5D] mov byte [0x583dc7],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556D64] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556D66] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556D67] ret
    return; // [556D67] ret
}
