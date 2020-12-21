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

void sub_556A10()
{
    cpu.push_dword(cpu.reg[EBP]); // [556A10] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556A11] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556A13] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [556A14] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556A1A] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+7516), cpu.get_reg16(AX)); // [556A1E] mov [0x583d5c],ax
    mem.write_word(mem.translate(data_section+7518), 456U); // [556A24] mov word [0x583d5e],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [556A2D] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556A2F] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556A30] ret
    return; // [556A30] ret
}
