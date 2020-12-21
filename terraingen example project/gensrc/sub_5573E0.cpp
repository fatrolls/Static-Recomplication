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

void sub_5573E0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5573E0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5573E1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5573E3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [5573E4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5573EA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [5573EE] or cl,0xff
    mem.write_word(mem.translate(data_section+8916), cpu.get_reg16(AX)); // [5573F1] mov [0x5842d4],ax
    mem.write_byte(mem.translate(data_section+8918), cpu.get_reg8(CL)); // [5573F7] mov [0x5842d6],cl
    mem.write_byte(mem.translate(data_section+8919), 130U); // [5573FD] mov byte [0x5842d7],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [557404] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557406] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557407] ret
    return; // [557407] ret
}
