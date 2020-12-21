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

void sub_5568C0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5568C0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5568C1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5568C3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [5568C4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5568CA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [5568CE] or cl,0xff
    mem.write_word(mem.translate(data_section+7468), cpu.get_reg16(AX)); // [5568D1] mov [0x583d2c],ax
    mem.write_byte(mem.translate(data_section+7470), cpu.get_reg8(CL)); // [5568D7] mov [0x583d2e],cl
    mem.write_byte(mem.translate(data_section+7471), 0U); // [5568DD] mov byte [0x583d2f],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [5568E4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5568E6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5568E7] ret
    return; // [5568E7] ret
}
