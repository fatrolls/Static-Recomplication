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

void sub_556890()
{
    cpu.push_dword(cpu.reg[EBP]); // [556890] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556891] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556893] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [556894] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55689A] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [55689E] or cl,0xff
    mem.write_word(mem.translate(data_section+7436), cpu.get_reg16(AX)); // [5568A1] mov [0x583d0c],ax
    mem.write_byte(mem.translate(data_section+7438), cpu.get_reg8(CL)); // [5568A7] mov [0x583d0e],cl
    mem.write_byte(mem.translate(data_section+7439), 130U); // [5568AD] mov byte [0x583d0f],0x82
    cpu.reg[ESP] = cpu.reg[EBP]; // [5568B4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5568B6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5568B7] ret
    return; // [5568B7] ret
}
