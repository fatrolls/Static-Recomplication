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

void sub_556B30()
{
    cpu.push_dword(cpu.reg[EBP]); // [556B30] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [556B31] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [556B33] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [556B34] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [556B3A] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+7560), cpu.get_reg16(AX)); // [556B3E] mov [0x583d88],ax
    mem.write_word(mem.translate(data_section+7562), 456U); // [556B44] mov word [0x583d8a],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [556B4D] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [556B4F] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [556B50] ret
    return; // [556B50] ret
}
