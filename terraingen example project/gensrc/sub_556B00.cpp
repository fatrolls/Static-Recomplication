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

void sub_556B00()
{
    cpu.push_dword(cpu.reg[EBP]); // [556B00] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556B01] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556B03] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556B04] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556B0A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556B0E] or cl,0xff
    mem.write_word(mem.translate(data_section+7556), cpu.get_reg16(AX)); // [556B11] mov [0x583d84],ax
    mem.write_byte(mem.translate(data_section+7558), cpu.get_reg8(CL)); // [556B17] mov [0x583d86],cl
    mem.write_byte(mem.translate(data_section+7559), 0U); // [556B1D] mov byte [0x583d87],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [556B24] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556B26] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556B27] ret
    return; // [556B27] ret
}
