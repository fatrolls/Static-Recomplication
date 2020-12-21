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

void sub_557020()
{
    cpu.push_dword(cpu.reg[EBP]); // [557020] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557021] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557023] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557024] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55702A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55702E] or cl,0xff
    mem.write_word(mem.translate(data_section+8816), cpu.get_reg16(AX)); // [557031] mov [0x584270],ax
    mem.write_byte(mem.translate(data_section+8818), cpu.get_reg8(CL)); // [557037] mov [0x584272],cl
    mem.write_byte(mem.translate(data_section+8819), 0U); // [55703D] mov byte [0x584273],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [557044] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557046] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557047] ret
    return; // [557047] ret
}
