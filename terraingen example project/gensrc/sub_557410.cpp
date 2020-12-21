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

void sub_557410()
{
    cpu.push_dword(cpu.reg[EBP]); // [557410] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557411] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557413] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557414] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55741A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55741E] or cl,0xff
    mem.write_word(mem.translate(data_section+8940), cpu.get_reg16(AX)); // [557421] mov [0x5842ec],ax
    mem.write_byte(mem.translate(data_section+8942), cpu.get_reg8(CL)); // [557427] mov [0x5842ee],cl
    mem.write_byte(mem.translate(data_section+8943), 0U); // [55742D] mov byte [0x5842ef],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [557434] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557436] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557437] ret
    return; // [557437] ret
}
