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

void sub_556AD0()
{
    cpu.push_dword(cpu.reg[EBP]); // [556AD0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556AD1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556AD3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556AD4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556ADA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556ADE] or cl,0xff
    mem.write_word(mem.translate(data_section+7532), cpu.get_reg16(AX)); // [556AE1] mov [0x583d6c],ax
    mem.write_byte(mem.translate(data_section+7534), cpu.get_reg8(CL)); // [556AE7] mov [0x583d6e],cl
    mem.write_byte(mem.translate(data_section+7535), 130U); // [556AED] mov byte [0x583d6f],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [556AF4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556AF6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556AF7] ret
    return; // [556AF7] ret
}
