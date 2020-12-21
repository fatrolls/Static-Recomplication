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

void sub_557470()
{
    cpu.push_dword(cpu.reg[EBP]); // [557470] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557471] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557473] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [557474] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55747A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55747E] or cl,0xff
    mem.write_word(mem.translate(data_section+8936), cpu.get_reg16(AX)); // [557481] mov [0x5842e8],ax
    mem.write_byte(mem.translate(data_section+8938), cpu.get_reg8(CL)); // [557487] mov [0x5842ea],cl
    mem.write_byte(mem.translate(data_section+8939), 130U); // [55748D] mov byte [0x5842eb],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [557494] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [557496] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557497] ret
    return; // [557497] ret
}
