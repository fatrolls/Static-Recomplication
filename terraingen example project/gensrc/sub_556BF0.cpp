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

void sub_556BF0()
{
    cpu.push_dword(cpu.reg[EBP]); // [556BF0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556BF1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556BF3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556BF4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556BFA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [556BFE] or cl,0xff
    mem.write_word(mem.translate(data_section+7568), cpu.get_reg16(AX)); // [556C01] mov [0x583d90],ax
    mem.write_byte(mem.translate(data_section+7570), cpu.get_reg8(CL)); // [556C07] mov [0x583d92],cl
    mem.write_byte(mem.translate(data_section+7571), 130U); // [556C0D] mov byte [0x583d93],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [556C14] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556C16] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556C17] ret
    return; // [556C17] ret
}
