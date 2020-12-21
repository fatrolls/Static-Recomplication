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

void sub_557350()
{
    cpu.push_dword(cpu.reg[EBP]); // [557350] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557351] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557353] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557354] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55735A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55735E] or cl,0xff
    mem.write_word(mem.translate(data_section+8892), cpu.get_reg16(AX)); // [557361] mov [0x5842bc],ax
    mem.write_byte(mem.translate(data_section+8894), cpu.get_reg8(CL)); // [557367] mov [0x5842be],cl
    mem.write_byte(mem.translate(data_section+8895), 130U); // [55736D] mov byte [0x5842bf],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [557374] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557376] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557377] ret
    return; // [557377] ret
}
