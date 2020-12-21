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

void sub_557320()
{
    cpu.push_dword(cpu.reg[EBP]); // [557320] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [557321] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [557323] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 51400U); // [557324] mov word [ebp-0x4],0xc8c8
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [55732A] mov ax,[ebp-0x4]
    mem.write_word(mem.translate(data_section+8900), cpu.get_reg16(AX)); // [55732E] mov [0x5842c4],ax
    mem.write_word(mem.translate(data_section+8902), 456U); // [557334] mov word [0x5842c6],0x1c8
    cpu.reg[ESP] = cpu.reg[EBP]; // [55733D] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [55733F] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [557340] ret
    return; // [557340] ret
}
