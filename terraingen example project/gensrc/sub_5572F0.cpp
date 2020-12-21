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

void sub_5572F0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5572F0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5572F1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5572F3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [5572F4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5572FA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [5572FE] or cl,0xff
    mem.write_word(mem.translate(data_section+8896), cpu.get_reg16(AX)); // [557301] mov [0x5842c0],ax
    mem.write_byte(mem.translate(data_section+8898), cpu.get_reg8(CL)); // [557307] mov [0x5842c2],cl
    mem.write_byte(mem.translate(data_section+8899), 0U); // [55730D] mov byte [0x5842c3],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [557314] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557316] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557317] ret
    return; // [557317] ret
}
